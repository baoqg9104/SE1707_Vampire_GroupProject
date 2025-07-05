using TMPro;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;


    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = pointB.position;

    }

    // Update is called once per frame
    void Update()
    {
        MoveSaw();
    }

    private void MoveSaw()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }
}


