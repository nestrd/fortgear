using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SplineMovement))]
public class SplineProjectileAlt : ProjectileAlt
{
    public bool rootMotion;

    protected SplineMovement splineMovement;
        
    public override void InitProjectile(GameObject ownerGO, WeaponAlt weapon, Transform firePoint)
    {
        base.InitProjectile(ownerGO, weapon, firePoint);
        splineMovement = GetComponent<SplineMovement>();
        splineMovement.speed = speed;

        if(rootMotion)
        {
            splineMovement.root = firePoint;
        }

        splineMovement.InitMovement();
        splineMovement.StartMovement();
    }
}
