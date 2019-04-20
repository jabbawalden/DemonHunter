using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeStrike : MonoBehaviour
{
    [System.NonSerialized] public float damage;
    [System.NonSerialized] public float attackTime;
    [System.NonSerialized] public int hitCount;
    [SerializeField] private int _maxHitCount;
    private EnemyController _enemyController;

    private void Start()
    {
        StartCoroutine(AttackTimer(attackTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.GetComponentInParent<C_Health>() != null)
        {
            collision.GetComponentInParent<C_Health>().Damage(damage);
            if (collision.GetComponentInParent<EnemyController>() != null)
            {
                _enemyController = collision.GetComponentInParent<EnemyController>();
            }
            HitCounter();


        }

    }

    private void HitCounter()
    {
        if (hitCount == _maxHitCount)
        {
            Destroy(gameObject);
        }
        else
        {
            hitCount++;
            _enemyController.knockBack = true;
        }
           
    }

    IEnumerator AttackTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
