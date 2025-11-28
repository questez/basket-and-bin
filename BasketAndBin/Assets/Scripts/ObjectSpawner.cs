using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ObjectSpawner : MonoBehaviour
{
    private Camera arCamera; 

    [SerializeField] private GameObject basketRing;
    [SerializeField] private GameObject ball;

    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;    

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isBasketSpawned;
    private bool delayInUsingBalls;

    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;
    private float swipeStartTime;
    private bool isSwiping = false;
    private GameObject currentBall;

    [SerializeField] private float throwForceMultiplier = 0.001f; // множитель силы
    [SerializeField] private float minThrowForce = 2f;
    [SerializeField] private float maxThrowForce = 8f;
    [SerializeField] private float minSwipeDistance = 50f; // Минимальная дистанция свайпа в пикселях
    [SerializeField] private float upwardBias = 0.5f; // Смещение вверх для броска

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        isBasketSpawned = false;
        delayInUsingBalls = false;
    }

    private void Start()
    {
        arCamera = Camera.main;
    }

    private void OnEnable()
    {
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
    }

    private void Update()
    {
        if (!InGameMenu.PauseMode)
        {
            if (!isBasketSpawned)
            {
                SpawnBasketRing();
            }
            //else if (!delayInUsingBalls && isBasketSpawned)
            //{
            //    SpawnBasketBall();                                
            //}
        }            
    }
    
    private void SpawnBasketRing()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed) return;

        TouchControl touch = Touchscreen.current.primaryTouch;        

        if (touch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = touch.position.ReadValue();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                ARPlane plane = hits[0].trackable as ARPlane;

                var anchor = anchorManager.AttachAnchor(plane, hitPose);
                if (anchor != null)
                {
                    Instantiate(basketRing, anchor.transform.position, Quaternion.Euler(anchor.transform.eulerAngles.x, anchor.transform.eulerAngles.y - 90f, anchor.transform.eulerAngles.z));
                    isBasketSpawned = true;
                }
            }
        }        
    }

    private void SpawnBasketBall()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed) return;

        TouchControl touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = touch.position.ReadValue();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                ARPlane plane = hits[0].trackable as ARPlane;

                var anchor = anchorManager.AttachAnchor(plane, hitPose);
                if (anchor != null)
                {
                    Instantiate(ball, new Vector3(anchor.transform.position.x, anchor.transform.position.y + 2, anchor.transform.position.z), anchor.transform.rotation);
                    StartCoroutine(DelayInUse());
                }
            }
        }        
    }

    private void SpawnBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ball, arCamera.transform);
    }


    private IEnumerator DelayInUse()
    {
        delayInUsingBalls = true;
        yield return new WaitForSecondsRealtime(2f);
        delayInUsingBalls = false;
    }

    private void OnFingerDown(Finger finger)
    {
        if (!isBasketSpawned || delayInUsingBalls || InGameMenu.PauseMode) return;

        swipeStartPosition = finger.screenPosition;
        swipeStartTime = Time.time;
        isSwiping = true;

        SpawnBall();
    }

    private void OnFingerUp(Finger finger)
    {
        if (!isSwiping || currentBall == null) return;

        swipeEndPosition = finger.screenPosition;
        ProcessSwipeGesture();
        isSwiping = false;
    }

    private void ProcessSwipeGesture()
    {
        Vector2 swipeVector = swipeEndPosition - swipeStartPosition;
        float swipeDistance = swipeVector.magnitude;
        float swipeDuration = Time.time - swipeStartTime;        

        // РАСЧЕТ СКОРОСТИ СВАЙПА (пиксели в секунду)
        float swipeSpeed = swipeDistance / swipeDuration;

        // РАСЧЕТ СИЛЫ БРОСКА
        float throwForce = CalculateThrowForce(swipeSpeed, swipeDistance);

        // РАСЧЕТ НАПРАВЛЕНИЯ БРОСКА
        Vector3 throwDirection = CalculateThrowDirection(swipeVector);

        // Применяем бросок
        ExecuteThrow(throwDirection, throwForce);

        StartCoroutine(DelayInUse());
    }

    private float CalculateThrowForce(float swipeSpeed, float swipeDistance) // РАСЧЕТ СИЛЫ БРОСКА
    {
        // Комбинируем скорость и дистанцию для более точного расчета силы
        float combinedForce = (swipeSpeed * 0.7f) + (swipeDistance * 0.3f);
        float force = combinedForce * throwForceMultiplier;

        return Mathf.Clamp(force, minThrowForce, maxThrowForce);
    }

    private Vector3 CalculateThrowDirection(Vector2 swipeVector) // РАСЧЕТ НАПРАВЛЕНИЯ БРОСКА
    {
        // нормализуем вектор свайпа
        Vector2 normalizedSwipe = swipeVector.normalized;

        // преобразуем 2D вектор экрана в 3D направление в мире
        Vector3 worldDirection = new Vector3(
            normalizedSwipe.x,									// горизонтальное направление
            Mathf.Max(normalizedSwipe.y, upwardBias),			// вертикальное направление с смещением вверх
            Mathf.Abs(normalizedSwipe.y)						// заднее/переднее направление
        ).normalized;

        // Учитываем поворот камеры/игрока
        return arCamera.transform.TransformDirection(worldDirection);
    }

    private void ExecuteThrow(Vector3 direction, float force)
    {
        if (currentBall == null) return;

        // Убираем мяч из родителя
        currentBall.transform.SetParent(null);

        Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Для лучшего обнаружения столкновений

            // Применяем силу
            ballRb.AddForce(direction * force, ForceMode.Impulse);

            // Добавляем случайное вращение
            ballRb.AddTorque(Random.insideUnitSphere * force * 0.1f, ForceMode.Impulse);
        }
        Destroy(currentBall, 5);
        currentBall = null;
    }
}
