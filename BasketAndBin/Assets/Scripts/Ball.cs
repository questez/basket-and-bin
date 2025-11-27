using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private AudioSource basketballFloorSound;

    private void Start()
    {
        basketballFloorSound.volume = AudioManager.TotalSoundVolume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            basketballFloorSound.Play();
        }        
    }
}
