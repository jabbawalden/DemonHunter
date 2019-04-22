using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Health : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _maxHealth;
    public float _currentHealth;
    public bool isPlayerComponent;
    [Space(4)]


    [Header("Scripts")]
    private UIManager _uiManager;
    [SerializeField] private PlayerCamera playerCam;

    private void Awake()
    {
        if (isPlayerComponent)
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

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

        if (_uiManager)
            _uiManager.UpdateHealthSlider();

        if (isPlayerComponent)
            playerCam.CameraShake();

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
