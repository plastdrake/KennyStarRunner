using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject _store;

    private bool _isPlayerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        _isPlayerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        _isPlayerInRange = false;
        _store.SetActive(false);
    }

    public void CloseMenu()
    {
        _store.SetActive(false);
    }

    void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pressing F");
            _store.SetActive(true);
        }
    }
}
