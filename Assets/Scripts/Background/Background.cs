using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform followCameraTransform;
    private Vector3 offset;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        if (followCameraTransform != null)
        {
            offset = transform.position - followCameraTransform.position;
            lastCameraPosition = followCameraTransform.position;
        }
    }

    private void LateUpdate()
    {
        if (followCameraTransform != null)
        {
            Vector3 currentCameraPosition = followCameraTransform.position;
            if (currentCameraPosition != lastCameraPosition)
            {
                Vector3 newPos = currentCameraPosition + offset;
                transform.position = newPos;
                lastCameraPosition = currentCameraPosition;
            }
        }
    }
}

