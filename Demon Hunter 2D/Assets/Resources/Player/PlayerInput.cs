using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Unfinished Script

    private PlayerController _playerController;
    private PlayerShoot _playerShoot;
    private HealthComponent _healthComponent;
    private PlayerEnergy _playerEnergy;
    private PlayerDash _playerDash;
    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerAbilities _playerAbilities;
    private NPCManager _npcManager;
    private UIManager _uiManager;
    private GameManager gameManager;
    private bool shoot;
    private bool dash;

    [SerializeField] private float _h, _v;
    public float h { get { return _h; } private set { _h = value; } }
    public float v { get { return _v; } private set { _v = value; } }

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
        _healthComponent = GetComponent<HealthComponent>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerDash = GetComponent<PlayerDash>();
        _playerMeleeAttack = GetComponent<PlayerMeleeAttack>();
        _playerAbilities = GetComponent<PlayerAbilities>();
        _npcManager = FindObjectOfType<NPCManager>();
        _uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.IsAlive())
        {
            if (!_playerController.isNPCInteracting && !gameManager.IsPause)
            {
                if (_playerController.canMove)
                    PlayerMoveInput();

                PlayerBasicAttacks();
                PlayerAbilitySelect();
            }

            MenuInput();
            PlayerNPCInteract();
        }

    }

    private void FixedUpdate()
    {
        if (_healthComponent.IsAlive())
        {
            if (!_playerController.isNPCInteracting && !gameManager.IsPause)
            {
                if (shoot)
                {
                    shoot = false;
                    _playerShoot.ShootAction();
                }

                //if (dash)
                //{
                //    dash = false;
                //    _playerDash.Dash();
                //}
            }
        }
    }

    private void PlayerMoveInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    private void StopInput()
    {
        h = 0;
        v = 0;
    }

    private void PlayerBasicAttacks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _playerMeleeAttack.playerMeleeEnabled)
        {
            _playerMeleeAttack.MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _playerShoot.playerShootEnabled)
        {
            shoot = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _playerDash.dashEnergyCost && _playerDash.playerDashEnabled)
        {
            _playerDash.Dash();
        }
    }

    private void MenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseMenuActivation();
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
            _playerAbilities.specialAbilityActivate();
    }
    
    private void PlayerNPCInteract()
    {
        _playerController.StopVelocity();

        //check bools then set npcmanager values
        if (Input.GetKeyDown(KeyCode.E) && !_playerController.isNPCInteracting && _playerController.npcSet)
        {
            _playerController.selectedNPC(true);
            _npcManager.SetSpeechCounters();
            _npcManager.ActivateSpeech();
            StopInput();
        }
        else if (Input.GetKeyDown(KeyCode.E) && _playerController.isNPCInteracting && _playerController.npcSet)
        {
            _playerController.selectedNPC(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerController.isNPCInteracting) 
        {
            if (_npcManager.currentConvCheck)
            {
                //if true, use 1 liner dialogue instead
                print(_npcManager.currentConvCheck);
                _playerController.selectedNPC(false);
            }
            else if (_npcManager.mainTalkCount <= _npcManager.maxTalkCount)
            {
                _npcManager.ActivateSpeech();
            }
            else if(_npcManager.mainTalkCount > _npcManager.maxTalkCount)
            {
                _playerController.selectedNPC(false);

            }
        }
    }


}
