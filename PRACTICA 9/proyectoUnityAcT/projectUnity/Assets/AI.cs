using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform player; // Arrástralo desde el inspector (FPSController)

    private Animator anim;
    private NavMeshAgent agent;
    private State currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Iniciar con el estado de patrullaje
        currentState = new PatrolState(gameObject, anim, player);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
