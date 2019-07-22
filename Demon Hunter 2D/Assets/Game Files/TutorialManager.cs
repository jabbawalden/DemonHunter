using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private UIManager uiManager;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeTime;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Text tutMove, tutMelee, tutShoot, tutDash;
    [SerializeField] private Color whiteStartColor, whiteEndColor;

    //Debugging
    GameManager gameManager;

    [System.NonSerialized] public bool tutorialComplete;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        tutMove.color = whiteStartColor;
        tutMelee.color = whiteStartColor;
        tutShoot.color = whiteStartColor;
        tutDash.color = whiteStartColor;


        //if (!gameManager.finishedTutorial)
        //    FadeTutMove(true);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //        StartCoroutine(uiManager.TextFadeCo(true, fadeSpeed, fadeTime, tutMove, whiteStartColor, whiteEndColor, lerpSpeed));
    //}

    public void LoadData()
    {
        tutorialComplete = JsonDataManager.gameData.tutorialComplete;
    }

    public void FadeTutMove(bool activate)
    {
        if (activate)
            StartCoroutine(uiManager.TextFadeCo(true, fadeSpeed, fadeTime, tutMove, whiteStartColor, whiteEndColor, lerpSpeed));
        else
            StartCoroutine(uiManager.TextFadeCo(false, fadeSpeed, fadeTime, tutMove, whiteStartColor, whiteEndColor, lerpSpeed));
    }

    public void FadeTutMelee(bool activate)
    {
        if (activate)
            StartCoroutine(uiManager.TextFadeCo(true, fadeSpeed, fadeTime, tutMelee, whiteStartColor, whiteEndColor, lerpSpeed));
        else
            StartCoroutine(uiManager.TextFadeCo(false, fadeSpeed, fadeTime, tutMelee, whiteStartColor, whiteEndColor, lerpSpeed));
    }

    public void FadeTutShoot(bool activate)
    {
        if (activate)
            StartCoroutine(uiManager.TextFadeCo(true, fadeSpeed, fadeTime, tutShoot, whiteStartColor, whiteEndColor, lerpSpeed));
        else
            StartCoroutine(uiManager.TextFadeCo(false, fadeSpeed, fadeTime, tutShoot, whiteStartColor, whiteEndColor, lerpSpeed));
    }

    public void FadeTutDash(bool activate)
    {
        if (activate)
            StartCoroutine(uiManager.TextFadeCo(true, fadeSpeed, fadeTime, tutDash, whiteStartColor, whiteEndColor, lerpSpeed));
        else
            StartCoroutine(uiManager.TextFadeCo(false, fadeSpeed, fadeTime, tutDash, whiteStartColor, whiteEndColor, lerpSpeed));
    }
}
