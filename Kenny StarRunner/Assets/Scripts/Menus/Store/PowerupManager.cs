using Assets.Scripts.Store;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private Powerup _equippedPowerUp;
    private Powerup _equippedTitle;
    private List<BuyItem> allItems = new List<BuyItem>();
    private HatManager hatManager;
    public AchievementManager achievementManager;

    public void Register(BuyItem item)
    {
        if (!allItems.Contains(item))
            allItems.Add(item);
    }

    public void SelectPowerup(Powerup powerup)
    {
        if (powerup.TypeOfItem == Item.ItemType.GalaxyHero || powerup.TypeOfItem == Item.ItemType.Hairdresser)
            _equippedTitle = powerup;
        else
            _equippedPowerUp = powerup;

        UpdateAll();
        RefreshBalance();

        // Save equipped powerup type in GameManager (if available)
        if (GameManager.Instance != null && powerup != null)
        {
            GameManager.Instance.SaveHat(powerup.HatIndex);

            if (powerup.TypeOfItem == Item.ItemType.Hairdresser || powerup.TypeOfItem == Item.ItemType.GalaxyHero)
            {
                GameManager.Instance.EquipTitle(powerup);
                achievementManager.CompleteAchievement(powerup.TypeOfItem);
            }
            else
            {
                GameManager.Instance.EquipPowerup(powerup);

                // Find HatManager if needed
                if (hatManager == null)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        hatManager = player.GetComponent<HatManager>();
                    }
                }

                // Use Powerup.HatIndex for hat selection
                if (hatManager != null && powerup != null)
                {
                    int hatIndex = powerup.HatIndex; // Use the explicit index from Powerup asset
                    hatManager.ShowHatByIndex(hatIndex);
                }
            }
        }

    }

    private void UpdateAll()
    {
        foreach (var item in allItems)
        {
            var powerup = item.GetPowerup();

            if (powerup.TypeOfItem == Item.ItemType.GalaxyHero || powerup.TypeOfItem == Item.ItemType.Hairdresser)
            {
                item.UpdateVisuals(powerup == _equippedTitle);
            }
            else
            {
                item.UpdateVisuals(powerup == _equippedPowerUp);
            }
        }
    }

    public void RefreshBalance()
    {
        foreach (var item in allItems)
            item.CheckBalance();
    }
}
