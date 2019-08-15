using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeStrike : MonoBehaviour
{
    public float damage;
    CircleCollider2D circleCollider;
    [SerializeField] private GameObject windUp, explosion;
    [SerializeField] private float attackTime;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(AttackAnimTime());
    }

    IEnumerator AttackAnimTime()
    {
        yield return new WaitForSeconds(attackTime);
        circleCollider.enabled = true;
        windUp.SetActive(false);
        explosion.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponentInParent<C_Health>().Damage(damage);
            circleCollider.enabled = false;
        }
    }
}
