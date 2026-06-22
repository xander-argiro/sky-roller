using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public static bool GAME_OVER = false;
    public TMP_Text gameOverText;
    public GameObject restartButton;
    public GameObject quitButton;

    float speedForward = 8.0f;
    float sideSpeed = 6.0f;

    float shove = 0;

    Rigidbody rb;

    Vector2 moveInput;

    float currentSideInput;
    float sideVelocity;

    float baseSpeedForward;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        baseSpeedForward = speedForward;

        shove = 1;

        gameOverText.gameObject.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void ActivateSpeedBoost(float boostSpeed, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boostSpeed, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostSpeed, float duration)
    {
        speedForward = boostSpeed;
        yield return new WaitForSeconds(duration);
        speedForward = baseSpeedForward;
    }

    public void SlowDown(float slowedSpeed, float duration)
    {
        StartCoroutine(SlowDownRoutine(slowedSpeed, duration));
    }

    private IEnumerator SlowDownRoutine(float slowedSpeed, float duration)
    {
        speedForward = slowedSpeed;
        yield return new WaitForSeconds(duration);
        speedForward = baseSpeedForward;
    }

    public void SidewaysShove(float shoveSpeed, float duration)
    {
        StartCoroutine(SidewaysShoveRoutine(shoveSpeed, duration));
    }

    private IEnumerator SidewaysShoveRoutine(float shoveSpeed, float duration)
    {
        shove = shoveSpeed;
        yield return new WaitForSeconds(duration);
        shove = 0;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (!GAME_OVER)
        {
            currentSideInput = Mathf.SmoothDamp(
                currentSideInput,
                moveInput.x,
                ref sideVelocity,
                0.1f
            );

            Vector3 movement = new Vector3(
                currentSideInput * sideSpeed + shove,
                rb.linearVelocity.y,
                speedForward
            );

            rb.linearVelocity = movement;
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.SetActive(true);
            quitButton.SetActive(true);
        }
    }
}
