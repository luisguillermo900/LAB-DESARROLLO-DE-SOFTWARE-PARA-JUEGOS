using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public int lap = 0;// N�mero de vueltas completadas
    public int checkpoint = -1; // N�mero de checkpoints alcanzados
    int checkpointCount ; // N�mero total de checkpoints
    int nextCheckpoint; // �ndice del siguiente checkpoint a alcanzar
    public GameObject lastCP; // Referencia al �ltimo checkpoint alcanzado

    // Start is called before the first frame update
    void Start()
    {
        checkpointCount= GameObject.FindGameObjectsWithTag("checkpoint").Length; // Contar el n�mero de checkpoints en la escena
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag== "checkpoint")
        {
             int thisCPNumber = int.Parse(col.gameObject.name); // Extraer el n�mero del checkpoint del nombre del objeto
            if (thisCPNumber == nextCheckpoint)
            {
                lastCP = col.gameObject; // Actualizar la referencia al �ltimo checkpoint alcanzado
                checkpoint = thisCPNumber;
                if(checkpoint==0) lap++; // Si se alcanza el checkpoint 0, se incrementa el n�mero de vueltas
                
                nextCheckpoint++; // Incrementar el �ndice del siguiente checkpoint
                if (nextCheckpoint >= checkpointCount) // Si se ha alcanzado el �ltimo checkpoint, reiniciar el �ndice
                {
                    nextCheckpoint = 0;
                }
                Debug.Log("Checkpoint reached: " + checkpoint + ", Lap: " + lap);
            }   
        }
     
    }
}
