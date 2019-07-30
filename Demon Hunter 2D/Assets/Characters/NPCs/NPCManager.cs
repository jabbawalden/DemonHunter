using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    //private Text npcText;
    int counter;
    private UIManager _uiManager;
    //temporary bool
    bool conversationFinished;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        //temporary input
        if (Input.GetKeyDown(KeyCode.Space) && conversationFinished)
        {
            counter++;
            OldWomanText(counter);
        }
    }

    public void OldWomanText(int index)
    {
        _uiManager._npcDialogue.text = "Hmm mm, mmm yes... now where did I put those potatoes?";

        switch (index)
        {
            case 1:
                _uiManager._npcDialogue.text = "Hello there, welcome to our village";
                break;
            case 2:
                _uiManager._npcDialogue.text = 
                    "Unusual to see visitors this time of year... hee hee! \n" +
                    "I imagine you'll be staying a while? \n" + 
                    "Very good, please enjoy your time here...";
                break;

            case 3:
                _uiManager._npcDialogue.text = 
                    "Oh and whatever you do... \n" + 
                    "NEVER go north... for your health deary";
                break;
            //temporary case
            case 4:
                conversationFinished = true;
                break;
        }
    }
    
}
