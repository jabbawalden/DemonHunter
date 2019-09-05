using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float _healingGraceCost;
    [SerializeField] private float _bombPlantCost;
    [SerializeField] private float _EnergyMaximizerCost;
    private HealthComponent _healthComp;

    public delegate void UpgradeAbilityActivate();
    public UpgradeAbilityActivate specialAbilityActivate;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _healthComp = FindObjectOfType<HealthComponent>();
    }

    private void Start()
    {
        specialAbilityActivate += HealingGraceAbility;
    }

    public void HealingGraceAbility()
    {
        float healAmount = _healthComp.maxHealth / 2;
        _healthComp.Heal(healAmount);
        print("Healing grace");
    }

    public void BombPlantAbility()
    {
        print("Bomb plant");
    }

    public void EnergyMaximizerAbility()
    {
        print("Energy maximizer");
    }

    public void SwitchSelectedAbility(int index)
    {
        if (index == 1)
            specialAbilityActivate = HealingGraceAbility;
        else if (index == 2)
            specialAbilityActivate = BombPlantAbility;
        else if (index == 3)
            specialAbilityActivate = EnergyMaximizerAbility;
    }

}
