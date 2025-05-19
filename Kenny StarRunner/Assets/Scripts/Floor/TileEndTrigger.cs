using UnityEngine;

public class TileEndTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject parentTile;

    private void Start()
    {
        if (parentTile == null)
        {
            parentTile = transform.root.gameObject;
            Debug.Log("Auto-assigned parent tile: " + parentTile.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger] {gameObject.name} hit by: {other.name}, parentTile = {parentTile?.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log($"[Destroy] Removing tile: {parentTile?.name}");
            Destroy(parentTile);
        }
    }
}
