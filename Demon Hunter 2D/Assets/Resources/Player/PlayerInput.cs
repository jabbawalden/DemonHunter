using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;
    private HealthComponent healthComponent;
    private PlayerEnergy playerEnergy;
    private PlayerDash playerDash;
    private PlayerMeleeAttack playerMeleeAttack;
    private PlayerAbilities playerAbilities;
    private NPCManager npcManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private bool shoot;
    private bool dash;

    [SerializeField] private float _h, _v;
    public float h { get { return _h; } private set { _h = value; } }
    public float v { get { return _v; } private set { _v = value; } }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
        healthComponent = GetComponent<HealthComponent>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerDash = GetComponent<PlayerDash>();
        playerMeleeAttack = GetComponent<PlayerMeleeAttack>();
        playerAbilities = GetComponent<PlayerAbilities>();
        npcManager = FindObjectOfType<NPCManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthComponent.IsAlive())
        {
            if (!playerController.isNPCInteracting && !gameManager.IsPause)
            {
                if (playerController.canMove)
                    PlayerMoveInput();

                if (playerController.canAttack)
                {
                    PlayerBasicAttacks();
                    PlayerAbilitySelect();
                }

            }

            MenuInput();
            PlayerNPCInteract();
        }

    }

    private void FixedUpdate()
    {
        if (healthComponent.IsAlive())
        {
            if (!playerController.isNPCInteracting && !gameManager.IsPause)
            {
                if (shoot)
                {
                    shoot = false;
                    playerShoot.ShootAction();
                }

                if (dash)
                {
                    dash = false;
                    playerDash.Dash();
                }
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
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMeleeAttack.playerMeleeEnabled)
        {
            playerMeleeAttack.MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerShoot.playerShootEnabled)
        {
            shoot = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerEnergy.currentEnergy >= playerDash.dashEnergyCost && playerDash.playerDashEnabled)
        {
            dash = true;
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
            playerAbilities.SwitchSelectedAbility(1);

        if (Input.GetKeyDown(KeyCode.Alpha9))
            playerAbilities.SwitchSelectedAbility(2);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            playerAbilities.SwitchSelectedAbility(3);

        if (Input.GetKeyDown(KeyCode.Q))
            playerAbilities.specialAbilityActivate();
    }
    
    private void PlayerNPCInteract()
    {
        playerController.StopVelocity();

        //check bools then set npcmanager values
        if (Input.GetKeyDown(KeyCode.E) && !playerController.isNPCInteracting && playerController.npcSet)
        {
            playerController.selectedNPC(true);
            npcManager.SetSpeechCounters();
            npcManager.ActivateSpeech();
            StopInput();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && playerController.isNPCInteracting && playerController.npcSet)
        {
            playerController.selectedNPC(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && playerController.isNPCInteracting) 
        {
            if (npcManager.currentConvCheck)
            {
                //if true, use 1 liner dialogue instead
                print(npcManager.currentConvCheck);
                playerController.selectedNPC(false);
            }
            else if (npcManager.mainTalkCount <= npcManager.maxTalkCount)
            {
                npcManager.ActivateSpeech();
            }
            else if(npcManager.mainTalkCount > npcManager.maxTalkCount)
            {
                playerController.selectedNPC(false);

            }
        }
    }


}
