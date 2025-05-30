using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Store
{
    [CreateAssetMenu(fileName = "New Powerup", menuName = "Store/Powerup")]
    public class Powerup : ScriptableObject
    {
        [SerializeField] public int Price;
        [SerializeField] public int Multiplier;
        [SerializeField] public int Amount;
        [SerializeField] public Item.ItemType TypeOfItem;
        [SerializeField] public int HatIndex;
    }
}
