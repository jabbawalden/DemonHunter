﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Unfinished Script

    private PlayerController _playerController;
    private PlayerShoot _playerShoot;
    private C_Health _healthComponent;
    private PlayerEnergy _playerEnergy;
    private PlayerDash _playerDash;
    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerAbilities _playerAbilities;

    public float h {get; private set;}
    public float v {get; private set;}

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
        _healthComponent = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerDash = GetComponent<PlayerDash>();
        _playerMeleeAttack = GetComponent<PlayerMeleeAttack>();
        _playerAbilities = GetComponent<PlayerAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.IsAlive())
        {
            if(_playerController.canMove)
                PlayerMoveInput();
            PlayerBasicAttacks();
            PlayerAbilitySelect();
        }
    }

    private void PlayerMoveInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    private void PlayerBasicAttacks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _playerMeleeAttack.playerMeleeEnabled)
        {
            _playerMeleeAttack.MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _playerShoot.playerShootEnabled)
        {
            _playerShoot.ShootAction();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _playerDash.dashEnergyCost && _playerDash.playerDashEnabled)
        {
            _playerDash.Dash();
        }
    }

    private void PlayerAbilitySelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
            _playerAbilities.SwitchSelectedAbility(1);

        if (Input.GetKeyDown(KeyCode.Alpha9))
            _playerAbilities.SwitchSelectedAbility(2);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            _playerAbilities.SwitchSelectedAbility(3);

        if (Input.GetKeyDown(KeyCode.Q))
            _playerAbilities.upgradeActivate();
    }
    



}
