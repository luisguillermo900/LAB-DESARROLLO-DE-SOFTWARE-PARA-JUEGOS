using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private int currentIndex = -1;
    private NavMeshAgent agent;
    private List<GameObject> waypoints;

    public PatrolState(GameObject _npc, Animator _anim, Transform _player)
        : base(_npc, _anim, _player)
    {
        name = STATE.PATROL;
        agent = npc.GetComponent<NavMeshAgent>();
        waypoints = GameEnvironment.Singleton.Checkpoints;
    }

    public override void Enter()
    {
        anim.SetTrigger("isWalking");

        if (waypoints.Count == 0) return;

        currentIndex = Random.Range(0, waypoints.Count);
        agent.SetDestination(waypoints[currentIndex].transform.position);
        stage = EVENT.UPDATE;
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentIndex].transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
