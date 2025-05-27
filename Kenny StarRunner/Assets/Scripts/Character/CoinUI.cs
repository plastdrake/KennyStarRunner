using UnityEngine;
using UnityEngine.UI; // For Text
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText; // Use this if using TextMeshPro

    void Start()
    {
        UpdateCoinText();
    }

    void Update()
    {
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null && GameManager.Instance != null)
        {
            coinText.text = $"Coins: {GameManager.Instance.Money}";
        }
    }
}
