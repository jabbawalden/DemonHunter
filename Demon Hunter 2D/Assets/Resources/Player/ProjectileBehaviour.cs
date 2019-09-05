using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float damage;
    private HealthComponent _healthComponent;
    private PlayerEnergy _playerEnergy;
    private PlayerController _playerController;
    [System.NonSerialized] public int hitCount;
    [SerializeField] private int _maxHitCount;
    //change to property
    public int targetLayer;
    public bool isPlayerProj;
    public Vector2 currentDirection;
    public float currentSpeed;

    //[SerializeField] private GameObject explosion;
    //[SerializeField] private GameObject explosion2;
    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        if (isPlayerProj)
            _playerController = FindObjectOfType<PlayerController>();
    }

    private void HitCounter()
    {
        hitCount++;
        if (hitCount >= _maxHitCount)
            Destroy(gameObject);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject explosionRef = Resources.Load<GameObject>(AssetPaths.pref_explosion);

        //dealing damage code
        if (collision.GetComponentInParent<EnemyController>() && !collision.GetComponentInParent<EnemyController>().canRecieveDamage)
        {
            //if enemy, and has enemy controller, but cannot receive damage
            print("hit shield");
            if (isPlayerProj)
                Destroy(gameObject);
        }
        else if (collision.gameObject.layer == targetLayer && collision.GetComponentInParent<HealthComponent>() != null /*&& collision.gameObject.GetComponent<EnemyController>() && collision.gameObject.GetComponent<EnemyController>().canRecieveDamage*/)
        { 
            //if enemy and health component exists
            print("Hit player");
            _healthComponent = collision.GetComponentInParent<HealthComponent>();

            if (_playerController && isPlayerProj)
                _healthComponent.Damage(damage * _playerController.DamageMultiplier);
            else
                _healthComponent.Damage(damage);

            if (_healthComponent.IsAlive())
                Destroy(gameObject);
            else
                HitCounter();
        }
        else if (collision.gameObject.layer == 13 && !isPlayerProj)
        {
            //if player melee attack to deflect
            //Destroy(gameObject);
            print("projectile reverted");
        }
        else if (collision.gameObject.layer == 14 && !isPlayerProj)
        {
            //if we are enemy projectile and run into playerproj layer
            //explode
            if (explosionRef)
                Instantiate(explosionRef, transform.position, Quaternion.identity);

            Destroy(gameObject);
            print("explode");
        }
        else if (gameObject.layer == 15 && collision.gameObject.layer == 15 && isPlayerProj)
        {
            //if we are enemyProj, and the other is enemyProj but WE have playerProj true, then explode and destroy
            if (explosionRef)
                Instantiate(explosionRef, transform.position, Quaternion.identity);

            Destroy(gameObject);
            print("explode");
        }
        else if (gameObject.layer == 15 && collision.gameObject.layer == 15 && !isPlayerProj && collision.GetComponent<ProjectileBehaviour>().isPlayerProj)
        {
            //if we are enmemyproj and they are enemyproj and our isPlayerProj is false but their isPlayerProj is true, then simply destroy.
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 15 && isPlayerProj)
        {
            //if we are player proj and run into enemy proj
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 12)
        {
            //if environment blocker, destroy object
            Destroy(gameObject);
        }
    }

    public void ConvertPlayerSettings()
    {
        if (isPlayerProj)
            _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
