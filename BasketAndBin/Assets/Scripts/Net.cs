using UnityEngine;

public class Net: MonoBehaviour
{
    private Player player = new Player();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            player.IncreaseScore();
        }
    }
}
