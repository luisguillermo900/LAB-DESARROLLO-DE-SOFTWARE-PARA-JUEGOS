using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("¡Has acertado al objetivo!");
            Destroy(gameObject); 
        }
    }
}
