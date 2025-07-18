using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCar : MonoBehaviour
{
    Rigidbody rb;
    float lastTimeChecked;
    // Start is called before the first frame update
    void Start()
    {
        rb= this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.up.y>0.5f || rb.velocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;    

        }

        if(Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }

    private void RightCar()
    {
        this.transform.position += Vector3.up;

        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }
}
