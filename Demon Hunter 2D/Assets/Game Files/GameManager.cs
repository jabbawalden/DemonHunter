using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance; 

    private TutorialManager _tutorialManager;
    private PlayerController _playerController;
    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerShoot _playerShoot;
    private PlayerDash _playerDash;
    private JsonDataManager _jsonDataManager;
    [System.NonSerialized] public bool gameIntroMove;
    [System.NonSerialized] public bool gameIntroMelee;
    [System.NonSerialized] public bool gameIntroShoot;
   /* [System.NonSerialized]*/ public bool gameIntroDash;

    [System.NonSerialized] public bool playerInTown; 

    private void Awake()
    {
        //Singleton code to be used for later reference
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        */

        _tutorialManager = FindObjectOfType<TutorialManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerDash = FindObjectOfType<PlayerDash>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _jsonDataManager = FindObjectOfType<JsonDataManager>();
    }

    private void Start()
    {

    }

    void Update()
    {
        if (_playerController.deathEnabled)
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene(0);
            }
    }

    public void LoadData()
    {
        gameIntroMove = JsonDataManager.gameData.gameIntroMove; 
        gameIntroMelee = JsonDataManager.gameData.gameIntroMelee;
        gameIntroShoot = JsonDataManager.gameData.gameIntroShoot;
        gameIntroDash = JsonDataManager.gameData.gameIntroDash;
    }

    public void TutorialCheckMove()
    {
        if (!_tutorialManager.tutorialComplete && !gameIntroMove)
        {
            gameIntroMove = true;
            _tutorialManager.FadeTutMove(false);
        }
    }

    public void TutorialCheckMelee()
    {
        if (!gameIntroMelee)
        {
            gameIntroMelee = true;
            _tutorialManager.FadeTutMelee(false);
        }
    }

    public void TutorialCheckShoot()
    {
        if (!gameIntroShoot)
        {
            gameIntroShoot = true;
            _tutorialManager.FadeTutShoot(false);
        }
    }

    public void TutorialCheckDash()
    {
        if (!gameIntroDash)
        {
            gameIntroDash = true;
            _tutorialManager.FadeTutDash(false);
            print("fade dash");
        }
    }

    public void PlayerInTownCentre(bool inTown) 
    {
        playerInTown = inTown;
        //if player is in town centre - bool checked to true
        //if player exits the game when inTown is true, save game upon exit
    }

    public void ExitGame()
    {
        if (playerInTown)
            _jsonDataManager.MainSaveGameExit();
        Application.Quit();
    }
}
