﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerEnergy _playerEnergy;
    private C_Health _playerHealthComp;
    private C_Health _enemyHealthComp;
    private UIManager _uiManager;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    [SerializeField] private PlayerCamera _playerCamera;

    [SerializeField] private float _dashEnergyCost;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDamage;
    [SerializeField] private float _dashHealAmount;
    [SerializeField] private BoxCollider2D playerBodyCollision;
    [SerializeField] private CircleCollider2D _circleCollider;

    private bool _canDashDamage;

    private void Awake()
    {
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerHealthComp = GetComponent<C_Health>();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealthComp.IsAlive())
            Dash();
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _dashEnergyCost)
        {
            _playerEnergy.RemoveEnergy(_dashEnergyCost);
            StartCoroutine(DashBehaviour(4.5f, 0.3f));
            _uiManager.UpdateEnergySlider();
            _uiManager.DamageEnergyBar();
            _playerCamera.CameraShake(0.15f, 0.08f);
        }
    }

    IEnumerator DashBehaviour(float time, float speed)
    {
        float count = 0;
        Vector2 currentAimDirection = _playerController.AimDirection();

        _playerController.canMove = false;
        _canDashDamage = true;
        _circleCollider.enabled = true;
        playerBodyCollision.enabled = false;

        while (count < time)
        {
            count += speed;
            _rb.velocity = currentAimDirection * _dashSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        _playerController.canMove = true;
        _canDashDamage = false;
        _circleCollider.enabled = false;
        playerBodyCollision.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && _canDashDamage && collision.GetComponentInParent<C_Health>() != null)
        {
            _enemyHealthComp = collision.GetComponentInParent<C_Health>();
            _playerHealthComp.Heal(_dashHealAmount);
            _enemyHealthComp.Damage(_dashDamage);
            _uiManager.UpdateHealthSlider();
            //_uiManager.DamageHealthBar();
        }
    }
}