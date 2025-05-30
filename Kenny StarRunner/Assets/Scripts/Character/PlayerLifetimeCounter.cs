using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerLifetimeCounter : MonoBehaviour
{
    public TMP_Text timerText;
    private int timeAlive = 0;
    private bool isAlive = true;

    [SerializeField] private RunnerCharacterController runnerController;

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