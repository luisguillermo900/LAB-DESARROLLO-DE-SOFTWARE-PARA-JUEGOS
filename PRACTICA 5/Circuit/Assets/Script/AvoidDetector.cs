using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0.0f; //Determina hacia que lado se desviara el auto.
    public float avoidTime = 0.0f; //Tiempo hasta que el auto evitará una colisión.
    public float wanderDistance = 6.0f; //Distancia de evasión.
    public float avoidLenght = 1.0f; //Duración del intento de evitar la colisión (1seg).


     void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag != "car")
        {

            avoidTime = 0.0f;
        }
    }

      void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag != "car") return;

        if (Time.time < avoidTime) return;

        Rigidbody otherCar = col.rigidbody;
        avoidTime = Time.time + avoidLenght;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z) * Mathf.Rad2Deg;
        avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle); //ajute el desvío

    }

}
