using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject _store;
    [SerializeField] private TMP_Text _storeText;
    private MenuManager _menuManager;

    private bool _isPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        _isPlayerInRange = true;
        _storeText.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        _isPlayerInRange = false;
        CloseMenu();
    }

    public void CloseMenu()
    {
        _store.SetActive(false);
        _storeText.gameObject.SetActive(false);

        // Lock and hide the cursor when leaving the store
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        _menuManager = FindObjectOfType<MenuManager>();
    }

    void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            _store.SetActive(true);
            _menuManager.CloseMainMenu();
            _menuManager.CloseSettings();

            // Unlock and show the cursor when entering the store
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
