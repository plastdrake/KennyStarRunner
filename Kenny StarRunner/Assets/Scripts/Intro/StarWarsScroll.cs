using TMPro;
using UnityEngine;

public class StarWarsScroll : MonoBehaviour
{
    public float scrollSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        GetComponent<TextMeshPro>().ForceMeshUpdate();
    }
}
