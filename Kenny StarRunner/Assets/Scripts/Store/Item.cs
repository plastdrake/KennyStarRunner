using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Store
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Types")]
    public class Item
    {
        public enum ItemType
        {
            Heart,
            Coin
        }
    }
}
