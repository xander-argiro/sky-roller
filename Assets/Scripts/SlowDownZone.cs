using UnityEngine;

public class SlowDownZone : MonoBehaviour
{
    float slowedSpeed = 6.0f;
    float duration = 2.0f;
    
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SlowDown(slowedSpeed, duration);
            }
        }
    }
}
