using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType
{
    Standard,
    Automatic,
}

public class WeaponAlt : MonoBehaviour
{
    public bool active;
    public GameObject projectilePrefab;
    public float fireRate;
    public WeaponType weaponType;
    public FireType fireType;
    public ParticleSystem muzzleFlashFX;
    public Sprite icon;
    public float fireTimer { get => _fireTimer; set => _fireTimer = value; }

    protected bool firing;
    protected List<Transform> firePoints = new List<Transform>();
    protected float _fireTimer;    

    private void Awake()
    {
        foreach (Transform T in GetComponentsInChildren<Transform>())
        {
            if (T != transform && T.GetComponent<SpriteRenderer>() == null) firePoints.Add(T);
        }
    }

    public bool isFiring()
    {
        return firing;
    }

    protected void reset()
    {
        firing = false;
        fireTimer = 0f;
    }

    public void activate()
    {
        active = true;

        reset();
    }

    public virtual void StartFire()
    {
        if (active)
        {
            switch (fireType)
            {
                case FireType.Standard:
                    Fire();
                    break;
                case FireType.Automatic:
                    InvokeRepeating("Fire", 0, fireRate);
                    break;
            }
        }
    }

    public virtual void EndFire()
    {
        if (active)
        {
            switch (fireType)
            {
                case FireType.Standard:
                    break;
                case FireType.Automatic:
                    CancelInvoke("Fire");
                    break;
            }
        }
    }

    protected virtual void Fire()
    {
        if (!isFiring())
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                if (muzzleFlashFX != null) Instantiate(muzzleFlashFX, firePoint.position, firePoint.rotation);

                ProjectileAlt projectile = go.GetComponent<ProjectileAlt>();

                if (projectile != null)
                {
                    projectile.InitProjectile(transform.root.gameObject, this, firePoint);
                }                
            }

            firing = true;
            Invoke("reset", fireRate);            
        }
    }
    
    protected virtual void Update()
    {
        if (!active) return;

        if (isFiring())
        {
            fireTimer += Time.deltaTime;

            /*
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
            }*/
        }
    }
}
