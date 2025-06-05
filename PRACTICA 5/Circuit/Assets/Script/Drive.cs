using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WC;
    public GameObject[] Wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 500;
    public AudioSource skidSound;
    public AudioSource highAccel;

    public ParticleSystem SmokePrefap;
    ParticleSystem[] skidSmoke = new ParticleSystem[4];
    public GameObject LBrakeLight;
    public GameObject RBrakeLight;

    public Rigidbody rb;
    public float gearlenght = 3;
    public float currentSpeed { get { return rb.velocity.magnitude * gearlenght; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 200;

    public GameObject playerNamePrefab;
    public Renderer cargameMesh;
    string[] aiNames = { "Pedro","Juan", "Marcos", "Mateo","Judas", "Jose","Alfredo"};

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            skidSmoke[i] = Instantiate(SmokePrefap);
            skidSmoke[i].Stop();
        }
        LBrakeLight.SetActive(false);
        RBrakeLight.SetActive(false);       

        GameObject playerName = Instantiate(playerNamePrefab);
        playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;
               
        if (this.GetComponent<AIController>().enabled)
        {      
            playerName.GetComponent<NameUIController>().playerName.text = aiNames[UnityEngine.Random.Range(0, aiNames.Length)];
        }
        else
        {       
            playerName.GetComponent<NameUIController>().playerName.text = "Jugador1";
        }
  
        playerName.GetComponent<NameUIController>().carRend = cargameMesh;

        // playerName.GetComponent<Text>().text = "PlayerName";
        AdjustWheelFriction(); // Esto hará que las ruedas tengan más agarre en las curvas.
    }




    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        //del curso
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        // chatgpt4
        // steer *= Mathf.Clamp(1 - (currentSpeed / maxSpeed), 0.3f, 1);
        //  brake = Mathf.Lerp(brake, maxBrakeTorque, Time.deltaTime * 5);
        float thrustTorque = accel * torque;

        //luces traseras de freno

        LBrakeLight.SetActive((brake != 0 ? true : false));
        RBrakeLight.SetActive((brake != 0 ? true : false));


        for (int i = 0; i < 4; i++)
        {

            WC[i].motorTorque = thrustTorque;
            if (i < 2)
            {
                WC[i].steerAngle = steer;
            }
            else
            {
                WC[i].brakeTorque = brake;
            }

            Quaternion quat;
            Vector3 position;
            WC[i].GetWorldPose(out position, out quat);
            Wheels[i].transform.position = position;
            Wheels[i].transform.rotation = quat;
        }

    }

    public void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float)numGears);
        //InverLerp
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear,
            gearPercentage * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));

        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);

        var gearNumFactor = currentGear / (float)numGears;

        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);
        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);

        float upperGearMax = (1 / (float)numGears * (currentGear + 1));
        float downGearMax = (1 / (float)numGears * (currentGear));

        if (currentGear > 0 && speedPercentage < downGearMax)
        {
            currentGear--;
        }
        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
        {
            currentGear++;
        }

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);


        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;

    }
    public void CheckForSkid()
    {
        int numSkidding = 0;

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            WC[i].GetGroundHit(out wheelHit);
            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                numSkidding++;
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();
                }
                //ubicaci n de Smoke Particle
                skidSmoke[i].transform.position = WC[i].transform.position - WC[i].transform.up * WC[i].radius;
                skidSmoke[i].Emit(1);
            }

        }
        if (numSkidding == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }

    }

    void AdjustWheelFriction()
    {
        foreach (WheelCollider wheel in WC)
        {
            WheelFrictionCurve friction = wheel.sidewaysFriction;
            friction.stiffness = 1.4f;  // Mayor agarre lateral
            wheel.sidewaysFriction = friction;
        }
    }


}
