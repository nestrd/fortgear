using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerAlt : ControllerAlt
{
    //Protected
    protected Vector2 input;
    protected Vector2 screenBounds;
    protected Vector2 characterBounds;

    protected override void Awake()
    {
        base.Awake();

        //screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //characterBounds = new Vector2(GetComponentInChildren<SpriteRenderer>().bounds.extents.x, GetComponentInChildren<SpriteRenderer>().bounds.extents.y);
    }

    void Update()
    {
        if (input != Vector2.zero)
        {
            SmoothRotate(input, turnRate);
        }

        //Animations
        animator.SetFloat("Speed", rb.velocity.magnitude);

        //Transform mousePosition = Input.mousePosition;
        //transform.rotation = transform.LookAt(Camera.ScreenToWorldPoint());
    }

    protected void LateUpdate()
    {
        /* Vector2 viewPos = transform.position;
         viewPos.x = Mathf.Clamp(viewPos.x, (screenBounds.x * -1) + characterBounds.x, screenBounds.x - characterBounds.x);
         viewPos.y = Mathf.Clamp(viewPos.y, (screenBounds.y * -1) + characterBounds.y, screenBounds.y - characterBounds.y);
         transform.position = viewPos;*/
    }

    public override void TakeDamage(float damage, ControllerAlt damageCauser = null)
    {
        //animator.SetTrigger("Hurt");
        base.TakeDamage(damage, damageCauser);

        //Health UI
        //uIManager.UpdateHealthBar(health, maxHealth);
    }

    public override void Death(ControllerAlt killer = null)
    {
        Reset();
        gameManager.RespawnPlayer();
        base.Death(killer);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Animations
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);

        //Input
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

    public void OnWeapon0(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireWeapon(true, 0);
        }
        else if (context.canceled)
        {
            FireWeapon(false, 0);
        }
    }

    public void OnWeapon1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireWeapon(true, 1);
        }
        else if (context.canceled)
        {
            FireWeapon(false, 1);
        }
    }

    public void OnWeapon2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireWeapon(true, 2);
        }
        else if (context.canceled)
        {
            FireWeapon(false, 2);
        }
    }
}
