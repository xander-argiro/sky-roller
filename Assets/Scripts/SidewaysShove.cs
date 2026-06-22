using UnityEngine;

public class SidewaysShove : MonoBehaviour
{
    float shoveSpeed = 4.0f;
    float duration = 1.0f;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SidewaysShove(shoveSpeed, duration);
            }
        }
    }
}
