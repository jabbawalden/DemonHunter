using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerCamera _playerCamera;
    private JsonDataManager _jsonDataManager;
    private GameManager _gameManager;
    private Vector3 checkPointPosition;
    [SerializeField] private bool isTownCheckPoint;
    

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _jsonDataManager = FindObjectOfType<JsonDataManager>();
        _gameManager = FindObjectOfType<GameManager>();
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
            _playerController.startLocation = checkPointPosition;
            _playerCamera.startLocation = checkPointPosition;
            //save data to json file
            _jsonDataManager.MainSaveCheckPoint();

            if (isTownCheckPoint)
                _gameManager.playerInTown = true;
            Debug.Log("Player reached checkpoint");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && isTownCheckPoint)
        {
            _gameManager.playerInTown = false;
            Debug.Log("Player exited town checkpoint");
        }
    }
}
