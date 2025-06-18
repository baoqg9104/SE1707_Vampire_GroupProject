using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform followCameraTransform;
    private Vector3 offset;

    private void Start()
    {
        if (followCameraTransform != null)
            offset = transform.position - followCameraTransform.position;
    }

    private void LateUpdate()
    {
        if (followCameraTransform != null)
        {
            Vector3 newPos = followCameraTransform.position + offset;
            transform.position = newPos;
        }
    }
}

