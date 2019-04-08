using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackRate;
    [SerializeField] private float _newTime;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _preAttackTime; 
    private EnemyController _enemyController;
    private C_Health _playerHealthComponent;
    private UIManager _uiManager;

    public EnemyState stateCheck;

    private void Awake()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController.playerRef != null)
        {
            if (_enemyController.PlayerDistance() <= _attackRange)
                AttackPlayer();
        }
      
    }

    private void AttackPlayer()
    {
        if (_newTime <= Time.time)
        {
            _newTime = Time.time + _attackRate;
            StartCoroutine(MeleeAttackBehaviour());
        }
    }

    IEnumerator MeleeAttackBehaviour()
    {
        stateCheck = _enemyController.enemyState;
        _enemyController.enemyState = EnemyState.attacking;
        yield return new WaitForSeconds(_preAttackTime);
        if (_enemyController.PlayerDistance() <= _attackRange)
        {
            _playerHealthComponent = _enemyController.playerRef.GetComponentInParent<C_Health>();
            _playerHealthComponent.Damage(_damage);
            _uiManager.UpdateHealthSlider();
            print("Hit successful");
        }
        else
        {
            print("Hit missed");
        } 
        yield return new WaitForSeconds(_attackTime);

        _enemyController.enemyState = stateCheck;
    }

}
