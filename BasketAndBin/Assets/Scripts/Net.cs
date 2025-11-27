using UnityEngine;

public class Net: MonoBehaviour
{
    [SerializeField] private AudioSource ringHitSound;

    private void Start()
    {
        ringHitSound.volume = AudioManager.TotalSoundVolume;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Player.IncreaseScore();
            Debug.Log("Score increased to: " + Player.GetScore());
            ringHitSound.Play();
        }
    }
}
