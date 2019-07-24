using System.Collections;
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

    public float h {get; private set;}
    public float v {get; private set;}

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
        _healthComponent = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerDash = GetComponent<PlayerDash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.IsAlive())
        {
            if(_playerController.canMove)
                PlayerMoveInput();
            PlayerDashInput();
            PlayerShootInput();
        }
    }

    private void PlayerMoveInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    private void PlayerDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _playerDash.dashEnergyCost && _playerDash.playerDashEnabled)
        {
            _playerDash.Dash();
        }
    }
    
    private void PlayerShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _playerShoot.playerShootEnabled)
        {
            _playerShoot.ShootAction();
        }
    }
}
