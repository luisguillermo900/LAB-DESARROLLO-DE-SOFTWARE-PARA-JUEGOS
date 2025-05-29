using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }

    private Vector3 initialPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        initialPosition = transform.position;
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DesactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    public void ResetPosition()
    {
        DesactivateRb();
        transform.position = initialPosition;
    }
}
