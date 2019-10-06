using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    private HealthComponent healthComponent;
    private UIManager uiManager;
    private PlayerEnergy playerEnergy;

    [Header("Variables")]
    [SerializeField] private float _projSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootAnimationTime;
    [SerializeField] private float _projDamage;
    [SerializeField] private float _healthCost;
    [SerializeField] private float _energyCost;
    private float _newTime;
    //public GameObject projectile;
    [SerializeField] private Transform _shootOrigin;
    [Space(4)]

    //[Header("Scripts")]
    private ProjectileBehaviour projBehaviour;
    public bool playerShootEnabled;
    public bool shootIconLit;
    //[System.NonSerialized] public bool haveShot;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = GetComponent<PlayerController>();
        healthComponent = GetComponent<HealthComponent>();
        playerEnergy = GetComponent<PlayerEnergy>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEnergy.currentEnergy >= _energyCost)
            shootIconLit = true;
        else
            shootIconLit = false;
    }

    public void LoadData()
    {
        playerShootEnabled = JsonDataManager.gameData.shootEnabled;
    }

    public void ShootAction()
    {
        if (Time.time >= _newTime && playerEnergy.currentEnergy >= _energyCost)
        {
            ShootAbility();
            gameManager.TutorialCheckShoot();
        }
    }

    private void ShootAbility()
    {
        playerController.currentMovementSpeed = playerController.shootingMovementSpeed;
        playerController.StopVelocity();

        _newTime = Time.time + _fireRate;
        StartCoroutine(ShootBehaviour());

        GameObject playerProjectile = Resources.Load<GameObject>(AssetPaths.pref_playerProjectile);

        GameObject proj = Instantiate(playerProjectile, _shootOrigin.position, playerProjectile.transform.rotation);
        Vector2 direction = playerController.AimDirection(); //normalized not actually needed

        proj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        proj.GetComponent<Rigidbody2D>().velocity = direction * _projSpeed * Time.deltaTime;

        //set damage
        projBehaviour = proj.GetComponent<ProjectileBehaviour>();
        projBehaviour.damage = _projDamage;

        //_healthComponent.Damage(_healthCost);
        //_uiManager.UpdateHealthSlider();
        playerEnergy.RemoveEnergy(_energyCost);
        uiManager.UpdateEnergySlider();
        uiManager.DamageEnergyBar();
    }

    IEnumerator ShootBehaviour()
    {
        playerController.canAttack = false;
        yield return new WaitForSeconds(_shootAnimationTime);
        playerController.currentMovementSpeed = playerController.defaultMovementSpeed;
        playerController.canAttack = true;
    }
}
