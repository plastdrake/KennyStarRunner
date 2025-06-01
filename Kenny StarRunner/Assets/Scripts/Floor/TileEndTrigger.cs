using UnityEngine;

public class TileEndTrigger : MonoBehaviour
{
    public GameObject parentTile;
    public FloorSpawner spawner;
    private bool destructionStarted = false;

    private void Start()
    {
        if (parentTile == null)
            parentTile = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !destructionStarted)
        {
            destructionStarted = true;
            if (spawner != null)
                spawner.OnTileEndReached(this);

            StartCoroutine(DelayedDestroy(2.0f));
        }
    }

    private System.Collections.IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spawner != null)
            spawner.OnTileDestroyed(parentTile);
        if (parentTile != null)
            Destroy(parentTile);
    }
}