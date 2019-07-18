using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheckPoint : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            gameManager.finishedTutorial = true;
        }
    }

}
