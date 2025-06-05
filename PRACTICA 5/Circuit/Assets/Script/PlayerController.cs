using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Referencia al componente Drive del veh�culo
    Drive ds;

    // Variable para registrar el �ltimo momento en que el auto se movi�
    float lastTimeMoving = 0;

    // Variables para guardar la �ltima posici�n y rotaci�n v�lida del auto
    Vector3 lastPosition;
    Quaternion lastRotation;

    CheckpointManager cpm; // Referencia al gestor de checkpoints

    // M�todo que restablece la capa y apaga el efecto de transparencia (Ghost)
    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0; // Regresa la capa del auto a Default (0)
        this.GetComponent<Ghost>().enabled = false; // Desactiva el efecto Ghost
    }

    // Start se ejecuta al inicio del juego
    void Start()
    {
        ds = this.GetComponent<Drive>(); // Se obtiene el componente Drive
        this.GetComponent<Ghost>().enabled = false; // Se asegura de que el efecto Ghost est� desactivado al comenzar
       
        lastRotation= ds.rb.gameObject.transform.rotation; // Guarda la rotaci�n inicial del auto
        lastPosition = ds.rb.gameObject.transform.position; // Guarda la posici�n inicial del auto
    }

    // Update se ejecuta una vez por frame
    void Update()
    {
        // Lectura de entrada del jugador
        float a = Input.GetAxis("Vertical");   // Aceleraci�n / Frenado
        float s = Input.GetAxis("Horizontal"); // Direcci�n
        float b = Input.GetAxis("Jump");       // Freno de mano u otra funci�n (asignable)

        // Si el auto se est� moviendo o la carrera a�n no empieza, actualiza el tiempo del �ltimo movimiento
        if (ds.rb.velocity.magnitude > 1 || !RaceMonitor.racing)
            lastTimeMoving = Time.time;

        // Detecta si el auto est� sobre una superficie v�lida (con tag "road")
        RaycastHit hit;
        if (Physics.Raycast(ds.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "road")
            {
                //Debug.Log("off road"); // Mensaje de depuraci�n
                lastPosition = ds.rb.gameObject.transform.position;  // Guarda la �ltima posici�n v�lida
                lastRotation = ds.rb.gameObject.transform.rotation;  // Guarda la �ltima rotaci�n v�lida
            }
        }

        // Si el auto ha estado inm�vil por m�s de 4 segundos, lo reposiciona
        if (Time.time > lastTimeMoving + 4)
        {
            if (cpm == null)
            {
                cpm = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
                Debug.Log("CheckpointManager not found, trying to find it again.");
            }
            Debug.Log(cpm);

            ds.rb.gameObject.transform.position =cpm.lastCP.transform.position + Vector3.up * 2; // Reposiciona el auto sobre el �ltimo checkpoint alcanzado
              // lastPosition;     // Lo mueve a la �ltima posici�n segura
            ds.rb.gameObject.transform.rotation =cpm.lastCP.transform.rotation; // Reposiciona el auto con la �ltima rotaci�n segura
             // lastRotation;     // Le asigna la �ltima rotaci�n segura
            ds.rb.gameObject.layer = 8;                             // Cambia la capa (posiblemente para evitar colisiones moment�neamente)
            this.GetComponent<Ghost>().enabled = true;              // Activa el efecto visual de "desaparecer"
            Invoke("ResetLayer", 3);                                // Despu�s de 3 segundos, vuelve a la normalidad
        }

        // Si la carrera no ha comenzado, se desactiva la aceleraci�n del jugador
        if (!RaceMonitor.racing) a = 0;

        // Se aplican las entradas al veh�culo
        ds.Go(a, s, b);                 // Control del auto
        ds.CheckForSkid();             // Verifica si hay derrape
        ds.CalculateEngineSound();     // Calcula el sonido del motor seg�n velocidad y RPM
    }
}
