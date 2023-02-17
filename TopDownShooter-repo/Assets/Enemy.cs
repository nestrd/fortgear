using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Enemy States

public class EnemyStateManager
{
    private Type _currentState = typeof(EnemyStates);
    public EnemyStates currentEnemyState;
    private Enemy enemyRef;

    public EnemyStateManager(Enemy _enemyRef)
    {
        enemyRef = _enemyRef;
        SwitchState(typeof(SeekingState));
    }

    public void SwitchState(Type enemyStates)
    {
        _currentState = enemyStates;
        currentEnemyState = (EnemyStates)Activator.CreateInstance(_currentState);
        currentEnemyState.OnProcessing(enemyRef);

    }

}

public class EnemyStates
{
    public virtual void OnProcessing(Enemy enemyRef)
    {
        throw new System.NotImplementedException();
    }
}

public class SeekingState : EnemyStates
{
    public override void OnProcessing(Enemy enemyRef)
    {
        enemyRef.OnSeeking();
        return;
    }
}

public class ChasingState : EnemyStates
{
    public override void OnProcessing(Enemy enemyRef)
    {
        enemyRef.OnChasing();
        return;
    }
}

public class AttackingState : EnemyStates
{
    public override void OnProcessing(Enemy enemyRef)
    {
        enemyRef.OnAttacking();
        return;
    }
}

#endregion


public class Enemy : MonoBehaviour
{
    protected EnemyStateManager _stateManager;
    protected bool isChasing;
    public GameObject _playerRef;

    protected Rigidbody2D enemyRb;

    public float maxHealth = 10.0f;
    protected float health;
    [SerializeField] protected Vector2 detectionRadius = new Vector2(1.0f, 1.0f);
    [SerializeField] protected float hitRadius = 0.3f;
    protected float moveSpeed = 10.0f;

    private void Awake()
    {
        _stateManager = new EnemyStateManager(this);
        enemyRb = GetComponent<Rigidbody2D>();

        health = maxHealth;

        if (GetComponent<CircleCollider2D>().isTrigger == false)
        {
            GetComponent<CircleCollider2D>().radius = hitRadius;
        }
        if(GetComponent<CircleCollider2D>().isTrigger == true)
        {
            GetComponent<BoxCollider2D>().size = detectionRadius;
        }

        _playerRef = FindObjectOfType<PlayerController>().gameObject;

    }

    private void Update()
    {
        //Always look at player
        Vector3 temp = _playerRef.transform.position - transform.position;
        temp.z = 0;
        temp.Normalize();
        transform.up = temp;
    }

    #region Enemy Behaviors
    #region Seeking
    public virtual void OnSeeking()
    {

    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            isChasing = true;
            _stateManager.SwitchState(typeof(ChasingState));
        }
    }
    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            _stateManager.SwitchState(typeof(SeekingState));
        }
    }
    #endregion

    #region Chasing
    public virtual void OnChasing()
    {
        isChasing = true;
        StartCoroutine(nameof(Chasing));
    }

    public virtual IEnumerator Chasing()
    {
        var t = 0.0F;
        while (t < 0.5f)
        {
            t = 0.0F;
            //enemyRb.MovePosition(transform.position - (_playerRef.transform.position * moveSpeed * Time.deltaTime));
            Vector3 temp = _playerRef.transform.position - transform.position;
            temp.z = 0;
            temp.Normalize();
            enemyRb.AddForce(temp * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            t += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(3);
        yield return 0;
    }

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            _stateManager.SwitchState(typeof(AttackingState));
        }
    }
    private void OnCollisionExit2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            _stateManager.SwitchState(typeof(SeekingState));
        }
    }
    #endregion

    #region Attacking
    public virtual void OnAttacking()
    {
        _playerRef.GetComponent<PlayerController>().TakeDamage(10);
    }


    #endregion

    #endregion

    #region Enemy Conditions
    public virtual void TakeDamage(float damage, ControllerAlt damageCauser = null)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0f)
        {
            Destroy(this);
        }

    }
    #endregion
}
