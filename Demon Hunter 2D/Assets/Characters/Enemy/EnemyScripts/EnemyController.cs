using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {patrol, engaged, disengaged, attacking}

public class EnemyController : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("Scripts")]
    private C_Health _healthComponent;
    [HideInInspector] public Transform playerRef;
    [Space(4)]

    [Header("Variables")]
    private Vector3 _originalPosition;
    float patrolDistance;
    float originDistance;
    private Rigidbody2D _rb;
    [SerializeField] private bool _inRange;
    [SerializeField] private float _moveSpeed; 
    [SerializeField] private float _patrolRateMin;
    [SerializeField] private float _patrolRateMax;
    [SerializeField] private float _newPatrolTime;
    public Vector3 newDestination;
    public Vector3 newDirection;
    [Space(4)]

    [Header("Prefabs")]
    public GameObject energyPickUp;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _healthComponent = GetComponent<C_Health>();
    }

    void Start()
    {
        _originalPosition = transform.position;
        _inRange = false;
    }

    void Update()
    {
        if (_healthComponent.isAlive())
        {
            DirectionCheck();
            EnemyMovement();
            StateManager();
            if (playerRef != null)
                PlayerDistance();
            //print(DirectionCheck());
        }
        else
        {
            Instantiate(energyPickUp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
            
    }

    public float PlayerDistance()
    {
        float playerDist = Vector2.Distance(playerRef.position, transform.position);
        //print(playerDist);
        return playerDist;
    }

    private void EnemyMovement()
    {
        originDistance = Vector2.Distance(transform.position, _originalPosition);
        Vector2 direction = new Vector2(0, 0);

        switch(enemyState)
        {
            case EnemyState.engaged:
                if (playerRef != null)
                    direction = playerRef.position - transform.position;
                if (PlayerDistance() > 0.5f)
                    _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                break;

            case EnemyState.attacking:
                _rb.velocity = new Vector2(0, 0);
                break;

            case EnemyState.disengaged:
                direction = _originalPosition - transform.position;
                _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                break;

            case EnemyState.patrol:
                SetPatrol();
                patrolDistance = Vector2.Distance(transform.position, newDestination);
                direction = newDirection;
                if (patrolDistance >= 0.25f)
                    _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                else
                    _rb.velocity = new Vector2(0, 0);
                break;
        }
        
    }



    private void SetPatrol()
    {
        if (_newPatrolTime <= Time.time)
        {
            float rate = Random.Range(_patrolRateMin, _patrolRateMax);

            newDestination = SetPatrolDestination();
            newDirection = newDestination - transform.position;
            _newPatrolTime = Time.time + rate;
        }
    }

    private Vector3 SetPatrolDestination()
    {
        float rX = Random.Range(-2f, 2f);
        float rY = Random.Range(-2f, 2f);

        if (rX < 0.5)
            rX += 1;
        else if (rX < -0.5)
            rX -= 1;

        if (rY < 0.5)
            rY += 1;
        else if (rY < -0.5)
            rY -= 1;

        Vector3 patrolDestination = new Vector3(_originalPosition.x + rX, _originalPosition.y + rY);
        return patrolDestination;
    }

    private void StateManager()
    {
        if (enemyState != EnemyState.engaged)
        {
            //if we aren't on patrol or are not attacking and not engaged
            if (originDistance >= 0.25f && enemyState != EnemyState.patrol && enemyState != EnemyState.attacking)
                enemyState = EnemyState.disengaged;
            else if (originDistance <= 0.25f)
                enemyState = EnemyState.patrol;
        }
    }

    private int DirectionCheck()
    {
        int direction = 1;

        if (_rb.velocity.y > 0 && _rb.velocity.x == 0)
        {
            direction = 1; //forward
            //_animator.Play("Run_Forward");
            //_animator.SetInteger("State", 1);
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x == 0)
        {
            direction = 2; //back
            //_animator.Play("Run_Back");
            //_animator.SetInteger("State", 2);
        }
        else if (_rb.velocity.x > 0 && _rb.velocity.y == 0)
        {
            direction = 3; //right
            //_animator.Play("Run_Right");
            //_animator.SetInteger("State", 3);
        }
        else if (_rb.velocity.x < 0 && _rb.velocity.y == 0)
        {
            direction = 4; //left
            //_animator.Play("Run_Left");
            //_animator.SetInteger("State", 4);
        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x > 0)
        {
            direction = 5;   //forward right
            //_animator.SetInteger("State", 5);
        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x < 0)
        {
            direction = 6;  //forward left
            //_animator.SetInteger("State", 6);
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x > 0)
        {
            direction = 7;  //back right
            //_animator.SetInteger("State", 7);
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x < 0)
        {
            direction = 8;  //back left
            //_animator.SetInteger("State", 8);
        }

        //Debug.Log(_rb.velocity);
        return direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //print("Player in range");
            playerRef = collision.gameObject.transform;
            enemyState = EnemyState.engaged;
            _inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //print("Player out of range");
            enemyState = EnemyState.disengaged;
            _inRange = false;
        }
    }

}
