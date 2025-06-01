using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    private CinemachineVirtualCameraBase mainCamera; 
    private CinemachineVirtualCameraBase highScoreCamera;
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
        if(mainCamera is null || highScoreCamera is null)
            InitializeCamera();
        
        _isHighScoreMenuOpen = true;
        mainCamera.Priority = 0;
        highScoreCamera.Priority = 10;
        _menuManager.CloseMainMenu();
    }

    private void MoveToMainCamera(bool isMoving)
    {
        if (!isMoving) 
            return;
        if(mainCamera is null || highScoreCamera is null)
            InitializeCamera();
        mainCamera.Priority = 10;
        highScoreCamera.Priority = 0;
    }

    void InitializeCamera()
    {
        mainCamera = GameObject.FindWithTag("MainCam").GetComponent<CinemachineVirtualCameraBase>();
        highScoreCamera = GameObject.FindWithTag("HighScoreCam").GetComponent<CinemachineVirtualCameraBase>();
        if(mainCamera is null || highScoreCamera is null) Debug.Log("A Camera is null");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _menuManager = GetComponent<MenuManager>();
        InitializeCamera();
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
