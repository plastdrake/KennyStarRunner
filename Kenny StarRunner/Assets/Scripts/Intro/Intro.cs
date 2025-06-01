using System.Collections;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public float waitTime = 50f;
    public float requiredHoldDuration = 2f;

    private bool sceneStarted = false;
    private float keyHoldTime = 0f;

    private void Start()
    {
        if (sceneLoader == null)
        {
            sceneLoader = Object.FindFirstObjectByType<SceneLoader>();
        }

        StartCoroutine(DelayedSceneLoad());
    }

    private void Update()
    {
        if (sceneStarted) return;

        if (Input.anyKey)
        {
            keyHoldTime += Time.deltaTime;

            if (keyHoldTime >= requiredHoldDuration)
            {
                StartScene();
            }
        }
        else
        {
            keyHoldTime = 0f; // Reset if key released
        }
    }

    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(waitTime);
        if (!sceneStarted)
        {
            StartScene();
        }
    }

    private void StartScene()
    {
        sceneStarted = true;
        sceneLoader.LoadScene();
    }
}
