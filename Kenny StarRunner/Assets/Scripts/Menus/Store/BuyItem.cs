using Assets.Scripts.Store;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{

    [SerializeField] private GameObject _audioManager;
    [SerializeField] private PowerupManager powerupManager;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private Powerup powerupToBuy;
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject _price;
    private TextMeshProUGUI buttonText;
    private ShopSound _shopSounds;
    private Button btn;
    private bool _hasBeenBought;
    
    void Start()
    {
        _shopSounds = _audioManager.GetComponent<ShopSound>();
        powerupManager.Register(this);
        _price.GetComponentInChildren<TMP_Text>().text = powerupToBuy.Price.ToString();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        btn = GetComponentInChildren<Button>();
        CheckBalance();
    }
    public Powerup GetPowerup() => powerupToBuy;

    public void Buy()
    {
        Debug.Log("Costs: " + _price.GetComponentInChildren<TMP_Text>().text);
        if (!_hasBeenBought)
        {
            if (!CheckBalance())
            {
                if(_shopSounds is null) Debug.Log("Is null");
                _shopSounds?.NotEnoughMoneySound();
                return;
            }

            _hasBeenBought = true;
            _price.gameObject.SetActive(false);
            _moneyManager.Adjust(-powerupToBuy.Price);
            _shopSounds.BuySound();
        }

        powerupManager.SelectPowerup(powerupToBuy);
        powerupManager.RefreshBalance();
    }
    //TODO Flytta till MoneyManager
    public bool CheckBalance()
    {
        if (powerupToBuy.Price <= _moneyManager.GetBalance() || _hasBeenBought)
            return true;
        
        
        //btn.interactable = false;
        
        _price.GetComponentInChildren<TMP_Text>().color = new Color(0.5f,0,0);
        return false;

    }

    public void UpdateVisuals(bool isEquipped)
    {
        if (!_hasBeenBought) 
            return;

        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (isEquipped)
        {
            _shopSounds.EquipSound();
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
