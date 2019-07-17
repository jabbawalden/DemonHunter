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
    [System.NonSerialized] public bool gameIntroMove;
    [System.NonSerialized] public bool gameIntroMelee;
    [System.NonSerialized] public bool gameIntroShoot;
   /* [System.NonSerialized]*/ public bool gameIntroDash;

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerDash = FindObjectOfType<PlayerDash>();
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
                SceneManager.LoadScene(0);
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
