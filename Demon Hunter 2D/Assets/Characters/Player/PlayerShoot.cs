﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerController _playerController;
    private C_Health _healthComponent;
    private UIManager _uiManager;
    private PlayerEnergy _playerEnergy;

    [Header("Variables")]
    [SerializeField] private float _projSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootAnimationTime;
    [SerializeField] private float _projDamage;
    [SerializeField] private float _healthCost;
    [SerializeField] private float _energyCost;
    private float _newTime;
    public GameObject projectile;
    [SerializeField] private Transform _shootOrigin;
    [Space(4)]

    //[Header("Scripts")]
    private ProjectileBehaviour projBehaviour;
    public bool playerShootEnabled;
    public bool shootIconLit;
    //[System.NonSerialized] public bool haveShot;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerController = GetComponent<PlayerController>();
        _healthComponent = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.IsAlive())
        {
            if (Input.GetKey(KeyCode.Mouse0) && playerShootEnabled)
            {
                if (Time.time >= _newTime && _playerEnergy.currentEnergy >= _energyCost)
                {
                    ShootAbility();
                    _gameManager.TutorialCheckShoot();
                }     
            }
        }

        if (_playerEnergy.currentEnergy >= _energyCost)
            shootIconLit = true;
        else
            shootIconLit = false;
    }

    public void LoadData()
    {
        playerShootEnabled = JsonDataManager.gameData.shootEnabled;
    }

    private void ShootAbility()
    {
        _playerController._currentMovementSpeed = _playerController._shootingMovementSpeed;
        _playerController.StopVelocity();

        _newTime = Time.time + _fireRate;
        StartCoroutine(ShootBehaviour());

        GameObject proj = Instantiate(projectile, _shootOrigin.position, projectile.transform.rotation);
        Vector2 direction = _playerController.AimDirection(); //normalized not actually needed

        proj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        proj.GetComponent<Rigidbody2D>().velocity = direction * _projSpeed * Time.deltaTime;

        //set damage
        projBehaviour = proj.GetComponent<ProjectileBehaviour>();
        projBehaviour.damage = _projDamage;

        //_healthComponent.Damage(_healthCost);
        //_uiManager.UpdateHealthSlider();
        _playerEnergy.RemoveEnergy(_energyCost);
        _uiManager.UpdateEnergySlider();
        _uiManager.DamageEnergyBar();
    }

    IEnumerator ShootBehaviour()
    {
        yield return new WaitForSeconds(_shootAnimationTime);
        _playerController._currentMovementSpeed = _playerController._defaultMovementSpeed;
    }
}
