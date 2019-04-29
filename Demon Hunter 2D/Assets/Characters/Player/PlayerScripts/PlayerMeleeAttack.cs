﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private C_Health _enemyHealthComp;
    private C_Health _playerHealthComp;
    private PlayerController _playerController;
    private PlayerEnergy _playerEnergy;
    private UIManager _uiManager;

    [Header("Variables")]
    [SerializeField] private float _recoveryAttackTime;
    [SerializeField] private float _initialAttackTime;
    [SerializeField] private float _attackWindUpTime;
    [SerializeField] private float _energyCost;
    [SerializeField] private float _attackRate; 
    [SerializeField] private float _newTime;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _meleeDamage;
    [SerializeField] private float _meleeSpawnRange; 
    [SerializeField] private bool _canMeleeDamage;
    private CircleCollider2D _circleCollider;

    [SerializeField] private GameObject _meleeAttackObj;
    public bool playerMeleeEnabled;
    public bool meleeIconLit;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerHealthComp = GetComponent<C_Health>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Start()
    {
        _canMeleeDamage = false;
    }

    void Update()
    {
        if (_playerHealthComp.IsAlive())
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && playerMeleeEnabled)
            {
                MeleeAttack();
            }
        }

        if (_playerEnergy.currentEnergy >= _energyCost)
            meleeIconLit = true;
        else
            meleeIconLit = false;
    }

    private void MeleeAttack()
    {
        if (_newTime <= Time.time && _playerEnergy.currentEnergy >= _energyCost)
        {
            _canMeleeDamage = true;
            //_playerController.canMove = false;
            //_playerController.StopVelocity();
            _newTime = Time.time + _attackRate;
            StartCoroutine(MeleeBehaviour());
            _playerEnergy.RemoveEnergy(_energyCost);
            _uiManager.UpdateEnergySlider();
            _uiManager.DamageEnergyBar();
        }
    }

    IEnumerator MeleeBehaviour()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y) + _playerController.AimDirection();
        yield return new WaitForSeconds(_attackWindUpTime);
        //spawn melee attack collider
        //use instantiate instead of collier because 
        GameObject obj = Instantiate(_meleeAttackObj, position, Quaternion.identity);

        if (obj.GetComponent<PlayerMeleeStrike>() != null)
        {
            obj.GetComponent<PlayerMeleeStrike>().attackTime = _attackTime;
            obj.GetComponent<PlayerMeleeStrike>().damage = _meleeDamage; 
        }

        yield return new WaitForSeconds(_recoveryAttackTime);
        //_playerController.canMove = true;
        _canMeleeDamage = false;       
    }

}
