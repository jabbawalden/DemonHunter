using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private GameManager gameManager; 
    private HealthComponent enemyHealthComp;
    private HealthComponent playerHealthComp;
    private PlayerController playerController;
    private PlayerEnergy playerEnergy;
    private UIManager uiManager;

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
    private CircleCollider2D circleCollider;

    //[SerializeField] private GameObject _meleeAttackObj;
    public bool playerMeleeEnabled;
    public bool meleeIconLit;
    //[System.NonSerialized] public bool haveAttacked;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = GetComponent<PlayerController>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealthComp = GetComponent<HealthComponent>();
        circleCollider = GetComponent<CircleCollider2D>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        _canMeleeDamage = false;
    }

    void Update()
    {

        if (playerEnergy.currentEnergy >= _energyCost)
            meleeIconLit = true;
        else
            meleeIconLit = false;
    }

    public void LoadData()
    {
        playerMeleeEnabled = JsonDataManager.gameData.meleeEnabled;
    }

    public void MeleeAttack()
    {
        gameManager.TutorialCheckMelee();

        if (_newTime <= Time.time && playerEnergy.currentEnergy >= _energyCost)
        {
            _canMeleeDamage = true;
            //_playerController.canMove = false;
            //_playerController.StopVelocity();
            _newTime = Time.time + _attackRate;
            StartCoroutine(MeleeBehaviour());
            playerEnergy.RemoveEnergy(_energyCost);
            uiManager.UpdateEnergySlider();
            uiManager.DamageEnergyBar();
        }
    }

    IEnumerator MeleeBehaviour()
    {
        Vector2 spawnAttackPosition = new Vector2(transform.position.x, transform.position.y) + playerController.AimDirection();
        playerController.currentMovementSpeed = playerController.meleeMovementSpeed;
        playerController.canAttack = false;

        yield return new WaitForSeconds(_attackWindUpTime);
        //spawn melee attack collider
        //use instantiate instead of collier because 

        GameObject meleeStrike = Resources.Load<GameObject>(AssetPaths.pref_playerMeleeStrike);

        GameObject obj = Instantiate(meleeStrike, spawnAttackPosition, Quaternion.identity);

        if (obj.GetComponent<PlayerMeleeStrike>())
        {
            obj.GetComponent<PlayerMeleeStrike>().attackTime = _attackTime;
            obj.GetComponent<PlayerMeleeStrike>().damage = _meleeDamage; 
        }

        yield return new WaitForSeconds(_recoveryAttackTime);
        //_playerController.canMove = true;
        _canMeleeDamage = false;
        playerController.currentMovementSpeed = playerController.defaultMovementSpeed;
        playerController.canAttack = true;
    }

}
