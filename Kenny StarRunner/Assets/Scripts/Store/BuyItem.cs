using Assets.Scripts.Store;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{

    [SerializeField] private PowerupManager powerupManager;
    [SerializeField] private Powerup powerupToBuy;
    [SerializeField] private Sprite sprite;
    private TextMeshProUGUI buttonText;
    private bool _hasBeenBought;
    void Start()
    {
        powerupManager.Register(this);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public Powerup GetPowerup() => powerupToBuy;

    public void Buy()
    {
        _hasBeenBought = true;
        powerupManager.SelectPowerup(powerupToBuy);
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
