using System.Collections;
using UnityEngine;

public class IdleAnimationSelector : MonoBehaviour
{
    private Animator animator;
    private int currentIdleIndex = 0;
    private readonly string[] idleStates = { "Idle_0", "Idle_1", "Idle_2" };

    private Coroutine idleCoroutine;
    private bool wasIdle = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isIdle = animator.GetFloat("Speed") < 0.1f && !animator.GetBool("IsInAir");

        if (isIdle && !wasIdle)
        {
            // Just entered idle
            idleCoroutine = StartCoroutine(IdleLoop());
        }
        else if (!isIdle && wasIdle)
        {
            // Just exited idle
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }
            // Immediately force the animator to the correct state
            // (Optional: If you want to force the run state, you can do so here)
            // animator.Play("Run"); // Uncomment and set your run state name if needed
        }

        wasIdle = isIdle;
    }

    IEnumerator IdleLoop()
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            float waitTime = stateInfo.length - 0.2f;
            if (waitTime < 0.1f) waitTime = 0.5f;

            yield return new WaitForSeconds(waitTime);

            // Check again before switching idle
            bool isStillIdle = animator.GetFloat("Speed") < 0.1f && !animator.GetBool("IsInAir");
            if (!isStillIdle)
                yield break; // Exit coroutine if not idle

            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, idleStates.Length);
            }
            while (nextIndex == currentIdleIndex);

            currentIdleIndex = nextIndex;

            // Use Play for immediate switch
            animator.Play(idleStates[currentIdleIndex], 0, 0f);
        }
    }
}
