using UnityEngine;
using System.Collections.Generic;

public class HatManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    private int equippedHatIndex = -1;

    public void ShowHatByIndex(int index)
    {
        if (index < 0 || index >= hats.Count)
        {
            HideAllHats();
            equippedHatIndex = -1;
            if (GameManager.Instance != null)
                GameManager.Instance.EquipHat(-1);
            return;
        }

        for (int i = 0; i < hats.Count; i++)
        {
            if (hats[i] != null)
                hats[i].SetActive(i == index);
        }
        equippedHatIndex = index;
        if (GameManager.Instance != null)
            GameManager.Instance.EquipHat(index);
    }

    public void HideAllHats()
    {
        foreach (var hat in hats)
        {
            if (hat != null)
                hat.SetActive(false);
        }
        equippedHatIndex = -1;
        if (GameManager.Instance != null)
            GameManager.Instance.EquipHat(-1);
    }

    public int GetEquippedHatIndex()
    {
        return equippedHatIndex;
    }

    void Start()
    {
        if (GameManager.Instance != null)
            equippedHatIndex = GameManager.Instance.EquippedHatIndex;
        else
            equippedHatIndex = -1;

        if (equippedHatIndex >= 0 && equippedHatIndex < hats.Count)
            ShowHatByIndex(equippedHatIndex);
        else
            HideAllHats();
    }
}
