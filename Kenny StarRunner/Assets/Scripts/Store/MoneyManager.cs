using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshPro;

    void Start()
    {
        UpdateText();
    }

    public void Adjust(int amount)
    {
        if (GameManager.Instance != null)
        {
            if (amount > 0)
                GameManager.Instance.AddMoney(amount);
            else
                GameManager.Instance.SpendMoney(-amount); // SpendMoney expects positive value

            UpdateText();
        }
    }

    void UpdateText()
    {
        if (GameManager.Instance != null)
            _textMeshPro.text = GameManager.Instance.Money.ToString();
    }

    public int GetBalance()
    {
        return GameManager.Instance != null ? GameManager.Instance.Money : 0;
    }
}
