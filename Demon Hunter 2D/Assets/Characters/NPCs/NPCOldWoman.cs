using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOldWoman : NPCInteraction
{
    // Start is called before the first frame update
    void Start()
    {
        setUI = OldWomanOptions;
    }

    public void OldWomanOptions(bool convOn)
    {
        if (convOn)
        {
            uiManager.TurnOnOldWomanNPC(convOn);
            npcManager.SetOldWomanSpeech();
        }
        else
        {
            uiManager.TurnOnOldWomanNPC(convOn);
        }

    }
}
