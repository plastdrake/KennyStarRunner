using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform _mainCamera;

    void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.forward);
    }
}
