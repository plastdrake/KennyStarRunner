using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class MenuManager : MonoBehaviour
{
 
    [SerializeField] private GameObject _store;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _audioManager;
    private CameraTransition _cameraTransition;
    private InputSystem_Actions menuAction;

    public GameObject _hatsMenu;
    public GameObject _titlesMenu;
    public GameObject _infoObject;
    
    private ShopSound _sounds;

    private void OnEnable()
    {
        menuAction.Enable();
    }

    private void OnDisable()
    {
        menuAction.Disable();
    }

    public void CloseStore()
    {
        _store.SetActive(false);
    }

    public void OpenStore()
    {
        CloseSettings();
        CloseMainMenu();
        _store.SetActive(true);
        _sounds.OpenMenuSound();
    }

    public void CloseMainMenu()
    {
        _mainMenu.SetActive(false);
        ShowCursor(false);
    }

    public void ToggleMainMenu()
    {
        _mainMenu.SetActive(!_mainMenu.activeSelf);
        ShowCursor(true);
    }

    public void OpenSettings()
    {
        _settings.SetActive(true);
    }

    public void CloseSettings()
    {
        _settings.SetActive(false);
    }

    public void OpenHatsMenu()
    {
        _titlesMenu.SetActive(false);
        _infoObject.SetActive(true);
        _hatsMenu.SetActive(true);
    }

    public void OpenTitlesMenu()
    {
        _hatsMenu.SetActive(false);
        _infoObject.SetActive(false);
        _titlesMenu.SetActive(true);
    }

    private void ShowCursor(bool showCursor)
    {
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void QuitGameMode()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Awake()
    {
        _cameraTransition = GetComponent<CameraTransition>();
        _sounds = _audioManager.GetComponent<ShopSound>();
        menuAction = new();
        menuAction.Player.Pause.performed += ctx =>
        {
            if (_cameraTransition.IsHighScoreCameraFocused())
            {
                _cameraTransition.MoveToMainCamera(true);
                return;
            }

            if (_store.activeSelf)
                _store.SetActive(false);
            else if (_settings.activeSelf)
                _settings.SetActive(false);
            else
                ToggleMainMenu();
                
        };
    }
}
