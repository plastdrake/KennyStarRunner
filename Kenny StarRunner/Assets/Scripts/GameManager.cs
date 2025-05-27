using UnityEngine;
using Assets.Scripts.Store; // Needed for Powerup and Item.ItemType

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Session data
    public int EquippedHatIndex { get; private set; } = -1;
    public int Money { get; private set; } = 0;
    public int CurrentScore { get; private set; } = 0;
    public Item.ItemType? EquippedPowerupType { get; private set; } = null; // Nullable for "none"
    public Powerup EquippedPowerup { get; private set; } = null;

    // High score (persistent)
    private const string HighScoreKey = "HighScore";
    public int HighScore
    {
        get => PlayerPrefs.GetInt(HighScoreKey, 0);
        private set
        {
            PlayerPrefs.SetInt(HighScoreKey, value);
            PlayerPrefs.Save();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Money management
    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            int multiplier = 1;
            if (EquippedPowerup != null && EquippedPowerup.TypeOfItem == Item.ItemType.Coin)
                multiplier = EquippedPowerup.Multiplier;

            Money += amount * multiplier;
            // Optionally: Update UI or trigger events here
        }
    }

    public bool SpendMoney(int amount)
    {
        if (amount > 0 && Money >= amount)
        {
            Money -= amount;
            // Optionally: Update UI or trigger events here
            return true;
        }
        return false;
    }

    // Hat management
    public void EquipHat(int hatIndex)
    {
        EquippedHatIndex = hatIndex;
        // Optionally: Trigger events or update visuals here
    }

    // Powerup management
    public void EquipPowerup(Powerup powerup)
    {
        EquippedPowerup = powerup;
        EquippedPowerupType = powerup != null ? powerup.TypeOfItem : (Item.ItemType?)null;
        // Optionally: Trigger events or update visuals here
    }

    // Score management
    public void AddScore(int amount)
    {
        if (amount > 0)
        {
            CurrentScore += amount;
            // Optionally: Update UI or trigger events here
        }
    }

    public void TrySetHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }

    public void ResetSession()
    {
        Money = 0;
        EquippedHatIndex = -1;
        CurrentScore = 0;
        EquippedPowerupType = null;
        EquippedPowerup = null;
        // Reset other session data as needed
    }
}
