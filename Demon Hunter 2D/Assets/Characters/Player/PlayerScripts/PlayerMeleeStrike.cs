using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeStrike : MonoBehaviour
{
    [System.NonSerialized] public float damage;
    [System.NonSerialized] public float attackTime;
    [System.NonSerialized] public int hitCount;
    [SerializeField] private int _maxHitCount;

    private void Start()
    {
        StartCoroutine(AttackTimer(attackTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.GetComponentInParent<C_Health>() != null)
        {
            print(collision.name);
            collision.GetComponentInParent<C_Health>().Damage(damage);
            HitCounter();
        }

    }

    private void HitCounter()
    {
        if (hitCount == _maxHitCount)
            Destroy(gameObject);
        else
            hitCount++;
    }

    IEnumerator AttackTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
