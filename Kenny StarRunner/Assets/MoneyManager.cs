using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshPro;
    public int _amount  = 1000;


    void Start()
    {
        UpdateText();
    }

    public void Adjust(int amount)
    {
        _amount += amount;
        UpdateText();
    }

    void UpdateText()
    {
        _textMeshPro.text = _amount.ToString();
    }

    public int GetBalance() => _amount;
}
