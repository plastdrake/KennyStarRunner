using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;

    void Start()
    {
        // Optionally auto-find the player by tag if not assigned
        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                targetPlayer = playerObj.transform;
        }
    }

    void LateUpdate()
    {
        if (targetPlayer != null)
        {
            Vector3 targetPos = targetPlayer.position;
            Vector3 direction = targetPos - transform.position;
            direction.y = 0f; // Ignore vertical difference

            if (direction.sqrMagnitude > 0.001f)
            {
                // Face the player, but rotate 180� if needed
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                // Add 180� around Y axis
                lookRotation *= Quaternion.Euler(0, 180f, 0);
                transform.rotation = lookRotation;
            }
        }
    }
}