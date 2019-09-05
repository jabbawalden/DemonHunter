using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private GameManager _gameManager; 
    private HealthComponent _enemyHealthComp;
    private HealthComponent _playerHealthComp;
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

    //[SerializeField] private GameObject _meleeAttackObj;
    public bool playerMeleeEnabled;
    public bool meleeIconLit;
    //[System.NonSerialized] public bool haveAttacked;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerController = GetComponent<PlayerController>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerHealthComp = GetComponent<HealthComponent>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        _canMeleeDamage = false;
    }

    void Update()
    {

        if (_playerEnergy.currentEnergy >= _energyCost)
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
        _gameManager.TutorialCheckMelee();

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
        Vector2 spawnAttackPosition = new Vector2(transform.position.x, transform.position.y) + _playerController.AimDirection();
        _playerController.currentMovementSpeed = _playerController.meleeMovementSpeed;

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
        _playerController.currentMovementSpeed = _playerController.defaultMovementSpeed;
    }

}
