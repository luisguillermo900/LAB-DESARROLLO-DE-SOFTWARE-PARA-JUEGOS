using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    // 'States' that the NPC could be in.
    public enum STATE
    {
        IDLE,
        PATROL,
        PURSUE,
        ATTACK,
        SLEEP,
        RUNAWAY
    }

    // 'Events' - where we are in the running of a STATE.
    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    }

    public STATE name;              // To store the name of the STATE.
    protected EVENT stage;          // To store the stage the EVENT is in.
    protected GameObject npc;       // To store the NPC game object.
    protected Animator anim;        // To store the Animator component.
    protected Transform player;     // To store the transform of the player.
    protected State nextState;      // This is NOT the enum above, it's the next State object.

    public State(GameObject _npc, Animator _anim, Transform _player)
    {
        npc = _npc;
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER;
    }

    // The Process method runs the logic for entering, updating, and exiting a state.
    public virtual State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { }
    public virtual void Exit() { stage = EVENT.EXIT; }
}
