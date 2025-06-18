using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    public float swingAngle = 60f; // Góc lệch tối đa (độ)
    public float swingSpeed = 2f;  // Tốc độ lắc
    public Transform pivot;
    public bool rotateContinuously = false; // Tùy chọn xoay liên tục
    public float continuousRotateSpeed = 90f; // Tốc độ xoay liên tục (độ/giây)
    private float startAngle;
    private float time;

    void Start()
    {
        startAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        if (rotateContinuously)
        {
            // Xoay liên tục quanh trục Z
            transform.RotateAround(pivot.position, Vector3.forward, continuousRotateSpeed * Time.deltaTime);
        }
        else
        {
            time += Time.deltaTime * swingSpeed;
            float angle = Mathf.Sin(time) * swingAngle;
            transform.RotateAround(pivot.position, Vector3.forward, startAngle + angle - transform.eulerAngles.z);
        }
    }
}