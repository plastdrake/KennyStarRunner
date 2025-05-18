using Assets.Scripts.Store;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> selectedPowerups = new List<Powerup>();

    public void SelectPowerup(Powerup powerup)
    {
        selectedPowerups.Add(powerup);
        Debug.Log($"Selected powerup: {powerup.TypeOfItem}");
    }

    public List<Powerup> GetPowerups() => selectedPowerups;
}
