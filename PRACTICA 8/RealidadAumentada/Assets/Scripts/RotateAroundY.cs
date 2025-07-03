using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto alrededor del eje Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
