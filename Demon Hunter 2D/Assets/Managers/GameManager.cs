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
    [System.NonSerialized] public bool gameIntroMove;
    [System.NonSerialized] public bool gameIntroMelee;
    [System.NonSerialized] public bool gameIntroShoot;
   /* [System.NonSerialized]*/ public bool gameIntroDash;
    [Space(4)]
    [Header("Debug")]
    public bool finishedTutorial;
    public Transform startPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


        _tutorialManager = FindObjectOfType<TutorialManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerDash = FindObjectOfType<PlayerDash>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.deathEnabled)
            if (Input.GetKey(KeyCode.Return))
            {
                startPosition = _playerController.gameObject.transform;
                SceneManager.LoadScene(0);
            }

        //debugging for the moment
        if (finishedTutorial)
        {
            _tutorialManager = FindObjectOfType<TutorialManager>();
            _playerController = FindObjectOfType<PlayerController>();
            _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
            _playerDash = FindObjectOfType<PlayerDash>();
            _playerShoot = FindObjectOfType<PlayerShoot>();
            _playerDash.playerDashEnabled = true;
            _playerShoot.playerShootEnabled = true;
            _playerMeleeAttack.playerMeleeEnabled = true;

        }


    }

    public void TutorialCheckMove()
    {
        if (!gameIntroMove)
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
}
