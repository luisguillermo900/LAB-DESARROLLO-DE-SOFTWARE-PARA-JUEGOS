using UnityEngine;

public class CupSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("�La bola entr� en la canasta!");
            GameManager.Instance.AddPoint();
        }
    }
}
