using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Character : MonoBehaviour {

    public enum CharacterState
    {
        IDLE,
        MOVE,
        FOLLOW,
        BATTLE
    }

    protected double atkCooldown;
    public CharacterState State { get; set; }
    
    // Use this for initialization
    void Start()
    {
        atkCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void Hit(double dmg);
    public abstract void Die();

    public virtual void Move(Vector3 targetPos)
    {
        Vector3 deflection = targetPos - transform.position;
        if (deflection.magnitude < 0.1)
        {
            State = CharacterState.IDLE;
        }
        else
        {
            transform.Translate(deflection.normalized * 0.2f);
        }
    }
}

