using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTempleGate : MonoBehaviour
{

    [SerializeField] private GameObject gate;
    [SerializeField] private Collider2D collider;
    private PlayerItemScript playerItems;

    private void Awake()
    {
        
    }
   
    void OpenGate()
    {
        if (playerItems.MainGateKey)
        {
            gate.SetActive(false);
            collider.enabled = false;
        }
          
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerItemScript>())
        {
            playerItems = collision.GetComponentInParent<PlayerItemScript>();
            //will later be activated via a keypress - the same as for dialogue
            OpenGate();
        }
    }
}
