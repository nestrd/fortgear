using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechGear : MonoBehaviour
{
    private PlayerGear gearRef;
    public bool activatingGear;
    private int deactivatingCounter;
    public TechActivator blockActivator;


    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(9, 12);
        gearRef = FindObjectOfType<PlayerGear>();
        deactivatingCounter = 0;
    }

    private void Update()
    {
        if (activatingGear)
        {
            ++deactivatingCounter;
            blockActivator.activatorOn = true;
        }
        if (activatingGear == false)
        {
            --deactivatingCounter;
            blockActivator.activatorOn = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGear"))
        {
            if (gearRef._cogManager.currentCogState is ActivatingState)
            {
                Debug.Log("Cog engage");
                activatingGear = true;
            }
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGear"))
        {
            Debug.Log("Cog release");
            activatingGear = false;
        }
    }
}
