using UnityEngine;

public class Net: MonoBehaviour
{   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Player.IncreaseScore();
            Debug.Log("Score increased to: " + Player.GetScore());
        }
    }
}
