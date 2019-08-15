using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{



    private TutorialManager _tutorialManager;
    private PlayerController _playerController;
    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerShoot _playerShoot;
    private PlayerDash _playerDash;
    private JsonDataManager _jsonDataManager;
    private UIManager uiManager;
    [System.NonSerialized] public bool gameIntroMove;
    [System.NonSerialized] public bool gameIntroMelee;
    [System.NonSerialized] public bool gameIntroShoot;
   /* [System.NonSerialized]*/ public bool gameIntroDash;

  /*  [System.NonSerialized] */public bool playerInTown;

    public bool IsPause { get; private set; }

    private void OnEnable()
    {
        GameEvents.EventGameExitMain += ExitGame;
        GameEvents.EventLoadLastSave += LoadLastSave;
        GameEvents.EventQuitToMenu += LoadMainMenu;
    }

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerDash = FindObjectOfType<PlayerDash>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _jsonDataManager = FindObjectOfType<JsonDataManager>();
        uiManager = FindObjectOfType<UIManager>();

    }

    private void Start()
    {

    }

    void Update()
    {
        //if (_playerController.deathEnabled)
        //    if (Input.GetKey(KeyCode.Return))
        //    {
        //        SceneManager.LoadScene(0);
        //    }
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
    }

    public void ExitGame()
    {
        //if player exits the game when inTown is true, save game upon exit
        if (playerInTown)
            GameEvents.ReportSaveFileExitTown();
        else
            GameEvents.ReportSaveFileExitNormal();

        print("Exit Game");
        Application.Quit();
    }

    public void PauseMenuActivation()
    {
        if (!IsPause)
        {
            Time.timeScale = 0;
            IsPause = true;
            uiManager.PausePanelActivate(true);
        }
        else
        {
            IsPause = false;
            Time.timeScale = 1;
            uiManager.PausePanelActivate(false);
        }
    }

    public void LoadLastSave()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Load last save");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        print("Load main menu");
    }
}
