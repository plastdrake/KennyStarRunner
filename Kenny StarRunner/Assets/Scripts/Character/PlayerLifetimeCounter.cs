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

    void Awake()
    {
        if (runnerController == null)
            runnerController = GetComponent<RunnerCharacterController>();
    }

    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (isAlive)
        {
            if (runnerController != null && !runnerController.IsFreeRunning)
            {
                timerText.text = timeAlive.ToString("D4");
                // Increase speed based on time survived
                float newSpeed = baseSpeed + (timeAlive * speedPerSecond);
                runnerController.forwardSpeed = Mathf.Min(newSpeed, maxSpeed);
                timeAlive++;
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
