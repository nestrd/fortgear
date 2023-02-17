using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingProjectileAlt : ProjectileAlt
{
    public float destroyDelay = 10.0f;

    public override void InitProjectile(GameObject ownerGO, WeaponAlt weapon, Transform firePoint)
    {
        base.InitProjectile(ownerGO, weapon, firePoint);

        DestroyProjectile();
    }

    protected override void Move()
    {
        rb.velocity = transform.up * speed;
    }

    protected virtual void DestroyProjectile()
    {
        Destroy(gameObject, destroyDelay);
    }
}
