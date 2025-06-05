using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respaldo : MonoBehaviour
{
    //Referencia al circuito que el auto debe seguir.
    public Circuit circuit;

    public float brakingSensitivity = 1.1f; //en el inspector poner 1 ó 3
    Drive ds;
    public float steeringSensitivity = 0.01f;
    public float accelSensitivity = 0.03f;
    Vector3 target;
    Vector3 nextTarget; //next waypoint
    int currentWP = 0;
    float totalDistanceToTarget;


    GameObject tracker;
    int currentTrackerWP = 0;
    float lookAhead = 10;

    float lastTimeMoving = 0;

    bool isResetting = false; // Se activa cuando el auto es teletransportado

    // Start is called before the first frame update
    void Start()
    {
        ds = this.GetComponent<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
        nextTarget = circuit.waypoints[currentWP + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, ds.gameObject.transform.position);


        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = ds.gameObject.transform.position;
        tracker.transform.rotation = ds.gameObject.transform.rotation;
        //new
        // this.GetComponent<Ghost>().enabled = false;

        // Time.timeScale = 5;
    }

    void ProgressTracker()
    {
        if (isResetting) return; // Si estamos en teletransporte, no aumentar waypoints

        Debug.DrawLine(ds.rb.gameObject.transform.position, tracker.transform.position);
        if (Vector3.Distance(ds.rb.gameObject.transform.position, tracker.transform.position) > lookAhead) return;

        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f); //velocidad de tracker
        if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.waypoints.Length)
                currentTrackerWP = 0;
        }
    }
    //new
    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0;
        // this.GetComponent<Ghost>().enabled = false;
    }

    void ResumeWaypoints()
    {
        isResetting = false; // Volver a permitir el aumento de waypoints
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();
        // tracker.transform.position
        Vector3 localTarget;
        float targetAngle;

        //new
        if (ds.rb.velocity.magnitude > 1)
            lastTimeMoving = Time.time;
        //new
        if (Time.time > lastTimeMoving + 4)
        {
            isResetting = true; // Bloquear incremento de waypoints temporalmente

            Debug.Log("Antes de resetear: " + currentTrackerWP);

            //currentTrackerWP = GetNextWaypoint();
            Debug.Log("FUNCION: " + currentTrackerWP);
            ds.rb.gameObject.transform.position =
                            circuit.waypoints[currentTrackerWP].transform.position + Vector3.up * 2 +
                            new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            tracker.transform.position = ds.rb.gameObject.transform.position;
            ds.rb.gameObject.layer = 8;
            //this.GetComponent<Ghost>().enabled = true;
            Debug.Log("Después de resetear: " + currentTrackerWP);
            Invoke(nameof(ResetLayer), 3);
            Invoke(nameof(ResumeWaypoints), 2); // Reactivar el waypoint después de 2 segundos

        }


        if (Time.time < ds.rb.GetComponent<AvoidDetector>().avoidTime)
        {
            localTarget = tracker.transform.right * ds.rb.GetComponent<AvoidDetector>().avoidPath;
        }
        else
        {
            localTarget = ds.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        }
        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;


        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(ds.currentSpeed);
        //  Debug.Log("Freno: " + brake + " Aceleracion " + accel+ " Velocidad "+ ds.rb.velocity.magnitude);

        //nuevo2
        float speedFactor = ds.currentSpeed / ds.maxSpeed;
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;
        //nuevo2
        float brake = 0;
        if (corner > 10 && speedFactor > 0.1f)
            brake = Mathf.Lerp(0, 1 + speedFactor * brakingSensitivity, cornerFactor);

        float accel = 1f;
        //nuevo2
        if (corner > 20 && speedFactor > 0.2f)
            accel = Mathf.Lerp(0, 1 * accelSensitivity, 1 - cornerFactor);

        ds.Go(accel, steer, brake);
        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }

}
