using Assets.Scripts.Store;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    public GameObject mainAchievementOverlay;
    public GameObject optionalAchievementOverlay;

    public void CheckAchievements()
    {
        if (GameManager.Instance is null) 
            return;

        foreach (var item in GameManager.Instance.Titles)
        {
            Debug.Log("Item: " + item);
        }
        if (GameManager.Instance.Titles.Contains(Item.ItemType.GalaxyHero))
        {
            CompleteAchievement(Item.ItemType.GalaxyHero);
        }
        if (GameManager.Instance.Titles.Contains(Item.ItemType.Hairdresser))
        {
            CompleteAchievement(Item.ItemType.Hairdresser);
        }
    }

    public void CompleteAchievement(Item.ItemType achievement)
    {
        switch (achievement)
        {
            case Item.ItemType.GalaxyHero when !mainAchievementOverlay.activeSelf:
                mainAchievementOverlay.SetActive(true);
                GameManager.Instance.GoToOutroScene();
                break;
            case Item.ItemType.Hairdresser when !optionalAchievementOverlay.activeSelf:
                optionalAchievementOverlay.SetActive(true);
                break;
        }
        if(!GameManager.Instance.Titles.Contains(achievement))
            GameManager.Instance.Titles.Add(achievement);
    }
}
