using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private float lifetime;

    private Collider2D collider;
    private Animator anim;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit)
        {
            return;
        }
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        collider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)  //h�m n�y �? h�?ng c?a fireball
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        collider.enabled = true;

        float localSacaleX = transform.localScale.x;
        if (Mathf.Sign(localSacaleX) != _direction) localSacaleX = -localSacaleX;
        transform.localScale = new Vector3(localSacaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}