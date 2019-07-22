using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCamera playerCamera;
    private JsonDataManager jsonDataManager;
    private Vector3 checkPointPosition;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        jsonDataManager = FindObjectOfType<JsonDataManager>();        
    }

    private void Start()
    {
        checkPointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //save player position
            playerController.startLocation = checkPointPosition;
            playerCamera.startLocation = checkPointPosition;
            //save data to json file
            jsonDataManager.MainSaveCheckPoint();
            Debug.Log("Player reached checkpoint");
        }
    }
}
