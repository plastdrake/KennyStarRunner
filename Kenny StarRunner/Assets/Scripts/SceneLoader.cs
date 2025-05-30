using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Assign the CanvasGroup of your black image
    [SerializeField] private float fadeDuration = 1f;

    // Call this from a UI Button
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(FadeAndLoadScene());
        }
        else
        {
            Debug.LogWarning("SceneLoader: Scene name is empty!");
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Fade to black
        if (fadeCanvasGroup != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // Load scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Optional: Load scene when player enters a trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }
}
