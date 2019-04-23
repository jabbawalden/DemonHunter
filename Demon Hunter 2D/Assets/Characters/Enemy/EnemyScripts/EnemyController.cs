using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {patrol, engaged, disengaged, attacking}
public enum EnemyMovementType {ranged, melee}

public class EnemyController : MonoBehaviour
{
    public EnemyState enemyState;
    public EnemyMovementType enemyMovementType;

    [Header("Scripts")]
    private C_Health _healthComponent;
    public Transform playerRef;
    private PlayerController _playerController;
    [Space(4)]

    [Header("Movement")]
    Vector2 direction;
    private Vector3 _originalPosition;
    float patrolDistance;
    [SerializeField] float originDistance;
    private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _distanceKeptMin;
    [SerializeField] private float _distanceKeptMax;
    [SerializeField] private float _distanceKept;
    [SerializeField] private float _AggroRange;

    public Vector3 newDestination;
    public Vector3 newDirection;
    public bool haveDirectlyEngaged;
    private bool canChangeRandomPosition;
    [SerializeField] private int routePosition;
    [Space(4)]

    [Header("patrol")]
    [SerializeField] private float _patrolRateMin;
    [SerializeField] private float _patrolRateMax;
    [SerializeField] private float _newPatrolTime;
    [SerializeField] private float _deathLerpTime;
    [SerializeField] private float _destroyObjectSeconds;
    [Space(4)]

    [Header("Special Movement Behaviours")]
    public bool knockBack;
    [Space(4)]

    [Header("Prefabs and Objects")]
    public GameObject energyPickUp;
    public GameObject enemyAlive, enemyDead;
    private bool deathEnabled;
    Color opacityEnemyDead;

    private void Awake()
    {
        playerRef = GameObject.Find("PlayerMain").transform;
        opacityEnemyDead = enemyDead.GetComponent<SpriteRenderer>().color;
        deathEnabled = false;
        enemyDead.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _healthComponent = GetComponent<C_Health>();
    }

    void Start()
    {
        _distanceKept = Random.Range(_distanceKeptMin, _distanceKeptMax);
        _originalPosition = transform.position;
        knockBack = false;
        haveDirectlyEngaged = false;
        canChangeRandomPosition = true;
    }

    void Update()
    {
        if (_healthComponent.IsAlive())
        {
            DirectionCheck();
            EnemyMovement();
            StateManager();
            if (playerRef != null)
                TargetDistance();
            //print(DirectionCheck());
        }
        else if (!deathEnabled)
        {
            deathEnabled = true;
            Instantiate(energyPickUp, transform.position, Quaternion.identity);
            enemyAlive.SetActive(false);
            enemyDead.SetActive(true);
            _rb.velocity = new Vector2(0, 0);

            if (haveDirectlyEngaged)
            {
                haveDirectlyEngaged = false;
                if (playerRef)
                    playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter--;
            }
            opacityEnemyDead.a = Mathf.Lerp(opacityEnemyDead.a, 0, _deathLerpTime);
            enemyDead.GetComponent<SpriteRenderer>().color = opacityEnemyDead;
            Invoke("DestroyOurObject", _destroyObjectSeconds);
        }

        if (deathEnabled)
        {

        }
            
    }

    private void DestroyOurObject()
    {
        Destroy(gameObject);
    }

    public float TargetDistance()
    {
        float playerDist = 0;

        if (haveDirectlyEngaged)
            playerDist = Vector2.Distance(playerRef.position, transform.position);
        else
            playerDist = Vector2.Distance(playerRef.gameObject.GetComponentInParent<PlayerController>().engagementPositions[routePosition].position, transform.position);

        return playerDist;
    }

    IEnumerator RandomMoveTimer()
    {
        float r = Random.Range(1, 2);
        yield return new WaitForSeconds(r);
        if (playerRef)
            SetRoutedPosition(playerRef);
        canChangeRandomPosition = true;
    }

    private void EnemyMovement()
    {
        originDistance = Vector2.Distance(transform.position, _originalPosition);

        if (haveDirectlyEngaged)
        {
            direction = playerRef.position - transform.position;
        }
        else
        {
            direction = playerRef.gameObject.GetComponentInParent<PlayerController>().engagementPositions[routePosition].position - transform.position;

            if (canChangeRandomPosition)
            {
                canChangeRandomPosition = false;
                StartCoroutine(RandomMoveTimer());
            }
        }


        if (!knockBack)
        {
            switch (enemyState)
            {
                case EnemyState.engaged:

                    if (enemyMovementType == EnemyMovementType.ranged)
                    {
                        if (TargetDistance() > _distanceKept + 0.5f)
                            _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                        else if (TargetDistance() < _distanceKept - 1.5f)
                            _rb.velocity = direction.normalized * -_moveSpeed * Time.deltaTime;
                        else
                            _rb.velocity = new Vector2(0, 0);

                        //SetRoutedPosition(playerRef);

                    }
                    else
                    {
                        if (playerRef && !haveDirectlyEngaged && playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter < 5)
                        {
                            haveDirectlyEngaged = true;
                            playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter++;
                        }
                        
                        if (TargetDistance() > _distanceKept)
                            _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                        else
                            _rb.velocity = new Vector2(0,0);

                    }
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
        else if (knockBack)
        {
            _rb.velocity = new Vector2(0, 0);
            _rb.AddForce(-direction * 2000 * Time.deltaTime);
            Invoke("KnockBackStateReversion", 0.5f);
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
        if (!knockBack)
        {
            if (TargetDistance() <= _AggroRange)
            {
                if (originDistance <= 3.5f)
                {
                    enemyState = EnemyState.engaged;

                }
                else if (originDistance > 10)
                {
                    enemyState = EnemyState.patrol;

                    if (haveDirectlyEngaged)
                    {
                        haveDirectlyEngaged = false;
                        playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter--;
                    }
                }
            }
            else
            {
                enemyState = EnemyState.disengaged;
            }
        }
    }

    private void KnockBackStateReversion()
    {
        knockBack = false;

        if (playerRef)
            enemyState = EnemyState.engaged;
        else
            enemyState = EnemyState.patrol;

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

    private void SetRoutedPosition(Transform target)
    {
        routePosition = Random.Range(0, target.gameObject.GetComponentInParent<PlayerController>().engagementPositions.Length - 1);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            playerRef = collision.gameObject.transform;
            SetRoutedPosition(playerRef);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            playerRef = null;
        }
    }
    */
}
