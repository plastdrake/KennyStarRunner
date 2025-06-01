using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineVirtualCameraBase mainCamera; 
    public CinemachineVirtualCameraBase highScoreCamera;
    private MenuManager _menuManager;
    private bool _isHighScoreMenuOpen;

    void OnEnable()
    {
        RunnerCharacterController.OnMovementChanged += MoveToMainCamera;
    }

    void OnDisable()
    {
        RunnerCharacterController.OnMovementChanged -= MoveToMainCamera;
    }

    public void MoveToHighScore()
    {
        _isHighScoreMenuOpen = true;
        mainCamera.Priority = 0;
        highScoreCamera.Priority = 10;
        _menuManager.CloseMainMenu();
    }

    private void MoveToMainCamera(bool isMoving)
    {
        if (!isMoving) 
            return;
        if (mainCamera != null && highScoreCamera != null)
        {
            mainCamera.Priority = 10;
            highScoreCamera.Priority = 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _menuManager = GetComponent<MenuManager>();
        MoveToMainCamera(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isHighScoreMenuOpen)
        {
            _menuManager.CloseMainMenu();
            MoveToMainCamera(true);
        }

    }
}
