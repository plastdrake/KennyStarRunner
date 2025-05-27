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
            // Add money to the player via GameManager
            GameManager.Instance.AddMoney(coinValue);

            // Destroy the coin object after pickup
            Destroy(gameObject);
        }
    }
}