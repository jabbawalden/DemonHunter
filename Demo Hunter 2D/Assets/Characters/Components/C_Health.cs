using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Health : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (damage >= _currentHealth)
            _currentHealth = 0;
        else
            _currentHealth -= damage;

    }

    public bool isAlive()
    {
        if (_currentHealth <= 0)
            return false;
        else
            return true;
    }
   
}
