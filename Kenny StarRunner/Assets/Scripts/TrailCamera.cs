using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 localOffset = new Vector3(0f, 5f, -10f);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        Quaternion targetRotation = target.rotation;

        Vector3 desiredPosition = target.position + targetRotation * localOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
