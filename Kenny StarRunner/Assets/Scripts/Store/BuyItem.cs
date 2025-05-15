using Assets.Scripts.Store;
using UnityEngine;

public class BuyItem : MonoBehaviour
{

    [SerializeField] private Powerup _powerup;

    public void BuySelectedItem_Coin()
    {
        _powerup.SetPowerup(2,1, Item.ItemType.Coin);
        Debug.Log("Clicked Coin");
    }

    public void BuySelectedItem_Heart()
    {
        _powerup.SetPowerup(1,2, Item.ItemType.Heart);
        Debug.Log("Clicked Heart");
    }
}
