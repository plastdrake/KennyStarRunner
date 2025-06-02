using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Store; // Needed for Powerup and Item.ItemType

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int startingMoney = 3000;

    // Custom cursor fields
    [SerializeField] private Texture2D customCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    // Session data
    public int EquippedHatIndex { get; private set; } = -1;
    public int Money { get; private set; } = 0;
    public int CurrentScore { get; private set; } = 0;
    public Item.ItemType? EquippedPowerupType { get; private set; } = null; // Nullable for "none"
    public Powerup EquippedPowerup { get; private set; } = null;
    public Powerup EquippedTitle { get; private set; } = null;

    // --- Event for powerup changes (can be left for future extensibility) ---
    public event Action OnEquippedPowerupChanged;

    // Top 3 high scores (persistent)
    private const string HighScore1Key = "HighScore1";
    private const string HighScore2Key = "HighScore2";
    private const string HighScore3Key = "HighScore3";

    public int[] HighScores { get; private set; } = new int[3];
    public List<int> Hats { get; } = new List<int>();
    public int HighScore => HighScores[0];

    public void SaveHat(int index)
    {
        Hats.Add(index);
        Debug.Log(index);
    }
    private void LoadHighScores()
    {
        HighScores[0] = PlayerPrefs.GetInt(HighScore1Key, 0);
        HighScores[1] = PlayerPrefs.GetInt(HighScore2Key, 0);
        HighScores[2] = PlayerPrefs.GetInt(HighScore3Key, 0);
    }

    private void SaveHighScores()
    {
        PlayerPrefs.SetInt(HighScore1Key, HighScores[0]);
        PlayerPrefs.SetInt(HighScore2Key, HighScores[1]);
        PlayerPrefs.SetInt(HighScore3Key, HighScores[2]);
        PlayerPrefs.Save();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Money = startingMoney;
            SetCustomCursor(); // Set the custom cursor at startup
            LoadHighScores();  // Load top 3 scores at startup
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Custom cursor methods
    public void SetCustomCursor()
    {
        if (customCursor != null)
            Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            SetCustomCursor();
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
        Debug.Log("Invoking OnEquippedPowerupChanged event");
        OnEquippedPowerupChanged?.Invoke();

        // --- Force update player health and UI directly ---
        ForceUpdatePlayerHealthAndUI();
    }

    public void ForceUpdatePlayerHealthAndUI()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            var health = playerObj.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("Force updating Health and HealthBar for player.");
                health.RefreshMaxHealthFromPowerup();
            }
            else
            {
                Debug.LogWarning("No Health component found on Player.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found.");
        }
    }

    public void EquipTitle(Powerup title)
    {
        EquippedTitle = title;

        // Activate Forcefield if GalaxyHero is equipped
        if (title != null && title.TypeOfItem == Item.ItemType.GalaxyHero)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Transform forcefield = playerObj.transform.Find("Forcefield");
                if (forcefield != null)
                {
                    forcefield.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            // Deactivate Forcefield if another title is equipped
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Transform forcefield = playerObj.transform.Find("Forcefield");
                if (forcefield != null)
                {
                    forcefield.gameObject.SetActive(false);
                }
            }
        }
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

    // Try to insert a new score into the top 3 high scores
    public void TrySetHighScore(int score)
    {
        for (int i = 0; i < HighScores.Length; i++)
        {
            if (score > HighScores[i])
            {
                // Shift lower scores down
                for (int j = HighScores.Length - 1; j > i; j--)
                {
                    HighScores[j] = HighScores[j - 1];
                }
                HighScores[i] = score;
                SaveHighScores();
                break;
            }
        }
    }

    // For compatibility: sets high score from CurrentScore
    public void TrySetHighScore()
    {
        TrySetHighScore(CurrentScore);
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
        EquippedTitle = null;
        // Reset other session data as needed
    }
}
