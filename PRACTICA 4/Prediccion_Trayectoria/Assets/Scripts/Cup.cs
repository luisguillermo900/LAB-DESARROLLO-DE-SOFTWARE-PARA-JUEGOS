﻿using UnityEngine;

public class Cup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("¡La bola entró en la canasta!");
            GameManager.Instance.AddPoint();
        }
    }
}
