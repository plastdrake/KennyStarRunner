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

    public void Buy()
    {
        powerupManager.SelectPowerup(powerupToBuy);
        GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
        //GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0.6f, 0);
        GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        GetComponentInChildren<TextMeshProUGUI>().fontWeight = FontWeight.Light;
        GetComponent<Image>().sprite = sprite;
    }
}
