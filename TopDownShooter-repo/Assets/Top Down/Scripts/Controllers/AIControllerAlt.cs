using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIControllerAlt : ControllerAlt
{
    public float aITickRate = 0.25f;
    protected float startTime;
    protected Transform target;
    public Transform Target { get { return target; } }

    public virtual void Start()
    {
        InvokeRepeating("processAI", 0, aITickRate);
    }

    protected abstract void processAI();

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }
    }
}
