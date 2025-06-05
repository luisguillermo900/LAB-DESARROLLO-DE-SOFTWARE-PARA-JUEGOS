using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Referencia al componente Drive del vehículo
    Drive ds;

    // Variable para registrar el último momento en que el auto se movió
    float lastTimeMoving = 0;

    // Variables para guardar la última posición y rotación válida del auto
    Vector3 lastPosition;
    Quaternion lastRotation;

    CheckpointManager cpm; // Referencia al gestor de checkpoints

    // Método que restablece la capa y apaga el efecto de transparencia (Ghost)
    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0; // Regresa la capa del auto a Default (0)
        this.GetComponent<Ghost>().enabled = false; // Desactiva el efecto Ghost
    }

    // Start se ejecuta al inicio del juego
    void Start()
    {
        ds = this.GetComponent<Drive>(); // Se obtiene el componente Drive
        this.GetComponent<Ghost>().enabled = false; // Se asegura de que el efecto Ghost esté desactivado al comenzar
       
        lastRotation= ds.rb.gameObject.transform.rotation; // Guarda la rotación inicial del auto
        lastPosition = ds.rb.gameObject.transform.position; // Guarda la posición inicial del auto
    }

    // Update se ejecuta una vez por frame
    void Update()
    {
        // Lectura de entrada del jugador
        float a = Input.GetAxis("Vertical");   // Aceleración / Frenado
        float s = Input.GetAxis("Horizontal"); // Dirección
        float b = Input.GetAxis("Jump");       // Freno de mano u otra función (asignable)

        // Si el auto se está moviendo o la carrera aún no empieza, actualiza el tiempo del último movimiento
        if (ds.rb.velocity.magnitude > 1 || !RaceMonitor.racing)
            lastTimeMoving = Time.time;

        // Detecta si el auto está sobre una superficie válida (con tag "road")
        RaycastHit hit;
        if (Physics.Raycast(ds.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "road")
            {
                //Debug.Log("off road"); // Mensaje de depuración
                lastPosition = ds.rb.gameObject.transform.position;  // Guarda la última posición válida
                lastRotation = ds.rb.gameObject.transform.rotation;  // Guarda la última rotación válida
            }
        }

        // Si el auto ha estado inmóvil por más de 4 segundos, lo reposiciona
        if (Time.time > lastTimeMoving + 4)
        {
            if (cpm == null)
            {
                cpm = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
                Debug.Log("CheckpointManager not found, trying to find it again.");
            }
            Debug.Log(cpm);

            ds.rb.gameObject.transform.position =cpm.lastCP.transform.position + Vector3.up * 2; // Reposiciona el auto sobre el último checkpoint alcanzado
              // lastPosition;     // Lo mueve a la última posición segura
            ds.rb.gameObject.transform.rotation =cpm.lastCP.transform.rotation; // Reposiciona el auto con la última rotación segura
             // lastRotation;     // Le asigna la última rotación segura
            ds.rb.gameObject.layer = 8;                             // Cambia la capa (posiblemente para evitar colisiones momentáneamente)
            this.GetComponent<Ghost>().enabled = true;              // Activa el efecto visual de "desaparecer"
            Invoke("ResetLayer", 3);                                // Después de 3 segundos, vuelve a la normalidad
        }

        // Si la carrera no ha comenzado, se desactiva la aceleración del jugador
        if (!RaceMonitor.racing) a = 0;

        // Se aplican las entradas al vehículo
        ds.Go(a, s, b);                 // Control del auto
        ds.CheckForSkid();             // Verifica si hay derrape
        ds.CalculateEngineSound();     // Calcula el sonido del motor según velocidad y RPM
    }
}
