using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathCreation;

public enum TreeBossStates { Idle, Pattern, Dead }

[RequireComponent(typeof(PathManager))]
[RequireComponent(typeof(SplineMovement))]
public class TreeBossAlt : AIControllerAlt
{
    protected PathManager pathManager;
    protected int pathIndex;
    protected SplineMovement splineMovement;

    public TreeBossStates state;

    public float visionRange = 10f;

    protected override void Awake()
    {
        base.Awake();

        pathManager = GetComponent<PathManager>();
        splineMovement = GetComponent<SplineMovement>();
        splineMovement.Init();

        pathIndex = 0;
        splineMovement.path = pathManager.getPath(pathIndex);

        splineMovement.endPathInstruction = EndOfPathInstruction.Stop;
        splineMovement.destroyOnComplete = false;
        splineMovement.reachedEndOfPath.AddListener(OnPatternComplete);

        Debug.Log(splineMovement.reachedEndOfPath.GetPersistentEventCount());

        state = TreeBossStates.Idle;
    }

    protected override void processAI()
    {
        switch (state)
        {
            case TreeBossStates.Idle:
                Idle();
                break;

            case TreeBossStates.Pattern:
                Pattern();
                break;
        }
    }
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(GetFacingDirection().x, GetFacingDirection().y, 0) * 2, Color.red);
    }

    public override void Death(ControllerAlt killer = null)
    {
        base.Death(killer);
        if (killer != null) killer.gameManager.progressionManager.RewardKillXP();

        // stop all weapons firing
        FireWeapon(false, 0);
        FireWeapon(false, 1);
        FireWeapon(false, 2);

        Destroy(gameObject);
    }

    protected void Idle()
    {
        //print("Idle");

        movementState = MovementState.Idle;

        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) < visionRange)
            {
                pathIndex = 0;
                splineMovement.path = pathManager.getPath(pathIndex);
                splineMovement.InitMovement();
                splineMovement.StartMovement();
                state = TreeBossStates.Pattern;
            }
        }
    }

    protected void Pattern()
    {
        //print("Pattern");

        if (target != null)
        {
            movementState = MovementState.Idle;

            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

            switch (pathIndex)
            {
                case 0:
                    weaponSlots[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    FireWeapon(true, 0);
                    break;

                case 1:
                    FireWeapon(true, 1);
                    FireWeapon(true, 2);
                    break;

                case 2:
                    weaponSlots[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    FireWeapon(true, 0);
                    FireWeapon(true, 1);
                    FireWeapon(true, 2);
                    break;
            }
        }
    }

    protected void Dead()
    {
        //print("Dead");
    }

    public void OnPatternComplete()
    {
        pathIndex++;

        if (pathIndex >= pathManager.paths.Count)
        {
            pathIndex = 0;
        }

        splineMovement.path = pathManager.getPath(pathIndex);
        splineMovement.InitMovement();
        splineMovement.StartMovement();
    }
}
