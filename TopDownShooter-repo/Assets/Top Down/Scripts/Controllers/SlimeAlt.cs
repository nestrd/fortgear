using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { Idle, Wandering, Chasing, Attacking, Fleeing, Dead }

public class SlimeAlt : AIControllerAlt
{
    protected States states;

    public float wonderingMaxDuration;
    private float wonderingCurrentDuration;

    public float idleMaxDuration;
    private float idleCurrentDuration;

    public float attackRange = 1f;
    public float visionRange = 8f;

    protected override void Awake()
    {
        base.Awake();
        states = States.Idle;
    }

    protected override void processAI()
    {
        switch (states)
        {
            case States.Idle:
                Idle();
                break;

            case States.Wandering:
                Wandering();
                break;

            case States.Chasing:
                Chasing();
                break;

            case States.Attacking:
                Attacking();
                break;

            case States.Fleeing:
                Fleeing();
                break;

            case States.Dead:
                Dead();
                break;
        }
    }
    void Update()
    {
        //Animations
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Horizontal", GetFacingDirection().x);
        animator.SetFloat("Vertical", GetFacingDirection().y);

        Debug.DrawLine(transform.position, transform.position + new Vector3(GetFacingDirection().x, GetFacingDirection().y, 0) * 2, Color.red);
    }

    public override void Death(ControllerAlt killer = null)
    {
        base.Death(killer);
        killer.gameManager.progressionManager.RewardKillXP();
        Destroy(gameObject);
    }

    protected void Idle()
    {
        //print("Idle");

        movementState = MovementState.Idle;

        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) < visionRange)
            {
                states = States.Chasing;
            }
        }

        idleCurrentDuration += aITickRate;

        //randomly chnage direction every N Seconds
        if (idleCurrentDuration >= idleMaxDuration)
        {
            idleCurrentDuration = 0;
            states = States.Wandering;
        }
    }

    protected void Wandering()
    {
        //print("Wandering");

        movementState = MovementState.Walking;
        wonderingCurrentDuration += aITickRate;

        SmoothRotate(Vector2FromAngle(Random.Range(0, 360)), turnRate * aITickRate);

        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) < visionRange)
            {
                states = States.Chasing;
            }
        }

        //randomly chnage direction every N Seconds
        if (wonderingCurrentDuration >= wonderingMaxDuration)
        {
            wonderingCurrentDuration = 0;

            if (states != States.Chasing)
            {
                states = States.Idle;
            }
        }
    }

    protected void Chasing()
    {
        //print("Chasing");

        if (target != null)
        {
            Vector3 tar = (target.transform.position - transform.position).normalized;

            Debug.DrawLine(transform.position, transform.position + (tar * 2), Color.green);

            SmoothRotate(tar, turnRate * aITickRate);

            movementState = MovementState.Walking;

            if (Vector2.Distance(target.position, transform.position) < attackRange) //attack range
            {
                states = States.Attacking;
            }
            else if (Vector2.Distance(target.position, transform.position) > visionRange)
            {
                states = States.Idle;
                //print(Vector2.Distance(target.position, transform.position));
            }
        }
        else
        {
            states = States.Wandering;
        }
    }

    protected void Attacking()
    {
        //print("Attacking");

        movementState = MovementState.Idle;

        if (target != null)
        {
            Vector3 tar = (target.transform.position - transform.position).normalized;

            Debug.DrawLine(transform.position, transform.position + (tar * 2), Color.green);

            SmoothRotate(tar, turnRate * aITickRate);

            FireWeapon(true, 0);

            if (Vector2.Distance(target.position, transform.position) > attackRange)
            {
                states = States.Chasing;
                FireWeapon(false, 0);
            }
            else
            {
                states = States.Attacking;
            }
        }
    }

    protected void Fleeing()
    {
        //print("Fleeing");
        // potentially try to move in the opposite direction to the player's position
    }

    protected void Dead()
    {
        //print("Dead");
    }
}
