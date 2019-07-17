using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float damage;
    private C_Health _healthComponent;
    private PlayerEnergy _playerEnergy;
    [System.NonSerialized] public int hitCount;
    [SerializeField] private int _maxHitCount;
    public int targetLayer;
    public bool isPlayerProj;
    [SerializeField] private GameObject explosion;
    public Vector2 currentDirection;
    public float currentSpeed;

    private void Awake()
    {
        _healthComponent = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
    }

    private void HitCounter()
    {
        hitCount++;
        if (hitCount >= _maxHitCount)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //dealing damage code
        if (collision.GetComponentInParent<EnemyController>() && !collision.GetComponentInParent<EnemyController>().canRecieveDamage)
        {
            //if enemy, and has enemy controller, but cannot recieve damage
            print("hit shield");
            if (isPlayerProj)
                Destroy(gameObject);
        }
        else if (collision.gameObject.layer == targetLayer && collision.GetComponentInParent<C_Health>() != null /*&& collision.gameObject.GetComponent<EnemyController>() && collision.gameObject.GetComponent<EnemyController>().canRecieveDamage*/)
        { 
            //if enemy and health component exists
            print("Hit player");
            _healthComponent = collision.GetComponentInParent<C_Health>();
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
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if (gameObject.layer == 15 && collision.gameObject.layer == 15 && isPlayerProj)
        {
            //if we are enemyProj, and the other is enemyProj and WE have playerProj true, then explode and destroy
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
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
            //if enemy blocker, destroy object
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
