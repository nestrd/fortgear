using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SplineMovement : MonoBehaviour
{
    private bool init = false;

    public PathCreator path;
    public EndOfPathInstruction endPathInstruction = EndOfPathInstruction.Stop;
    public bool moveOnStart;
    public bool destroyOnComplete;
    public bool destroyPathObjectOnComplete;
    public Transform root;
    public float speed;
    public AnimationCurve speedOverLife;
    public int maxLoops;
    public bool rotationFollowsMovement;

    protected PathCreator pathCreator;
    protected GameObject pathObject;
    protected float distanceTravelled;
    protected int numLoops;
    protected bool isReversing;
    public UnityEvent reachedEndOfPath;
    protected bool active;
    

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if(!init)
        {
            reachedEndOfPath = new UnityEvent();
            reachedEndOfPath.AddListener(OnReachedEndOfPath);

            if (path == null)
            {
                Debug.Log("No path found, assign in inspector");
                Destroy(gameObject);
            }

            if (moveOnStart)
            {
                InitMovement();
                StartMovement();
            }

            init = true;
        }
    }

    public virtual void InitMovement()
    {        
        pathObject = Instantiate(path.gameObject);
        pathCreator = pathObject.GetComponent<PathCreator>();
        pathObject.transform.position = transform.position;
        pathObject.transform.rotation = transform.rotation;

        distanceTravelled = 0;

        if (root != null)
        {
            pathObject.transform.parent = root;
        }

        switch (endPathInstruction)
        {
            case EndOfPathInstruction.Stop:
                speedOverLife.postWrapMode = WrapMode.Clamp;
                break;

            case EndOfPathInstruction.Loop:
                speedOverLife.postWrapMode = WrapMode.Loop;
                break;

            case EndOfPathInstruction.Reverse:
                speedOverLife.postWrapMode = WrapMode.PingPong;
                break;
        }
    }

    public void StartMovement()
    {
        active = true;
    }

    public void StopMovement()
    {
        active = false;

        if (destroyPathObjectOnComplete == true)
        {
            Destroy(pathObject);
        }
    }

    void Update()
    {
        if (active)
        {
            if (pathCreator != null)
            {
                //Distance
                float linearT = distanceTravelled / pathCreator.path.length;
                float scaledSpeed = speedOverLife.Evaluate(linearT);
                distanceTravelled += (speed * scaledSpeed) * Time.deltaTime;

                //Projectile Movement
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endPathInstruction);

                //Rotation
                if (rotationFollowsMovement)
                {
                    Vector3 direction = pathCreator.path.GetDirectionAtDistance(distanceTravelled);
                    if (isReversing)
                    {
                        direction *= -1;
                    }

                    transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
                }

                //End Path
                if (distanceTravelled >= (pathCreator.path.length * (numLoops + 1)))
                {
                    reachedEndOfPath.Invoke();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void OnReachedEndOfPath()
    {
        switch (endPathInstruction)
        {
            case EndOfPathInstruction.Loop:
            case EndOfPathInstruction.Reverse:
                if (numLoops == maxLoops - 1)
                {
                    if (destroyOnComplete)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        StopMovement();
                    }
                }
                else
                {
                    numLoops++;
                }
                if (endPathInstruction == EndOfPathInstruction.Reverse)
                {
                    isReversing = !isReversing;
                }

                break;
            case EndOfPathInstruction.Stop:
                if (destroyOnComplete)
                {
                    Destroy(gameObject);
                }
                else if(destroyPathObjectOnComplete)
                {
                    Destroy(pathObject);
                }
                break;
        }
    }    

    protected void OnDestroy()
    {
        Destroy(pathObject);
    }
}
