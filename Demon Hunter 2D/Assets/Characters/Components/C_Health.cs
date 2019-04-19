using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Health : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _maxHealth;
    public float _currentHealth;
    public bool isPlayerComponent;  

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (damage >= _currentHealth)
            _currentHealth = 0;
        else
        {
            _currentHealth -= damage;
        }

    }

    public void Heal(float heal)
    {
        if (_currentHealth < _maxHealth)
            _currentHealth += heal;
        
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        print("heal player");
    }

    public float GetHealthPercent()
    {
        float percent = _currentHealth / _maxHealth;
        return percent;
    }

    public bool IsAlive()
    {
        if (_currentHealth <= 0)
            return false;
        else
            return true;
    }
   
}
