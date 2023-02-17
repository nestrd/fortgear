using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MovementState
{
    Idle,
    Walking
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ControllerAlt : MonoBehaviour
{
    //Public
    [Header("Character Stats")]
    public float maxHealth = 100f;

    [Header("Character Effects")]
    public ParticleSystem deathFX;
    public GameObject damageHoverText;

    [Header("Character Movement Settings")]
    public float maxMovementSpeed = 3f;
    public float timeZeroToMax = 0.1f;
    public float timeMaxToZero = 0.1f;
    public float turnRate = 0.1f;

    [Header("Character Settings")]
    public Transform aimer;

    public GameManagerAlt gameManager;

    //Protected
    protected Rigidbody2D rb;
    protected Animator animator;
    protected float health;

    protected List<WeaponSlotAlt> weaponSlots = new List<WeaponSlotAlt>();

    //protected bool accelerating;
    protected Vector2 currentVelocity;
    //protected float accelerationRate;
    //protected float decelerationRate;

    protected float smoothTurn;
    protected float smoothAX, smoothAY, smoothDX, smoothDY;
    protected MovementState movementState;
    protected float rotation;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        health = maxHealth;

        //uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        gameManager = FindObjectOfType<GameManagerAlt>();

        foreach (WeaponSlotAlt weaponSlot in GetComponentsInChildren<WeaponSlotAlt>())
        {
            weaponSlots.Add(weaponSlot);
        }
    }

    public virtual void Reset()
    {
        //Reset stats etc
        health = maxHealth;
        rb.velocity = Vector2.zero;
        rotation = 0;
    }

    public float GetHealth()
    {
        return health;
    }

    protected Vector2 GetFacingDirection()
    {
        return Vector2FromAngle(rotation);
    }

    protected Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    public void SetMovementState(MovementState state)
    {
        movementState = state;
    }

    protected virtual void SmoothRotate(Vector2 direction, float rotationRate)
    {
        float targetRotation = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + 90;
        rotation = Mathf.SmoothDampAngle(rotation, targetRotation, ref smoothTurn, rotationRate);
    }

    protected virtual void SmoothAccelerate(Vector2 direction, float maxSpeed, float rate)
    {
        Vector2 targetVelocity = Vector2.zero;
        targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, maxSpeed * direction.x, ref smoothAX, rate);
        targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, maxSpeed * direction.y, ref smoothAY, rate);
        rb.velocity = targetVelocity;
    }

    protected virtual void SmoothDecelerate(float minSpeed, float rate)
    {
        Vector2 targetVelocity = Vector2.zero;

        if (rb.velocity.magnitude <= 0.1f)
        {
            smoothAX = 0f;
            smoothAY = 0f;
            smoothDX = 0f;
            smoothDY = 0f;
        }
        else
        {
            targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, minSpeed, ref smoothDX, rate);
            targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, minSpeed, ref smoothDY, rate);
        }
        rb.velocity = targetVelocity;
    }

    public List<WeaponAlt> GetWeapons()
    {
        List<WeaponAlt> weapons = new List<WeaponAlt>();

        foreach (WeaponSlotAlt weaponSlot in weaponSlots)
        {
            if (weaponSlot.GetWeapon() != null)
            {
                //weapons.Add(weaponSlot.GetWeapon());
            }
        }

        return weapons;
    }

    public void EquipWeapon(WeaponAlt weapon)
    {
        List<WeaponSlotAlt> weaponSlotsInUse = new List<WeaponSlotAlt>();

        foreach (WeaponSlotAlt weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponType == weapon.weaponType)
            {
                //Does a free hardpoint exist?
                if (weaponSlot.GetWeapon() == null)
                {
                    //If so, attach to the free slot
                    weaponSlot.AttachWeapon(weapon);
                    return;
                }
                else
                {
                    weaponSlotsInUse.Add(weaponSlot);
                }
            }
        }

        //if no free hard points, decide how to attach the weapon. 
        //Current approach is to just find the first one (of the same type). 
        if (weaponSlotsInUse.Count > 0)
        {
            weaponSlotsInUse[0].AttachWeapon(weapon);
        }
    }

    protected virtual void FireWeapon(bool start, int slot)
    {
        WeaponAlt weap = weaponSlots[slot].GetWeapon();
        if (weap != null)
        {
            if (start)
            {
                weap.StartFire();
                if (animator != null) animator.SetBool("Attacking", true);
            }
            else
            {
                weap.EndFire();
                if (animator != null) animator.SetBool("Attacking", false);
            }
        }
    }

    public virtual void TakeDamage(float damage, ControllerAlt damageCauser = null)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        gameManager.GenerateSCT(damage.ToString(), transform);//create scrolling combat text

        //uIManager.SpawnHoverText(damage.ToString(), damageHoverText, transform);
        if (health <= 0f)
        {
            Death(damageCauser);
        }
    }

    public virtual void Death(ControllerAlt killer = null)
    {
        if (deathFX != null)
        {
            ParticleSystem p = Instantiate(deathFX);
            p.transform.position = transform.position;
            p.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (aimer != null) aimer.transform.eulerAngles = new Vector3(0, 0, rotation - 90f);

        switch (movementState)
        {
            case MovementState.Idle:
                SmoothDecelerate(0f, timeMaxToZero);
                break;
            case MovementState.Walking:
                SmoothAccelerate(GetFacingDirection(), maxMovementSpeed, timeZeroToMax);
                break;
        }
    }
}