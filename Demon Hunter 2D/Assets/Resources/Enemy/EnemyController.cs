using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {patrol, engaged, disengaged, attacking, dead}
public enum EnemyMovementType {ranged, melee}
public enum EnemySpecialType {none, shield}

public class EnemyController : MonoBehaviour
{
    public bool isVisible;
    public EnemyState enemyState;
    public EnemyMovementType enemyMovementType;
    public EnemySpecialType enemySpecialType;

    [Header("Scripts and Setup")]
    private C_Health _healthComponent;
/*    [System.NonSerialized]*/ public C_Health playerHealth;
    public Transform playerRef;
    private PlayerController playerController;
    private bool deathEnabled;
/*    [System.NonSerialized] */public bool canRecieveDamage;
    private bool haveAdded, haveRemoved;
    [Space(4)]

    [Header("Movement")]
    private Rigidbody2D rb;
    Vector2 direction;
    private Vector3 _originalPosition;
    float patrolDistance;
    private float originDistance;
    [SerializeField] private float distanceKept;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceKeptMin;
    [SerializeField] private float distanceKeptMax;

    [SerializeField] private float AggroRange;

    [SerializeField] private Vector3 newDestination;
    [SerializeField] private Vector3 newDirection;
    public bool haveDirectlyEngaged;
    private bool canChangeRandomPosition;
    [SerializeField] private int routePosition;
    [Space(4)]

    [Header("patrol")]
    private float newPatrolTime;
    [SerializeField] private float patrolRateMin;
    [SerializeField] private float patrolRateMax;
    [SerializeField] private float deathLerpTime;
    [SerializeField] private float destroyObjectSeconds;
    [Space(4)]

    [Header("Special Movement Behaviors")]
    public bool knockBack;
    [Space(4)]

