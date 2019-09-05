using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPool : MonoBehaviour
{
    [SerializeField] private float _healAmount;
    private HealthComponent _playerHealthComp;
    private bool _canHeal;

    private void Start()
    {
        _canHeal = false;
    }

    IEnumerator Healing()
    {
        while (_canHeal)
        {
            _playerHealthComp.Heal(_healAmount);
            print("Heal");
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<HealthComponent>() && collision.gameObject.layer == 8)
        {
            _playerHealthComp = collision.GetComponentInParent<HealthComponent>();
            _canHeal = true;
            StartCoroutine(Healing());
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<HealthComponent>() && collision.gameObject.layer == 8)
        {
            StopCoroutine(Healing());
            _canHeal = false;
        }
    
    }
}
