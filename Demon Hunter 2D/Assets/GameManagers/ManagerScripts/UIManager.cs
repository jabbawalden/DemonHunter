using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _energySlider;

    private C_Health _playerHealthComponenent;
    private PlayerEnergy _playerEnergy;

    private void Awake()
    {
        _playerHealthComponenent = GameObject.Find("PlayerController").GetComponent<C_Health>();
        _playerEnergy = GameObject.Find("PlayerController").GetComponent<PlayerEnergy>();
    }

    public void UpdateHealthSlider()
    {
        _healthSlider.value = _playerHealthComponenent.GetHealthPercent();
    }

    public void UpdateEnergySlider()
    {
        _energySlider.value = _playerEnergy.GetEnergyPercent();
    }

}
