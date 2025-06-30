using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensibilidad del mouse
    public float moveSpeed = 10f;         // Velocidad de movimiento
    public float maxVerticalAngle = 85f; // �ngulo m�ximo hacia arriba/abajo

    private float pitch = 0f;            // Rotaci�n vertical
    private float yaw = 0f;              // Rotaci�n horizontal

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla y ocultarlo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotaci�n de la c�mara con el mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        // Restringir la rotaci�n vertical para evitar voltear la c�mara
        pitch = Mathf.Clamp(pitch, -maxVerticalAngle, maxVerticalAngle);

        // Aplicar rotaci�n
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Movimiento con teclas WASD o flechas
        Vector3 moveDirection = new Vector3(
            Input.GetAxis("Horizontal"), // Movimiento horizontal (A/D o flechas izquierda/derecha)
            0,                           // Movimiento vertical desactivado para mantener en el plano
            Input.GetAxis("Vertical")    // Movimiento hacia adelante/atr�s (W/S o flechas arriba/abajo)
        );

        // Permitir movimiento en cualquier direcci�n
        if (Input.GetKey(KeyCode.Space)) moveDirection.y += 1;   // Ascender (barra espaciadora)
        if (Input.GetKey(KeyCode.LeftShift)) moveDirection.y -= 1; // Descender (Shift)

        // Normalizar direcci�n y aplicar velocidad
        moveDirection = transform.TransformDirection(moveDirection.normalized) * moveSpeed * Time.deltaTime;

        // Aplicar movimiento
        transform.position += moveDirection;

        // Salir del modo espectador
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}