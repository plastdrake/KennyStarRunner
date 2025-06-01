using UnityEngine;

public class TileEndTrigger : MonoBehaviour
{
    public GameObject parentTile;
    public FloorSpawner spawner;

    private void Start()
    {
        if (parentTile == null)
            parentTile = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the spawner to spawn more tiles
            if (spawner != null)
                spawner.OnTileEndReached(this);

            StartCoroutine(DelayedDestroy(2.0f));
        }
    }

    private System.Collections.IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (parentTile != null)
            Destroy(parentTile);
    }
}
