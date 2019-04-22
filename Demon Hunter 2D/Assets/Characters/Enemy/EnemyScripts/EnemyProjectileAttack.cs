using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
{
    [Header("Scripts")]
    private EnemyController _enemyController;
    private ProjectileBehaviour projBehaviour;
    private C_Health _healthComp;
    [Space(4)]

    [Header("Variables")]
    [SerializeField] private float _fireRate;
    [SerializeField] private float _projDamage;
    [SerializeField] private float _projSpeed;
    [SerializeField] private float _shootRange;
    [SerializeField] private float _shootAnimationTime;
    private float _newTime;
    [Space(4)]

    [Header("Object References")] 
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootOrigin;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _healthComp = GetComponent<C_Health>();
    }


    // Update is called once per frame
    void Update()
    {
        if (_healthComp.IsAlive())
        {
            if (_enemyController.playerRef != null && _enemyController.TargetDistance() <= _shootRange)
            {
                ShootProjectile();
            }
        }
    }

    private void ShootProjectile()
    {
        if (_newTime <= Time.time)
        {
            _newTime = Time.time + _fireRate;
            GameObject proj = Instantiate(_projectile, _shootOrigin.position, _projectile.transform.rotation);
            Vector2 direction = new Vector2();
            if (_enemyController.playerRef != null)
                direction = transform.position - _enemyController.playerRef.position;
            direction = direction.normalized;

            proj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            projBehaviour = proj.GetComponent<ProjectileBehaviour>();
            proj.GetComponent<Rigidbody2D>().velocity = -direction * _projSpeed * Time.deltaTime;
            projBehaviour.currentDirection = -direction;
            projBehaviour.currentSpeed = _projSpeed;
            projBehaviour.damage = _projDamage;

            StartCoroutine(ShootBehaviour());
        }
    }

    IEnumerator ShootBehaviour()
    {
        EnemyState stateCheck;
        stateCheck = _enemyController.enemyState;
        _enemyController.enemyState = EnemyState.attacking;
        yield return new WaitForSeconds(_shootAnimationTime);
        _enemyController.enemyState = stateCheck;
    }
}
