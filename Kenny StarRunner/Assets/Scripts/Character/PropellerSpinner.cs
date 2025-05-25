using UnityEngine;

public class PropellerSpinner : MonoBehaviour
{
    [SerializeField] private Transform propeller; // Assign in Inspector
    [SerializeField] private float spinSpeed = 360f; // Degrees per second

    void Update()
    {
        if (propeller != null)
        {
            propeller.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
        }
    }
}