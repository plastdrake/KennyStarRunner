using Assets.Scripts.Store;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{

    [SerializeField] private PowerupManager powerupManager;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private Powerup powerupToBuy;
    [SerializeField] private Sprite sprite;
    private TextMeshProUGUI buttonText;
    private Button btn;
    private bool _hasBeenBought;
    void Start()
    {
        powerupManager.Register(this);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        btn = GetComponentInChildren<Button>();
        CheckBalance();
    }
    public Powerup GetPowerup() => powerupToBuy;

    public void Buy()
    {
        _hasBeenBought = true;
        if (!CheckBalance()) return;
        powerupManager.SelectPowerup(powerupToBuy);
        _moneyManager.Adjust(-powerupToBuy.Price);
    }

    private bool CheckBalance()
    {
        if (powerupToBuy.Price <= _moneyManager.GetBalance()) return true;
        
        btn.interactable = false;
        return false;

    }

    public void UpdateVisuals(bool isEquipped)
    {
        if (!_hasBeenBought) return;
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (isEquipped)
        {
            buttonText.text = "Equipped";
            buttonText.color = new Color(0, 0.6f, 0);
        }
        else
        {
            buttonText.text = "Equip";
            buttonText.color = Color.white;
        }

        GetComponent<Image>().sprite = sprite;
    }

}
