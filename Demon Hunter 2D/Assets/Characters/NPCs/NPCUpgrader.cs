using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUpgrader : NPCInteraction
{
    // Start is called before the first frame update
    void Start()
    {
        setUI = UgradeOptions;
    }

    public void UgradeOptions(bool convOn)
    {
        if (convOn)
        {
            npcManager.maxTalkCount = maxTalkCount;
            uiManager.TurnOnUpgradeNPC(convOn);
            npcManager.SetUpgradeSpeech();
        }
        else
        {
            uiManager.TurnOnUpgradeNPC(convOn);
        }

    }

}
