using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventActivator : EventManager
{

    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGear") && FindObjectOfType<PlayerGear>().isActivating == true)
        {
            anim.SetBool("IsActivating", true);
            anim.SetBool("IsDeactivating", false);
            activateAll = true;
        }
        if (collision.gameObject.CompareTag("PlayerGear") && FindObjectOfType<PlayerGear>().isDeactivating == true)
        {
            anim.SetBool("IsActivating", false);
            anim.SetBool("IsDeactivating", true);
            activateAll = false;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGear"))
        {
            activateAll = false;
        }
    }

    public void ActivatorOn()
    {

    }
}
