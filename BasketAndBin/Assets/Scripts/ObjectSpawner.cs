using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject basketRing;
    [SerializeField] private GameObject ball;

    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    //[SerializeField] private Camera ARcamera;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isBasketSpawned = false;

    private void Update()
    {
        if (!isBasketSpawned)
        {
            SpawnBasketRing();
        }
        else
        {
            SpawnBasketBall();
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
                }
            }
        }

        isBasketSpawned = true;
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
                }
            }
        }
    }

}
