using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileAlt : MonoBehaviour
{
    public float speed;
    public float damage;
    public ParticleSystem hitFX;

    protected ControllerAlt owner;
    protected WeaponAlt weapon;
    protected Transform firePoint;
    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void InitProjectile(GameObject ownerGO, WeaponAlt weapon, Transform firePoint)
    {
        owner = ownerGO.GetComponent<ControllerAlt>();

        if (ownerGO.layer == LayerMask.NameToLayer("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy Projectile");
        }

        this.weapon = weapon;
        this.firePoint = firePoint;
    }

    protected virtual void FixedUpdate()
    {
        if (speed > 0)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        //Override with custom movement
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == true) return;

        ControllerAlt controller = other.gameObject.GetComponent<ControllerAlt>();
        
        if (controller != null)
        {
            if(owner != controller)
            {
                controller.TakeDamage(damage, owner);

                if(GetComponent<CinemachineImpulseSource>() != null)
                    GetComponent<CinemachineImpulseSource>().GenerateImpulse();


                if (hitFX != null) Instantiate(hitFX, transform.position, transform.rotation);
            }            
        }
        else
        {
            if (hitFX != null) Instantiate(hitFX, transform.position, transform.rotation);
        }        
    }
}
