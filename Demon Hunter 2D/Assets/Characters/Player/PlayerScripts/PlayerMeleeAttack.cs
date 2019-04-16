using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private C_Health _enemyHealthComp;
    private C_Health _playerHealthComp;
    private PlayerController _playerController;

    [Header("Variables")]
    [SerializeField] private float _recoveryAttackTime;
    [SerializeField] private float _initialAttackTime;
    [SerializeField] private float _attackWindUpTime;
    [SerializeField] private float _attackRate; 
    [SerializeField] private float _newTime;
    [SerializeField] private float _meleeDamage;
    [SerializeField] private bool _canMeleeDamage;
    private CircleCollider2D _circleCollider;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealthComp = GetComponent<C_Health>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _canMeleeDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealthComp.IsAlive())
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                MeleeAttack();
            }
        }

    }

    private void MeleeAttack()
    {
        if (_newTime <= Time.time)
        {
            print("Player melee attack");
            _canMeleeDamage = true;
            _playerController.canMove = false;
            _playerController.StopVelocity();
            _newTime = Time.time + _attackRate;
            StartCoroutine(MeleeBehaviour());
        }
    }

    IEnumerator MeleeBehaviour()
    {
        yield return new WaitForSeconds(_attackWindUpTime);
        //spawn melee attack collider
        //use instantiate instead of collier because 
        print("attack spawns");
        yield return new WaitForSeconds(_recoveryAttackTime);
        _playerController.canMove = true;
        _canMeleeDamage = false;       
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && _canMeleeDamage && collision.GetComponentInParent<C_Health>() != null)
        {
            _enemyHealthComp = collision.GetComponentInParent<C_Health>();
            _enemyHealthComp.Damage(_meleeDamage);
            print("hit enemy");
            //add a small knock-back effect and also stun effect that interrupts attacks
        }
    }
    */
}
