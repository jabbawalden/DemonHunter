using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public UIManager uiManager { get; private set;}
    private PlayerController _playerController;
    public NPCManager npcManager { get; protected set; }
    [SerializeField] private int _maxTalkCount;
    public int maxTalkCount
    {
        get
        {
            return _maxTalkCount;
        }

        protected set
        {
            _maxTalkCount = value;
        }
    }

    public bool canInteract;
    public delegate void SetUI(bool convOn);
    public SetUI setUI;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        _playerController = FindObjectOfType<PlayerController>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    public void Interaction(bool convOn)
    {
        if (convOn)
        {
            setUI(true);
            _playerController.isNPCInteracting = true;
        }

        else if (!convOn)
        {
            setUI(false);
            _playerController.isNPCInteracting = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //player in range
            _playerController.selectedNPC = Interaction;
            _playerController.npcSet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //player out of range
            _playerController.selectedNPC -= Interaction;
            _playerController.npcSet = false;
        }
    }
}
