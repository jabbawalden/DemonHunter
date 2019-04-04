using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public float maxEnergy;
    public float currentEnergy;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    private void Update()
    {

    }

    public void AddEnergy(int energyToAdd)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyToAdd;
            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;
        }
    }

    public float GetEnergyPercent()
    {
        float percent = currentEnergy / maxEnergy;
        return percent;
    }
}
