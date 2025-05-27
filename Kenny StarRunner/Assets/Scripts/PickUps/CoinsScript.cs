using UnityEngine;

public class CoinsScript : MonoBehaviour
{
    [Tooltip("How much money this coin is worth.")]
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player triggered the coin
        if (other.CompareTag("Player"))
        {
            // Try to get PlayerSounds from the player object
            PlayerSounds playerSounds = other.GetComponent<PlayerSounds>();
            if (playerSounds == null)
            {
                // Fallback: try to get from children (if needed)
                playerSounds = other.GetComponentInChildren<PlayerSounds>();
            }

            if (playerSounds != null)
            {
                playerSounds.CoinPickupSound();
            }
            else
            {
                Debug.LogWarning("CoinsScript: PlayerSounds component not found on player!");
            }

            // Add money to the player via GameManager
            GameManager.Instance.AddMoney(coinValue);

            // Destroy the coin object after pickup
            Destroy(gameObject);
        }
    }
}
