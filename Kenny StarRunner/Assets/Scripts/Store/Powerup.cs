using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Store
{
    public class Powerup : MonoBehaviour
    {
        [SerializeField] public int Multiplier;
        [SerializeField] public int Amount;
        [SerializeField] public Item.ItemType TypeOfItem;
        private List<Powerup> _powerupList = new List<Powerup>();

        public List<Powerup> GetPowerups() => _powerupList;
        public void SetPowerup(int multiplier, int amount, Item.ItemType itemType)
        {
            this.Multiplier = multiplier;
            this.Amount = amount;
            this.TypeOfItem = itemType;
            _powerupList.Add(this);
        }
    }
}
