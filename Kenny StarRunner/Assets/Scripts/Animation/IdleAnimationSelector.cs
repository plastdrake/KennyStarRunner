using System.Collections;
using UnityEngine;

public class IdleAnimationSelector : MonoBehaviour
{
    private Animator animator;
    private int currentIdleIndex = 0;
    private readonly string[] idleStates = { "Idle_0", "Idle_1", "Idle_2" };

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(IdleLoop());
    }

    IEnumerator IdleLoop()
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Wait until near the end of the current state
            float waitTime = stateInfo.length - 0.2f;
            if (waitTime < 0.1f) waitTime = 0.5f; // fallback wait time

            yield return new WaitForSeconds(waitTime);

            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, idleStates.Length);
            }
            while (nextIndex == currentIdleIndex); // ensure different idle

            currentIdleIndex = nextIndex;

            // Smoothly blend to the new idle animation
            animator.CrossFade(idleStates[currentIdleIndex], 0.2f);
        }
    }
}
