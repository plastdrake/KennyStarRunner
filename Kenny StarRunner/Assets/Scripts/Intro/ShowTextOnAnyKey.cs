using UnityEngine;
using TMPro;
using System.Collections;

public class ShowTextOnAnyKey : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private Coroutine hideCoroutine;

    public float displayDuration = 2f;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.enabled = false;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (tmpText != null)
            {
                tmpText.enabled = true;

                // If already waiting to hide, restart the timer
                if (hideCoroutine != null)
                {
                    StopCoroutine(hideCoroutine);
                }

                hideCoroutine = StartCoroutine(HideAfterDelay());
            }
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        tmpText.enabled = false;
        hideCoroutine = null;
    }
}
