using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShop : NPCInteraction
{
    // Start is called before the first frame update
    void Start()
    {
        setUI = ShopOptions;
    }

    public void ShopOptions(bool convOn)
    {
        if (convOn)
        {
            npcManager.maxTalkCount = maxTalkCount;
            uiManager.TurnOnShopNPC(convOn);
            npcManager.SetShopSpeech();
        }
        else
        {
            uiManager.TurnOnShopNPC(convOn);
        }

    }

}
