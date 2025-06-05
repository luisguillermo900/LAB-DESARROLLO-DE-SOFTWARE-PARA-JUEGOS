using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    public Light redLight; // Asigna en el Inspector la luz roja
    public Light blueLight; // Asigna en el Inspector la luz azul
    public float blinkSpeed = 0.5f; // Velocidad de intermitencia

    public bool isRedOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //apago las luces
        
        if (redLight) redLight.enabled = false;
        if (blueLight) blueLight.enabled = false;

        //inicia la intermitencia
        InvokeRepeating(nameof(ToggleLights), 0, blinkSpeed);
    }
    void ToggleLights()
    {
        isRedOn = !isRedOn;

        if(redLight) redLight.enabled = !isRedOn;
        if(blueLight) blueLight.enabled = isRedOn; //alterna rojo y azul

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
