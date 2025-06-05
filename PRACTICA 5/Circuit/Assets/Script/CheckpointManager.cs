using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public int lap = 0;// Número de vueltas completadas
    public int checkpoint = -1; // Número de checkpoints alcanzados
    int checkpointCount ; // Número total de checkpoints
    int nextCheckpoint; // Índice del siguiente checkpoint a alcanzar
    public GameObject lastCP; // Referencia al último checkpoint alcanzado

    // Start is called before the first frame update
    void Start()
    {
        checkpointCount= GameObject.FindGameObjectsWithTag("checkpoint").Length; // Contar el número de checkpoints en la escena
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag== "checkpoint")
        {
             int thisCPNumber = int.Parse(col.gameObject.name); // Extraer el número del checkpoint del nombre del objeto
            if (thisCPNumber == nextCheckpoint)
            {
                lastCP = col.gameObject; // Actualizar la referencia al último checkpoint alcanzado
                checkpoint = thisCPNumber;
                if(checkpoint==0) lap++; // Si se alcanza el checkpoint 0, se incrementa el número de vueltas
                
                nextCheckpoint++; // Incrementar el índice del siguiente checkpoint
                if (nextCheckpoint >= checkpointCount) // Si se ha alcanzado el último checkpoint, reiniciar el índice
                {
                    nextCheckpoint = 0;
                }
                Debug.Log("Checkpoint reached: " + checkpoint + ", Lap: " + lap);
            }   
        }
     
    }
}
