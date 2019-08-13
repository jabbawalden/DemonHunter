using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _energySlider;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _energyImage;
    [SerializeField] private float _lerpColourTime;
    [SerializeField] private float _colourResetTime;
    [SerializeField] private Text _meleeUI, _shootUI, _dashUI, _pointsUI;
    [SerializeField] private Color colorMoveReady, colorMoveRecharge;
    private bool canChangeColorHealth;
    private bool canChangeColorEnergy;

    private C_Health playerHealthComponenent;
    private PlayerEnergy playerEnergy;

    private PlayerMeleeAttack playerMeleeAttack;
    private PlayerShoot playerShoot;
    private PlayerDash playerDash;
    private PlayerEnergyPoints playerEnergyPoints;
    private PlayerUpgradesManager playerUpgradesManager;
    private GameManager gameManager;
    //private PlayerUpgradesManager _playerUpgradesManager;
    private bool _isFadingIn, _isFadingOut;
    [Space(4)]

    [Header("NPC and Upgrades UI")]
    public Text _npcDialogue;
    [SerializeField] private GameObject npcPanel;
    [SerializeField] private GameObject upgradePanel;

    [SerializeField] private Text _speedUpgradesLeft;
    [SerializeField] private Text _healthUpgradesLeft;
    [SerializeField] private Text _energyUpgradeLeft;
    [SerializeField] private Text _damageUpgradesLeft;
    [SerializeField] private Text _healthRegenStatus;
    public Text speedUpgradesLeft { get { return _speedUpgradesLeft; } private set { _speedUpgradesLeft = value; } }
    public Text healthUpgradesLeft { get { return _healthUpgradesLeft; } private set { _healthUpgradesLeft = value; } }
    public Text energyUpgradeLeft { get { return _energyUpgradeLeft; } private set { _energyUpgradeLeft = value; } }
    public Text damageUpgradesLeft { get { return _damageUpgradesLeft; } private set { _damageUpgradesLeft = value; } }
    public Text healthRegenStatus { get { return _healthRegenStatus; } private set { _healthRegenStatus = value; } }
    public string regenStatusOn;
    public string regenStatusOff;

    [SerializeField] private Button speedB, healthB, energyB, damageB, healthRegenB;
    [Space(4)]


    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button quitGame;


    private void Awake()
    {
        playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        playerShoot = FindObjectOfType<PlayerShoot>();
        playerDash = FindObjectOfType<PlayerDash>();
        playerHealthComponenent = GameObject.Find("PlayerController").GetComponent<C_Health>();
        playerEnergy = FindObjectOfType<PlayerEnergy>();
        playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        npcPanel.SetActive(false);
        upgradePanel.SetActive(false);
        pauseMenu.SetActive(false);

        canChangeColorHealth = false;
        canChangeColorEnergy = false;

        AddButtonFunctionality();
    }

    private void Update()
    {
        if (canChangeColorHealth)
            BarColourReaction(false, _healthImage);

        if (canChangeColorEnergy)
            BarColourReaction(true, _energyImage);

        MoveSetColour();
    }

    public void AddButtonFunctionality()
    {
        speedB.onClick.AddListener (delegate { playerUpgradesManager.UpgradeActivation(1); });
        healthB.onClick.AddListener (delegate { playerUpgradesManager.UpgradeActivation(2); });
        energyB.onClick.AddListener (delegate { playerUpgradesManager.UpgradeActivation(3); });
        damageB.onClick.AddListener(delegate { playerUpgradesManager.UpgradeActivation(4); });
        healthRegenB.onClick.AddListener (delegate { playerUpgradesManager.UpgradeActivation(5); });
        quitGame.onClick.AddListener(gameManager.ExitGame);
    }

    private void BarColourReaction(bool energy, Image barImage)
    {
        if (!energy)
        {
            Color newColor;
            newColor = barImage.color;
            newColor = new Color(Mathf.Lerp(barImage.color.r, 0, _lerpColourTime), newColor.g, Mathf.Lerp(barImage.color.b, 0, _lerpColourTime), newColor.a);
            barImage.color = newColor;
        }
        else
        {
            Color newColor;
            newColor = barImage.color;
            newColor = new Color(Mathf.Lerp(barImage.color.r, 0, _lerpColourTime), newColor.g, Mathf.Lerp(barImage.color.b, 0.9f, _lerpColourTime), newColor.a);
            barImage.color = newColor;
        }
    }
    
    public void UpdateEnergyPoints()
    {
        _pointsUI.text = playerEnergyPoints.energyPoints.ToString();
    } 

    public void UpdateHealthSlider()
    {
        _healthSlider.value = playerHealthComponenent.GetHealthPercent();
    }

    public void DamageHealthBar()
    {
        //set new colour
        _healthImage.color = new Color(1, _healthImage.color.g, 1, _healthImage.color.a);
        canChangeColorHealth = true;
        StartCoroutine(ReturnFalse(canChangeColorHealth));
    }

    public void UpdateEnergySlider()
    {
        _energySlider.value = playerEnergy.GetEnergyPercent();
    }

    public void DamageEnergyBar()
    {
        //set new colour
        _energyImage.color = new Color(1, _energyImage.color.g, _energyImage.color.b, _energyImage.color.a);
        canChangeColorEnergy = true;
        StartCoroutine(ReturnFalse(canChangeColorEnergy));
    }

    IEnumerator ReturnFalse (bool boolean)
    {
        yield return new WaitForSeconds(_colourResetTime);
        boolean = false;
    }

    private void MoveSetColour()
    {
        if (playerMeleeAttack.meleeIconLit)
            _meleeUI.color = colorMoveReady;
        else
            _meleeUI.color = colorMoveRecharge;

        if (playerShoot.shootIconLit)
            _shootUI.color = colorMoveReady;
        else
            _shootUI.color = colorMoveRecharge;

        if (playerDash.dashIconLit)
            _dashUI.color = colorMoveReady;
        else
            _dashUI.color = colorMoveRecharge;
    }

    public IEnumerator TextFadeCo(bool isOn, float speed, float time, Text text, Color startColor, Color endColor, float lerpSpeed)
    {
        speed = 0;

        if (isOn)
        {
            text.color = startColor;
            _isFadingIn = true;
            _isFadingOut = false;
            while (speed < time && _isFadingIn)
            {
                speed++;
                text.color = Color.Lerp(text.color, endColor, lerpSpeed);
                yield return new WaitForSeconds(0.05f);
                //lerp colour
            }
            text.color = endColor;
        }
        else
        {
            text.color = endColor;
            _isFadingIn = false;
            _isFadingOut = true;
            while (speed < time && _isFadingOut)
            {
                speed++;
                text.color = Color.Lerp(text.color, startColor, lerpSpeed);
                yield return new WaitForSeconds(0.05f);
                //lerp colour
            }
            text.color = startColor;
        }
        //print("lerp complete");
    }

    public void PausePanelActivate(bool isOn)
    {
        pauseMenu.SetActive(isOn);
    }

    public void TurnOnUpgradeNPC(bool on)
    {
        if (on)
        {
            npcPanel.SetActive(true);
            upgradePanel.SetActive(true);
        }
        else
        {
            npcPanel.SetActive(false);
            upgradePanel.SetActive(false);
        }

    }

    public void TurnOnShopNPC(bool on)
    {
        if (on)
            npcPanel.SetActive(true);
        else
            npcPanel.SetActive(false);
    }

    public void TurnOnOldWomanNPC(bool on)
    {
        if (on)
            npcPanel.SetActive(true);
        else
            npcPanel.SetActive(false);
    }

    public void UpdateUpgradesCount(int amount, Text text)
    {
        text.text = amount.ToString();
    }
}
