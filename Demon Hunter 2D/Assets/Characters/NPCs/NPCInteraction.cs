using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public UIManager _uiManager { get; private set;}
    private PlayerController _playerController;

    public bool canInteract;
    public delegate void SetUI(bool convOn);
    public SetUI setUI;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _playerController = FindObjectOfType<PlayerController>();
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
