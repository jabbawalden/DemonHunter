using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    //private Text npcText;
    public int mainTalkCount { get; private set; }
    [SerializeField] private int _secondCounter;
    public int secondCounter
    {
        get
        {
            return _secondCounter;
        }

        set
        {
            if (_secondCounter >= 3)
                _secondCounter = 1;
            else
                _secondCounter = value;
        }
    }
    [System.NonSerialized] public int maxTalkCount; 

    private UIManager _uiManager;
    private PlayerController _playerController;

    public bool oldWomanConv{ get; private set; }
    public bool shopConv{ get; private set; }
    public bool upgradeConv{ get; private set; }
    public bool currentConvCheck { get; private set; }

    public delegate void ConversationAction(int index);
    public ConversationAction conversationAction; 

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
         SetSpeechCounters();
    }

    private void Update()
    {
        //temporary input
    }

    public void LoadData()
    {
        oldWomanConv = JsonDataManager.gameData.oldWomanConv;
        shopConv = JsonDataManager.gameData.shopConv;
        upgradeConv = JsonDataManager.gameData.upgradeConv;
    }

    public void SetSpeechCounters()
    {
        mainTalkCount = 1;
        //secondCounter = 1;
    }

    //set inside each npc script
    public void SetOldWomanSpeech()
    {
        conversationAction = OldWomanText;
        conversationAction -= ShopText;
        conversationAction -= UpgradeText;
        currentConvCheck = oldWomanConv;
    }

    //set inside each npc script
    public void SetShopSpeech()
    {
        conversationAction -= OldWomanText;
        conversationAction = ShopText;
        conversationAction -= UpgradeText;
        currentConvCheck = shopConv;
    }

    //set inside each npc script
    public void SetUpgradeSpeech()
    {
        conversationAction -= OldWomanText;
        conversationAction -= ShopText;
        conversationAction = UpgradeText;
        currentConvCheck = upgradeConv;
    }

    public void ActivateSpeech()
    {
        conversationAction(mainTalkCount);
    }

    public void OldWomanText(int index)
    {
        print(mainTalkCount);

        if (!oldWomanConv)
        {
            if (mainTalkCount < 4)
            {
                mainTalkCount++;
            }
              
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
                    oldWomanConv = true;
                    break;
                //temporary case
            }
        }
        else
        {
            secondCounter++;

            switch (_secondCounter)
            {
                case 1:
                    _uiManager._npcDialogue.text = "Hmm mm, mmm yes... now where did I put those potatoes?";
                    break;
                case 2:
                    _uiManager._npcDialogue.text = "ouch... my back.";
                    break;
                case 3:
                    _uiManager._npcDialogue.text = "When I was your age... wait how old am I again?";
                    break;
            }
        }
    }
    
    public void ShopText(int index)
    {
        if (!shopConv)
        {

            if (mainTalkCount < 4)
            {
                mainTalkCount++;
            }


            //info about the store
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
                    shopConv = true;
                    break;
                    //temporary case
            }
        }
        else
        {
            secondCounter++;
            //official wording
            switch (_secondCounter)
            {
                case 1:
                    _uiManager._npcDialogue.text = "Hmm mm, mmm yes... now where did I put those potatoes?";
                    break;
                case 2:
                    _uiManager._npcDialogue.text = "ouch... my back.";
                    break;
                case 3:
                    _uiManager._npcDialogue.text = "When I was your age... wait how old am I again?";
                    break;
            }
        }
    }

    public void UpgradeText(int index)
    {
        //set later, first have introduction dialogue, then switches to shop mode.
        upgradeConv = true;
        _uiManager._npcDialogue.text = "Hello there friendo! \n so what shall yee be buying today. Got some good shit for ya!";
    }
}
