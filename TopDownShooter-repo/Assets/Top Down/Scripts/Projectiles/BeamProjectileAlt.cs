using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjectileAlt : ProjectileAlt
{
    public LayerMask layerMask;

    public float accuracy;

    public float beamTime;
    protected float beamTimer;

    public float maxRange;

    protected LineRenderer lineRenderer;
    Vector3 endPointOffset, targetPosition;
    Transform target;

    public override void InitProjectile(GameObject ownerGO, WeaponAlt weapon, Transform firePoint)
    {
        base.InitProjectile(ownerGO, weapon, firePoint);

        lineRenderer = GetComponent<LineRenderer>();        
        lineRenderer.enabled = true;

        target = weapon.transform.root.GetComponent<AIControllerAlt>().Target;

        if (target != null)
        {
            endPointOffset = Random.insideUnitCircle * accuracy;
            targetPosition = target.position;
        }
    }

    public void stopVFX()
    {
        if (lineRenderer != null && lineRenderer.enabled == true)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (target == null || lineRenderer == null || firePoint == null)
        {
            stopVFX();
            return;
        }

        if (beamTimer >= beamTime)
        {
            stopVFX();
            beamTimer = 0;
            Destroy(gameObject);
        }
        else
        {
            beamTimer += Time.deltaTime;

            lineRenderer.SetPosition(0, firePoint.position);           

            Vector3 dir = (targetPosition + endPointOffset) - firePoint.position;

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, dir, maxRange, layerMask);
           
            if (hitInfo.collider != null)
            {
                lineRenderer.SetPosition(1, hitInfo.point);
                OnTriggerEnter2D(hitInfo.collider);
            }
            else
            {
                Vector2 endPoint = targetPosition + endPointOffset;
                lineRenderer.SetPosition(1, endPoint);
            }
        }
    }
}
