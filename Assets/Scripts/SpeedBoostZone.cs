using UnityEngine;

public class SpeedBoostZone : MonoBehaviour
{
    float boostSpeed = 12.0f;
    float duration = 2.0f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateSpeedBoost(boostSpeed, duration);
            }
        }
    }
}
