using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed;    //speed 
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int direction;
    //[SerializeField] private bool isDashing;
    public bool canMove;
    [Space(4)]

    [Header("Main Properties")]
    [SerializeField] private bool _canDamage;
    [SerializeField] private float dashDamage;
    [SerializeField] private float _dashEnergyCost;
    public float maxEnergy;
    public float currentEnergy;
    //[SerializeField] private Vector2 dashDirection; 
    [Space(4)]

    [Header("Local Components")]
    private Rigidbody2D _rb;
    [Space(4)]

    [Header("Scripts")]
    private C_Health _healthComp;     //health component
    private C_Health _enemyHealthComp;
    Vector2 mousePoint;

    private void Awake()
    {
        _healthComp = GetComponent<C_Health>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        direction = 1;
        _canDamage = false;
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComp.isAlive())
        {
            if (canMove)
                PlayerMovement(_movementSpeed);

            DirectionCheck();
            Dash(_dashSpeed);
        }
    }

    private Vector2 AimDirection()
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
        //transform.Translate(move);
    }

    private void Dash(float x)
    {
        //dash direction = direction int
        //use switch on int to set velocity?
        x *= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentEnergy >= _dashEnergyCost)
                currentEnergy -= _dashEnergyCost;
            else
                currentEnergy = 0;

            //TODO - only dash when energy is equal or above dash cost
            StartCoroutine(DashBehaviour(5, 0.3f));
        }
    }

    IEnumerator DashBehaviour(float time, float speed)
    {
        float count = 0;
        //isDashing = true;
        canMove = false;
        _canDamage = true;

        while (count < time)
        {
            count += speed;
            _rb.AddForce(AimDirection() * _dashSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        _canDamage = false;
        canMove = true;
        //isDashing = false;
        print("end dash");
    }

    public void StopVelocity()
    {
        _rb.velocity = new Vector2(0, 0);
    }

    private int DirectionCheck()
    {
        if (_rb.velocity.y > 0 && _rb.velocity.x == 0)
        {
            direction = 1; //forward
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x == 0)
        {
            direction = 2; //back
        }
        else if (_rb.velocity.x > 0 && _rb.velocity.y == 0)
        {
            direction = 3; //right
        }
        else if (_rb.velocity.x < 0 && _rb.velocity.y == 0)
        {
            direction = 4; //left
        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x > 0)
        {
            direction = 5;   //forward right

        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x < 0)
        {
            direction = 6;  //forward left
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x > 0)
        {
            direction = 7;  //back right
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x < 0)
        {
            direction = 8;  //back left
        }

        //Debug.Log(_rb.velocity);
        return direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _canDamage && collision.GetComponentInParent<C_Health>() != null)
        {
            _enemyHealthComp = collision.GetComponentInParent<C_Health>();
            _enemyHealthComp.Damage(dashDamage);
        }
    }
}
