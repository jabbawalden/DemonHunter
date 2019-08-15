using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainPage, LoadGamePage;
    [SerializeField] private Button startGame, quit;
    [SerializeField] private Button slot1, slot2, slot3, back;
    private GameTracker gameTracker;

    private void Awake()
    {
        gameTracker = FindObjectOfType<GameTracker>();
        MainPage.SetActive(true);
        LoadGamePage.SetActive(false);
    }

    private void Start()
    {
        slot1.onClick.AddListener(delegate { gameTracker.LoadPath(1); });
        slot2.onClick.AddListener(delegate { gameTracker.LoadPath(2); });
        slot3.onClick.AddListener(delegate { gameTracker.LoadPath(3); });
        startGame.onClick.AddListener(StartGame);
        quit.onClick.AddListener(QuitGame);
        back.onClick.AddListener(BackToMain);
    }

    private void StartGame()
    {
        MainPage.SetActive(false);
        LoadGamePage.SetActive(true);
    }

    private void BackToMain()
    {
        MainPage.SetActive(true);
        LoadGamePage.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

}
