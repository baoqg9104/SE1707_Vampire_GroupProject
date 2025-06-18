using UnityEngine;
public class PlatformBrown : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = pointB.position;
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }
}