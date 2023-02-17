using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


#region State Classes
public class CogManager
{ 

    private Type _currentState = typeof(CogStates);
    public CogStates currentCogState;
    private PlayerGear gearRef;

    public CogManager(PlayerGear _gearRef) 
    {
        gearRef = _gearRef;
        SwitchState(typeof (HeldState));
    }
    

    public void SwitchState(Type cogStates)
    {

        _currentState = cogStates;
        currentCogState = (CogStates) Activator.CreateInstance(_currentState);
        currentCogState.OnProcessing(gearRef);

        //Debug.Log("SwitchState");

    }

}

public class CogStates {
    public virtual void OnProcessing(PlayerGear gearRef)
    {
        throw new System.NotImplementedException();
    }
}

public class HeldState : CogStates
{
    public override void OnProcessing(PlayerGear gearRef)
    {
        gearRef.OnHeld();
        return;
    }
}

public class ThrownState : CogStates
{
    public override void OnProcessing(PlayerGear gearRef)
    {
        gearRef.OnThrown();
        return;
    }
}

public class ActivatingState : CogStates
{
    public override void OnProcessing(PlayerGear gearRef)
    {
        gearRef.OnActivating();
        return;
    }

}
public class DeactivatingState : CogStates
{
    public override void OnProcessing(PlayerGear gearRef)
    {
        gearRef.OnDeactivating();
        return;
    }

}
#endregion

public abstract class PlayerGear : MonoBehaviour
{

    protected SpriteRenderer cogSprite;
    protected Rigidbody2D cogRb;
    protected Rigidbody2D playerRb;
    public float cogSpeed = 100.0F;
    public float travelDistance = 2.0F;
    public float returnTime = 2.0F;
    [Range(0, 2)] public float cogWeight = 2.0F;
    public float cogSize = 1.0F;
    public CogManager _cogManager;
    protected Animator cogAnim;
    public GameObject cogUI;
    protected Animator uiAnim;
    public GameObject cogOrigin;
    protected AudioSource cogAudio;
    public AudioClip thrownClip;
    private Vector3 localPos;
    public bool isActivating;
    public bool isDeactivating;

    private float gearRotator;

    protected float time;


    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        cogSprite = GetComponentInChildren<SpriteRenderer>();
        cogRb = GetComponent<Rigidbody2D>();
        playerRb = GetComponentInParent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().radius = cogSize;
        cogAnim = GetComponentInChildren<Animator>();
        uiAnim = cogUI.GetComponent<Animator>();
        cogAudio = GetComponent<AudioSource>();

        //Modifier for player movement speed
        GetComponentInParent<PlayerController>().maxMovementSpeed = GetComponentInParent<PlayerController>().maxMovementSpeed * cogWeight;

        _cogManager = new CogManager(this);

    }

    private void FixedUpdate()
    {
        if(_cogManager.currentCogState is HeldState)
        {
            //localPos = transform.TransformDirection(this.transform.up) + cogOrigin.transform.position;
            //cogRb.MovePosition(localPos);
            cogRb.transform.localPosition = new Vector2(0.0f, 0.44f);
            cogRb.transform.rotation = Quaternion.Euler(Vector3.zero);

        }
        if (_cogManager.currentCogState is ActivatingState || _cogManager.currentCogState is DeactivatingState)
        {
            cogRb.transform.localPosition = new Vector2(0.0f, 0.44f);
        }
    }


    #region State Behaviors
    public void OnHeld()
    {
        //cogRb.simulated = false;
    }

    public virtual void OnThrown()
    {

    }

    public virtual void OnActivating()
    {
        StartCoroutine("CogActivating");
    }

    public virtual void OnDeactivating()
    {
        StartCoroutine("CogDeactivating");
    }

    private IEnumerator CogActivating()
    {
        isActivating = true;
        cogAnim.SetBool("IsActivating", true);
        yield return new WaitForSeconds(0.5f);
        isActivating = false;
        cogAnim.SetBool("IsActivating", false);
        _cogManager.SwitchState(typeof(HeldState));
        yield return null;
    }

    private IEnumerator CogDeactivating()
    {
        isDeactivating = true;
        cogAnim.SetBool("IsDeactivating", true);
        yield return new WaitForSeconds(0.5f);
        isDeactivating = false;
        cogAnim.SetBool("IsDeactivating", false);
        _cogManager.SwitchState(typeof(HeldState));
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D hitObject)
    {
        if (hitObject.gameObject.CompareTag("Enemy") && _cogManager.currentCogState is ThrownState){
            //hitObject.gameObject.GetComponent<Enemy>().TakeDamage(5);
            Destroy(hitObject.gameObject);
        }
    }

    #endregion

}
