using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _store;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _audioManager;
    private ShopSound _sounds;
    public void CloseStore()
    {
        _store.SetActive(false);
    }

    public void OpenStore()
    {
        _store.SetActive(true);
        _sounds.OpenMenuSound();
    }

    public void CloseMainMenu()
    {
        _mainMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        _settings.SetActive(true);
    }

    public void CloseSettings()
    {
        _settings.SetActive(false);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseCursor()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_store.activeSelf)
                _store.SetActive(false);
            else
                _mainMenu.SetActive(!_mainMenu.activeSelf);
        }
    }

    void Start()
    {
        _sounds = _audioManager.GetComponent<ShopSound>();
    }
}
