using Assets.Scripts.Store;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> selectedPowerups = new List<Powerup>();
    private Powerup _equippedPowerUp;
    private List<BuyItem> allItems = new List<BuyItem>();

    public void Register(BuyItem item)
    {
        if (!allItems.Contains(item))
            allItems.Add(item);
    }
    public void SelectPowerup(Powerup powerup)
    {
        _equippedPowerUp = powerup;
        Debug.Log($"Selected powerup: {powerup.TypeOfItem}");
        UpdateAll();
        RefreshBalance();
    }
    private void UpdateAll()
    {
        foreach (var item in allItems)
        {
            item.UpdateVisuals(item.GetPowerup() == _equippedPowerUp);
        }
    }
    public void RefreshBalance()
    {
        foreach (var item in allItems)
        {
            item.CheckBalance();
        }
    }
}
