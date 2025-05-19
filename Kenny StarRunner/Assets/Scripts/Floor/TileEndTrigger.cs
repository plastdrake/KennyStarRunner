using UnityEngine;

public class TileEndTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject parentTile;
    private void Start()
    {
        if (parentTile == null)
            parentTile = transform.root.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DelayedDestroy(2.0f));
    }
    private System.Collections.IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (parentTile != null)
            Destroy(parentTile);
    }
}
