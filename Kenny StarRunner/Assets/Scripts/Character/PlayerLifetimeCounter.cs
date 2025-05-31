using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerLifetimeCounter : MonoBehaviour
{
    public TMP_Text timerText;
    private int timeAlive = 0;
    private bool isAlive = true;

    [SerializeField] private RunnerCharacterController runnerController;

    [SerializeField] private float baseSpeed = 16f;
    [SerializeField] private float speedPerSecond = 0.1f;
    [SerializeField] private float maxSpeed = 40f;

    [Header("Movement Check")]
    [Tooltip("Minimum distance the player must move per second to count as alive.")]
    [SerializeField] private float minMoveDistancePerSecond = 0.5f;

    private Vector3 lastPosition;

    void Awake()
    {
        if (runnerController == null)
            runnerController = GetComponent<RunnerCharacterController>();
    }

    void Start()
    {
        lastPosition = transform.position;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (isAlive)
        {
            if (runnerController != null && !runnerController.IsFreeRunning)
            {
                float distanceMoved = Vector3.Distance(transform.position, lastPosition);

                if (distanceMoved >= minMoveDistancePerSecond)
                {
                    timerText.text = timeAlive.ToString("D4");
                    // Increase speed based on time survived
                    float newSpeed = baseSpeed + (timeAlive * speedPerSecond);
                    runnerController.forwardSpeed = Mathf.Min(newSpeed, maxSpeed);
                    timeAlive++;
                }

                lastPosition = transform.position;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnPlayerDeath()
    {
        isAlive = false;
        // Set high score if this run is better
        if (GameManager.Instance != null)
            GameManager.Instance.TrySetHighScore(timeAlive);
    }
}
