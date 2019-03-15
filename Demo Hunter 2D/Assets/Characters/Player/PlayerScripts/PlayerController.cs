using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed;    //speed 
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int direction;
    //[SerializeField] private Vector2 dashDirection; 
    [Space(4)]

    [Header("Local Components")]
    private Rigidbody2D _rb;

    [Header("Scripts")]
    private C_Health _healthComp;     //health component


    private void Awake()
    {
        _healthComp = GetComponent<C_Health>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComp.isAlive())
        {
            PlayerMovement(_movementSpeed);
            DirectionCheck();
            Dash(_dashSpeed);
        }

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
            print("dash");

            StartCoroutine(DashBehaviour(5, 0.5f));

        }
    }

    IEnumerator DashBehaviour(float time, float speed)
    {
        float count = 0;

        while (count < time)
        {
            count += speed;
            _rb.AddForce(new Vector2(0, _dashSpeed));
            yield return new WaitForSeconds(0.01f);
        }
        print("end dash");

    }

    private int DirectionCheck()
    {
        if (_rb.velocity.y > 0 && _rb.velocity.x == 0)
        {
            direction = 1; //forward
            //print("Forward");
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x == 0)
        {
            direction = 2; //back
            //print("Back");
        }
        else if (_rb.velocity.x > 0 && _rb.velocity.y == 0)
        {
            direction = 3; //right
            //print("Right");
        }
        else if (_rb.velocity.x < 0 && _rb.velocity.y == 0)
        {
            direction = 4; //left
            //print("Left");
        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x > 0)
        {
            direction = 5;
            //print("Forward right"); //forward right
        }
        else if (_rb.velocity.y > 0 && _rb.velocity.x < 0)
        {
            direction = 6;
            //print("Forward left"); //forward left
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x > 0)
        {
            direction = 7;
            //print("Back right"); //back right
        }
        else if (_rb.velocity.y < 0 && _rb.velocity.x < 0)
        {
            direction = 8;
            //print("Back left"); //back left
        }

        //Debug.Log(_rb.velocity);
        return direction;
    }


}
