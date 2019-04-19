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
    public PlayerState playerState;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed;    //speed 
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int direction;
    //[SerializeField] private bool isDashing;
    public bool canMove;

    [SerializeField] private bool isDashing;
    Vector2 mousePoint;

    [Space(4)]

    [Header("Main Properties")]
    [SerializeField] private bool _canDashDamage;
    [SerializeField] private float _dashDamage;
    [SerializeField] private float _dashHealAmount;
    [SerializeField] private float _dashEnergyCost;
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

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        direction = 1;
        _playerState = 0;
        _canDashDamage = false;
        _circleCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealthComp.IsAlive())
        {
            if (canMove)
                PlayerMovement(_movementSpeed);

            DirectionCheck();
            Dash();
        }
        else
        {
            StopVelocity();
        }
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

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _dashEnergyCost)
        {
            _playerEnergy.RemoveEnergy(_dashEnergyCost);
            //TODO - only dash when energy is equal or above dash cost
            StartCoroutine(DashBehaviour(4.5f, 0.3f));
            _uiManager.UpdateEnergySlider();
            _playerCamera.CameraShake();
        }
    }

    IEnumerator DashBehaviour(float time, float speed)
    {
        float count = 0;
        Vector2 currentAimDirection = AimDirection();

        canMove = false;
        _canDashDamage = true;
        _circleCollider.enabled = true;
        

        while (count < time)
        {
            _playerState = 1;
            count += speed;
            _rb.velocity = currentAimDirection * _dashSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;
        _canDashDamage = false;
        _circleCollider.enabled = false;
    }

    public void StopVelocity()
    {
        _rb.velocity = new Vector2(0, 0);
        isDashing = false;
        _playerState = 0;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && _canDashDamage && collision.GetComponentInParent<C_Health>() != null)
        {
            _enemyHealthComp = collision.GetComponentInParent<C_Health>();
            /*
            //if they are about to die
            if (_enemyHealthComp._currentHealth <= _dashDamage)
            {
                _playerHealthComp.Heal(_dashHealAmount);
            }
            */
            _playerHealthComp.Heal(_dashHealAmount);
            _enemyHealthComp.Damage(_dashDamage);
            _uiManager.UpdateHealthSlider();

            //print("hit enemy");
        }
    }

}
