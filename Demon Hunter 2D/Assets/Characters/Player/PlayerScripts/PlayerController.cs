using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    running,
    dashing,
    shooting
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Main Setup")]
    public PlayerState playerState;
    public int enemyEngagedCounter;
    [SerializeField] private GameObject playerAlive, playerDead;
    [SerializeField] private Collider2D playerBodyCollision;
    [System.NonSerialized] public bool deathEnabled; 
    public Transform[] engagementPositions;
    private EnemyController enemyControllerDetected;
    public List<GameObject> enemiesInRange = new List<GameObject>();

    [Header("Movement")]
    public float _currentMovementSpeed;    //speed 
    public float _defaultMovementSpeed;
    public float _shootingMovementSpeed;
    public float _meleeMovementSpeed;
    [SerializeField] private int direction;
    //[SerializeField] private bool isDashing;
    public bool canMove;

    Vector2 mousePoint;
    [Space(4)]


    [Header("Local Components")]
    private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _circleCollider; 
    [Space(4)]

    [Header("Scripts")]
    private C_Health _playerHealthComp;  
    private C_Health _enemyHealthComp;
    private PlayerEnergy _playerEnergy;
    private Animator _animator;
    private UIManager _uiManager;
    private PlayerCamera _playerCamera;
    [Space(4)]

    [Header("Animations")]
    //private Animation runF, runB, runR, runL, runFR, runFL, runBR, runBL;
    //private Animation iF, iB, iR, iL, iFR, iFL, iBR, iBL;
    [SerializeField] private int _playerState;

    private void Awake()
    {
        _playerHealthComp = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _playerCamera = GameObject.Find("CameraHolder").GetComponent<PlayerCamera>();
    }

    void Start()
    {
        deathEnabled = false;
        playerDead.SetActive(false);
        canMove = true;
        direction = 1;
        _playerState = 0;
        _circleCollider.enabled = false;
    }

    void Update()
    {
        if (_playerHealthComp.IsAlive())
        {
            if (canMove)
                PlayerMovement(_currentMovementSpeed);

            DirectionCheck();
        }
        else if (!deathEnabled)
        {
            deathEnabled = true;
            playerDead.SetActive(true);
            playerAlive.SetActive(false);
            StopVelocity();
        }

    }

    private void FixedUpdate()
    {
        CheckEnemyBlockers();
    }

    public Vector2 AimDirection()
    {
        Vector2 aimDirection;
        mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePoint - new Vector2(transform.position.x, transform.position.y);
        aimDirection = aimDirection.normalized;

        return aimDirection;
    }

    private void PlayerMovement(float x)
    {
        x *= Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(h, v);
        move = move.normalized * x;
        _rb.velocity = move;
    }


    public void StopVelocity()
    {
        _rb.velocity = new Vector2(0, 0);
        //_playerState = 0;
    }

    private void CheckEnemyBlockers()
    {
        if (enemiesInRange.Count > 0)
        {
            foreach (GameObject obj in enemiesInRange)
            {
                Vector2 rayDirection = obj.transform.position - transform.position;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 15);
                Debug.DrawRay(transform.position, rayDirection * 15, Color.red, 0.1f);

                if (hit.collider)
                {
                    //if collider is enemy collider
                    if (hit.collider.gameObject.layer == 10)
                    {
                        if (hit.collider.GetComponentInParent<EnemyController>())
                        {
                            enemyControllerDetected = hit.collider.GetComponentInParent<EnemyController>();
                            enemyControllerDetected.canRecieveDamage = true;
                        }
                    }//if collider is shield
                    else if (hit.collider.gameObject.layer == 17)
                    {
                        if (hit.collider.GetComponentInParent<EnemyController>())
                        {
                            enemyControllerDetected = hit.collider.GetComponentInParent<EnemyController>();
                            enemyControllerDetected.canRecieveDamage = false;
                        }
                    }
                }
                else
                {
                    if (enemyControllerDetected != null)
                        enemyControllerDetected.canRecieveDamage = false;
                }
            }
        }


        

        


    }

    private void SetAnimationPlay(int state)
    {
        /*
        if (state == 0)
        {
            if (_rb.velocity.x > 0 || _rb.velocity.y > 0)
            {
                print("Moving");
            }
            else if (_rb.velocity.x == 0 && _rb.velocity.y == 0)
            {
                print("Idle");
            }
        }
        else if (state == 1)
        {
            print("dash");
        }
        else if (state != 1 && state == 2)
        {
            print("shoot");
        }
        */
    }

    private int DirectionCheck()
    {
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

}
