using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float _healingGraceCost;
    [SerializeField] private float _bombPlantCost;
    [SerializeField] private float _EnergyMaximizerCost;

    public delegate void UpgradeAbilityActivate();
    public UpgradeAbilityActivate upgradeActivate;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        upgradeActivate += HealingGraceAbility;
    }

    public void HealingGraceAbility()
    {
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
            upgradeActivate = HealingGraceAbility;
        else if (index == 2)
            upgradeActivate = BombPlantAbility;
        else if (index == 3)
            upgradeActivate = EnergyMaximizerAbility;
    }

}