    [Header("Prefabs and Objects")]
    //public GameObject energyPickUp;
    //switch to animation system instead
    public GameObject enemyAlive, enemyDead, enemyShield;
    Color opacityEnemyDead;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerRef = GameObject.Find("PlayerMain").transform;
        playerHealth = GameObject.Find("PlayerController").GetComponent<C_Health>();
        opacityEnemyDead = enemyDead.GetComponent<SpriteRenderer>().color;
        deathEnabled = false;
        enemyDead.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        _healthComponent = GetComponent<C_Health>();
    }

    void Start()
    {
        distanceKept = Random.Range(distanceKeptMin, distanceKeptMax);
        _originalPosition = transform.position;
        knockBack = false;
        haveDirectlyEngaged = false;
        canChangeRandomPosition = true;
        canRecieveDamage = true;
    }

    void Update()
    {
        PlayerEngagedState();

        if (_healthComponent.IsAlive())
        {
            DirectionCheck();
            EnemyMovement();
            StateManager();

            if (playerRef && playerController.playerHealthComp.IsAlive())
            {
                    TargetDistance();
            }
            //else
            //{
            //    print("player dead");
            //}
            //print(DirectionCheck());
        }
        else if (!deathEnabled)
        {
            deathEnabled = true;
            Instantiate(Resources.Load<GameObject>(AssetPaths.pref_energyDrop), transform.position, Quaternion.identity);
            enemyAlive.SetActive(false);
            enemyDead.SetActive(true);
            if (enemyShield)
                enemyShield.SetActive(false);
            rb.velocity = new Vector2(0, 0);
            if (haveDirectlyEngaged)
            {
                haveDirectlyEngaged = false;
                if (playerRef)
                    playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter--;
            }
        }

        if (deathEnabled)
        {
            enemyState = EnemyState.dead;
            opacityEnemyDead.a = Mathf.Lerp(opacityEnemyDead.a, 0, deathLerpTime);
            enemyDead.GetComponent<SpriteRenderer>().color = opacityEnemyDead;
            Invoke("DestroyOurObject", destroyObjectSeconds);
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
                        if (TargetDistance() > distanceKept + 0.5f)
                            rb.velocity = direction.normalized * moveSpeed * Time.deltaTime;
                        else if (TargetDistance() < distanceKept - 1.5f)
                            rb.velocity = direction.normalized * -moveSpeed * Time.deltaTime;
                        else
                            rb.velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        if (playerRef && !haveDirectlyEngaged && playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter < 5)
                        {
                            haveDirectlyEngaged = true;
                            playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter++;
                        }
                        
                        if (TargetDistance() > distanceKept)
                            rb.velocity = direction.normalized * moveSpeed * Time.deltaTime;
                        else
                            rb.velocity = new Vector2(0,0);
                    }
                    break;

                case EnemyState.attacking:
                    rb.velocity = new Vector2(0, 0);
                    break;

                case EnemyState.disengaged:
                    direction = _originalPosition - transform.position;
                    rb.velocity = direction.normalized * moveSpeed * Time.deltaTime;
                    break;

                case EnemyState.patrol:
                    SetPatrol();
                    patrolDistance = Vector2.Distance(transform.position, newDestination);
                    direction = newDirection;
                    if (patrolDistance >= 0.15f)
                        rb.velocity = direction.normalized * moveSpeed * Time.deltaTime;
                    else
                        rb.velocity = new Vector2(0, 0);
                    break;
            }
        }
        else if (knockBack)
        {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(-direction * 2000 * Time.deltaTime);
            Invoke("KnockBackStateReversion", 0.5f);
        }
    }

    private void PlayerEngagedState()
    {
        if (enemyState == EnemyState.engaged)
        {
            if (!haveAdded)
            {
                haveAdded = true;
                haveRemoved = false;

                if (playerRef)
                {
                    playerRef.GetComponentInParent<PlayerController>().inCombat = true;
                    playerRef.GetComponentInParent<PlayerController>().enemyInRangeCounter++;
                }
            }
        }
        else
        {
            if (!haveRemoved)
            {
                //print("remove enemy counter");
                haveRemoved = true;
                haveAdded = false;

                if (playerRef)
                {
                    playerRef.GetComponentInParent<PlayerController>().enemyInRangeCounter--;

                    if (playerRef.GetComponentInParent<PlayerController>().enemyInRangeCounter <= 0)
                    {
                        playerRef.GetComponentInParent<PlayerController>().inCombat = false;
                        playerRef.GetComponentInParent<PlayerController>().enemyInRangeCounter = 0;
                    }
                }
            }
        }
    }

    private void SetPatrol()
    {
        if (newPatrolTime <= Time.time)
        {
            float rate = Random.Range(patrolRateMin, patrolRateMax);

            newDestination = SetPatrolDestination();
            newDirection = newDestination - transform.position;
            newPatrolTime = Time.time + rate;
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
            if (playerController.playerHealthComp.IsAlive())
            {
                if (TargetDistance() <= AggroRange)
                {
                    if (originDistance <= 3.8f)
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
                    enemyState = EnemyState.patrol;

                    if (haveDirectlyEngaged)
                    {
                        haveDirectlyEngaged = false;
                        playerRef.gameObject.GetComponentInParent<PlayerController>().enemyEngagedCounter--;
                    }
                }
            }
            else //if the player has died
            {
                enemyState = EnemyState.patrol;
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

        if (rb.velocity.y > 0 && rb.velocity.x == 0)
        {
            direction = 1; //forward
            //_animator.Play("Run_Forward");
            //_animator.SetInteger("State", 1);
        }
        else if (rb.velocity.y < 0 && rb.velocity.x == 0)
        {
            direction = 2; //back
            //_animator.Play("Run_Back");
            //_animator.SetInteger("State", 2);
        }
        else if (rb.velocity.x > 0 && rb.velocity.y == 0)
        {
            direction = 3; //right
            //_animator.Play("Run_Right");
            //_animator.SetInteger("State", 3);
        }
        else if (rb.velocity.x < 0 && rb.velocity.y == 0)
        {
            direction = 4; //left
            //_animator.Play("Run_Left");
            //_animator.SetInteger("State", 4);
        }
        else if (rb.velocity.y > 0 && rb.velocity.x > 0)
        {
            direction = 5;   //forward right
            //_animator.SetInteger("State", 5);
        }
        else if (rb.velocity.y > 0 && rb.velocity.x < 0)
        {
            direction = 6;  //forward left
            //_animator.SetInteger("State", 6);
        }
        else if (rb.velocity.y < 0 && rb.velocity.x > 0)
        {
            direction = 7;  //back right
            //_animator.SetInteger("State", 7);
        }
        else if (rb.velocity.y < 0 && rb.velocity.x < 0)
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

    private void OnBecameVisible()
    {
        if (enemySpecialType == EnemySpecialType.shield)
        {
            isVisible = true;
            playerController = playerRef.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                playerController.enemiesInRange.Add(this.gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (enemySpecialType == EnemySpecialType.shield)
        {
            isVisible = false;
            if (playerController)
            {
                playerController.enemiesInRange.Remove(this.gameObject);
            }
        }
    }
}
