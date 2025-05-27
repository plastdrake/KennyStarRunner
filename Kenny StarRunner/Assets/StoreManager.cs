using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject _store;
    [SerializeField] private TMP_Text _storeText;

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
        Debug.Log("Closing");
        _store.SetActive(false);
        _storeText.gameObject.SetActive(false);

        // Lock and hide the cursor when leaving the store
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pressing F");
            _store.SetActive(true);

            // Unlock and show the cursor when entering the store
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
