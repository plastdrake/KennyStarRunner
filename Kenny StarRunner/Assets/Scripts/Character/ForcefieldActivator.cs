using UnityEngine;
using Assets.Scripts.Store; // For Item.ItemType

public class ForcefieldActivator : MonoBehaviour
{
    [SerializeField] private GameObject forcefieldObject; // Assign in Inspector

    void Start()
    {
        if (forcefieldObject == null)
        {
            Debug.LogWarning("ForcefieldActivator: No forcefieldObject assigned.");
            return;
        }

        bool active = false;
        if (GameManager.Instance != null && GameManager.Instance.EquippedTitle != null)
        {
            active = GameManager.Instance.EquippedTitle.TypeOfItem == Item.ItemType.GalaxyHero;
        }
        forcefieldObject.SetActive(active);
    }
}