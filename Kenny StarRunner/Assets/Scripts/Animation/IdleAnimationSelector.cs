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
            currentIdleIndex = 0;
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
            // Instantly force the animator to the correct state
            animator.CrossFade("Run", 0f, 0, 0f); // Replace "Run" with your actual run state name if needed
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

            bool isStillIdle = animator.GetFloat("Speed") < 0.1f && !animator.GetBool("IsInAir");
            if (!isStillIdle)
                yield break;

            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, idleStates.Length);
            }
            while (nextIndex == currentIdleIndex);

            currentIdleIndex = nextIndex;

            animator.CrossFade(idleStates[currentIdleIndex], 0.2f, 0, 0f);
        }
    }
}
