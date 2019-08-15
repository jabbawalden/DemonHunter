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
    [SerializeField] private float attackSpawnDistance;
    private EnemyController _enemyController;
    private C_Health _playerHealthComponent;
    private C_Health _ourHealthComp;
    private UIManager _uiManager;
    private EnemyMeleeStrike _enemyMeleeStrike;
    [SerializeField] private GameObject meeleStrike;
    public EnemyState stateCheck;

    private void Awake()
    {
        _ourHealthComp = GetComponent<C_Health>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController.playerRef != null)
        {
            if (_enemyController.TargetDistance() <= _attackRange && _enemyController.haveDirectlyEngaged && _ourHealthComp.IsAlive())
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

    private Vector2 AttackDirection()
    {
        Vector2 direction = _enemyController.playerRef.position - transform.position;
        direction = direction.normalized;
        return direction; 
    }

    private void MeleeAttack()
    {
        //Instantiate attack
        Vector2 attackLoc = new Vector2 (transform.position.x + AttackDirection().x, transform.position.y + AttackDirection().y);

        GameObject obj = Instantiate(meeleStrike, attackLoc, Quaternion.identity);
        obj.GetComponent<EnemyMeleeStrike>().damage = _damage;
    }

    private void MeleeWindUp()
    {

    }

    IEnumerator MeleeAttackBehaviour()
    {
        stateCheck = _enemyController.enemyState;
        _enemyController.enemyState = EnemyState.attacking;
        yield return new WaitForSeconds(_preAttackTime);
        MeleeAttack();
        /*
        if (_enemyController.TargetDistance() <= _attackRange)
        {
            _playerHealthComponent = _enemyController.playerRef.GetComponentInParent<C_Health>();
            _playerHealthComponent.Damage(_damage);
            _uiManager.UpdateHealthSlider();
            _uiManager.DamageHealthBar();

        }
        */
        yield return new WaitForSeconds(_attackTime);
        _enemyController.enemyState = EnemyState.engaged;
    }

}
