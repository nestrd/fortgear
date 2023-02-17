using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : ControllerAlt
{

#region Variables
    [SerializeField] private GameObject cursorRef;
    private Transform cursorTarget;

    protected Vector2 input;
    protected float scrollDir;
    private float t;
    protected Vector2 screenBounds;
    protected Vector2 characterBounds;

    public PlayerGear currentCog;
    public GameObject cogOrigin;

    private bool isDodging;
    public AudioSource dodgeSound;

    [HideInInspector] public bool isThrowing;

    [SerializeField] private GameObject uiTextRef;
    [SerializeField] private GameObject enteredDoor;

#endregion

    protected override void Awake()
    {
        base.Awake();
        dodgeSound = GetComponent<AudioSource>();

        t = 0.0F;
    }

    void Update()
    {
        if(GetComponent<PlayerInput>().enabled == true)
        {
            AimingLookAt();
        }
    }

    protected override void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = input * maxMovementSpeed;
    }


    #region Player Inputs
    void AimingLookAt()
    {
        //Player character looks at cursor
        Vector3 temp = cursorRef.transform.position - transform.position;
        temp.z = 0;
        temp.Normalize();
        transform.up = temp;

    }

    public void OnMovement(InputAction.CallbackContext context)
    {

        input = context.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            SetMovementState(MovementState.Walking);
        }
        else
        {
            SetMovementState(MovementState.Idle);
        }

    }

    public void OnThrowing(InputAction.CallbackContext context)
    {
        if (context.started && currentCog._cogManager.currentCogState is HeldState)
        {
            currentCog._cogManager.SwitchState(typeof (ThrownState));
        }
    }

    public void OnActivating(InputAction.CallbackContext context)
    {
        scrollDir = context.ReadValue<float>();
        if (scrollDir >= 1.0f && currentCog._cogManager.currentCogState is HeldState)
        {
            currentCog._cogManager.SwitchState(typeof(ActivatingState));
        }
    }

    public void OnDeactivating(InputAction.CallbackContext context)
    {
        scrollDir = context.ReadValue<float>();
        if (scrollDir <= 0.0f && currentCog._cogManager.currentCogState is HeldState)
        {
            currentCog._cogManager.SwitchState(typeof(DeactivatingState));
        }
    }

    public void OnDodging(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isDodging == false)
            {
                StartCoroutine("Dodging");
            }
            if (uiTextRef.activeInHierarchy == true && uiTextRef.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("UI_ToolTip"))
            {
                uiTextRef.GetComponent<Animator>().SetTrigger("UITooltipAccept");
            }
            if (rb.velocity.magnitude >= 2.5f)
            {
                dodgeSound.PlayOneShot(dodgeSound.clip);
            }
        }


    }

    #endregion

    #region Player Conditions

    //Entering/Exiting doorway & pickups
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            if (uiTextRef.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("UI_ToolTipFadeIn") || uiTextRef.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("UI_ToolTip"))
            {
                uiTextRef.GetComponent<Animator>().SetTrigger("UITooltipAccept");

            }
            enteredDoor = collision.gameObject;
            animator.SetTrigger("ExitingRoom");
            GetComponent<PlayerInput>().enabled = false;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            Invoke(nameof(ExitingRoom), 1.0f);
        }
        if (collision.CompareTag("PlayerGear"))
        {
            currentCog.gameObject.SetActive(true);
            Destroy(collision.gameObject);
        }
    }


    private void ExitingRoom()
    {
        transform.position = enteredDoor.GetComponent<DoorTrigger>().offset.transform.position;
        animator.SetTrigger("EnteringRoom");
        GetComponent<PlayerInput>().enabled = true;
        Invoke(nameof(EnteringRoom), 3.0f);
    }

    private void EnteringRoom()
    {
        SceneManager.UnloadSceneAsync(1);
        animator.SetTrigger("Entered");
    }

    private IEnumerator Dodging()
    {
        isDodging = true;
        t = 0.0F;
        if(input != Vector2.zero)
        {
            while (t <= 0.5f)
            {
                Vector2 dodgeVelocity = input * maxMovementSpeed * 8f;
                rb.AddForce(dodgeVelocity, ForceMode2D.Force);
                t += Time.deltaTime;
                yield return null;
            }
        }
        isDodging = false;
        yield break;
    }

    public override void TakeDamage(float damage, ControllerAlt damageCauser = null)
    {
        base.TakeDamage(damage, damageCauser);

    }

    public override void Death(ControllerAlt killer = null)
    {
        Reset();
        gameManager.RespawnPlayer();
        base.Death(killer);
    }

#endregion

}
