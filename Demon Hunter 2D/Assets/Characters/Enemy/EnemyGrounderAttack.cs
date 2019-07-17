using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrounderAttack : MonoBehaviour
{
    private EnemyController _enemyController;
    [SerializeField] private GameObject crystalAttack;
    private CrystalAttackBehaviour _crystalAttackBehaviour;
    [SerializeField] private float _fireRate;
    private float _newTime;

    [SerializeField] private float _spawnRate;
    [SerializeField] private int _maxAmount;
    private float _spawnTime;
    private int _currentAmount;
    private bool _groundAttack;

    [SerializeField] private float _damage;

    private Vector2 _SpawnLoc;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (_enemyController.enemyState == EnemyState.engaged)
        {
            AttackBehaviour();
        }
    }

    private void AttackBehaviour()
    {
        if (_newTime <= Time.time)
        {
            float rFRate = Random.Range(_fireRate, _fireRate * 1.5f);
            _newTime = Time.time + rFRate;
            print("grounder attack");
            _groundAttack = true;
            _currentAmount = 0;
            _SpawnLoc = new Vector2(transform.position.x + AttackDirection().x * 1.5f, transform.position.y + AttackDirection().y * 1.5f) ;
            //attack if enemy within range
        }

        if (_groundAttack)
        {
            GrounderAttack();
        }
    }

    private Vector2 AttackDirection()
    {
        Vector2 direction = _enemyController.playerRef.position - transform.position;
        direction = direction.normalized;
        return direction;
    }

    public void GrounderAttack()
    {
        if (_spawnTime <= Time.time)
        {
            _spawnTime = Time.time + _spawnRate;
            if (_currentAmount < _maxAmount)
            {
                _currentAmount++;
                _SpawnLoc += AttackDirection() * 1.5f;
                SpawnCrystal(_SpawnLoc);
            }
            else
            {
                _groundAttack = false;
            }
            //attack if enemy within range
        }
    }

    private void SpawnCrystal(Vector2 location)
    {
        GameObject obj = Instantiate(crystalAttack, location, Quaternion.identity);
        obj.GetComponent<CrystalAttackBehaviour>().damage = _damage;
    }

}
