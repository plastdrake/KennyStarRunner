using Assets.Scripts.Store;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject mainAchievementOverlay;
    public GameObject optionalAchievementOverlay;

    public void CompleteAchievement(Item.ItemType achievement)
    {
        switch (achievement)
        {
            case Item.ItemType.GalaxyHero when !mainAchievementOverlay.activeSelf:
                mainAchievementOverlay.SetActive(true);
                break;
            case Item.ItemType.Hairdresser when !optionalAchievementOverlay.activeSelf:
                optionalAchievementOverlay.SetActive(true);
                break;
        }
    }
}
