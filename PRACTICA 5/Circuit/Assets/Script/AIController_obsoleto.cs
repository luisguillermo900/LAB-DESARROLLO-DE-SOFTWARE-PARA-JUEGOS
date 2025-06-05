using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_obsoleto : MonoBehaviour
{
    //referencia al circuto que el carro debe seguir
    public Circuit circuit;
    //Sensibilidad del frenado
    public float brakingSensitivy = 1.1f;
    Drive ds;
    //Sensibilidad de la dirección
    public float steeringSensitivy = 0.01f;
    //Sensibilidad de la aceleración
    public float accelSensitivy = 0.03f;
    //Posición del WP actual
    Vector3 target;
    //Posición del WP siguiente
    Vector3 NextTarget;

    //Indice del WP actual en el circuto
    int currentWP = 0;
    //Distancial total hasta el WP actual
    float totalDistanceTotarget;

    // Start is called before the first frame update
    void Start()
    {
        //Obtiene el componente de conducción
        ds= this.GetComponent<Drive>();
        //Obtiene el 1er WP
        target= circuit.waypoints[currentWP].transform.position;
        //Obtiene el siguiente WP
        NextTarget = circuit.waypoints[currentWP+1].transform.position;
        //Calcular la distancia total
        totalDistanceTotarget= Vector3.Distance(target,ds.gameObject.transform.position);
     
         
    }

    // Update is called once per frame
    //Se ejecuta en cada fotograma
    void Update()
    {

        //Paso 1 Obtener posiciones relativas del WP

        Vector3 localTarget=ds.rb.gameObject.transform.InverseTransformPoint(target);
        Vector3 NextlocalTarget=ds.rb.gameObject.transform.InverseTransformPoint(NextTarget);

        float distanceToTarget = Vector3.Distance(target,ds.rb.gameObject.transform.position);


        //Paso 2 Calcular ángulo de dirección

        float targetAngle = Mathf.Atan2(localTarget.x,localTarget.z)* Mathf.Rad2Deg;

        float NexttargetAngle = Mathf.Atan2(NextlocalTarget.x, NextlocalTarget.z)* Mathf.Rad2Deg;

        //Ajustar la dirección del auto
        float steer = Mathf.Clamp(targetAngle * steeringSensitivy,-1,1) * Mathf.Sign(ds.currentSpeed);

        //Paso 3 Ajustar aceleración y frenado
        //Indicar cuanto falta para llegar al WP
        float distanceFactor = distanceToTarget / totalDistanceTotarget;

        //Indica que tan rapido va el auto en relación con su vel. max.
        float speedFactor = ds.currentSpeed / ds.maxSpeed;

        //Aumenta la aceleración a medida que el auto se acerca al WP
        float accel = Mathf.Lerp(accelSensitivy, 1, distanceFactor); //0.3f;// 1.0f;

        //Usa el ángulo del sgte. WP  para anticipar curvas cerradas..
        float brake = Mathf.Lerp((-1-Mathf.Abs(NexttargetAngle))*brakingSensitivy,1+speedFactor,1-distanceFactor);


        //Ajustes Adicionales
        //Si la curva es muy cerrada ,se a a reforzar el freno y reducir la aceleración.
        if (Mathf.Abs(NexttargetAngle) > 20)
        {
            brake += 0.8f;
            accel -= 0.8f;
        }
        /*
        if (distanceToTarget <5)
        {
            brake = 0.5f;
            accel = 0.1f;
        }
        */
        //Para mover el auto con los valores calculados
        ds.Go(accel,steer,brake);

        if (distanceToTarget < 4) { 

            currentWP++;

            if (currentWP >= circuit.waypoints.Length) { 
            
                currentWP = 0;
            }
            target = circuit.waypoints[currentWP].transform.position;

           // NextTarget = circuit.waypoints[currentWP+1].transform.position;

            if (currentWP == circuit.waypoints.Length - 1) {

                NextTarget = circuit.waypoints[0].transform.position;
            }
            else
            {
                NextTarget = circuit.waypoints[currentWP + 1].transform.position;

            }

            totalDistanceTotarget=Vector3.Distance(target,ds.rb.gameObject.transform.position);

            //Verificar si las llantas derrapan
            ds.CheckForSkid();
            //Ajusta el sonido del motor según la velocidad
            ds.CalculateEngineSound();
        }
    }
}
