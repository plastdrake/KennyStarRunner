using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject _store;
    [SerializeField] private TMP_Text _storeText;
    private MenuManager _menuManager;
    private InputSystem_Actions storeAction;
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

    void Awake()
    {
        _menuManager = FindFirstObjectByType<MenuManager>();
        storeAction = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        storeAction.Enable();
        storeAction.Player.OpenStore.performed += OnInteract;
    }

    private void OnDisable()
    {
        storeAction.Player.OpenStore.performed -= OnInteract;
        storeAction.Disable();
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!_isPlayerInRange) return;

        _store.SetActive(true);
        _menuManager.CloseMainMenu();
        _menuManager.CloseSettings();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
