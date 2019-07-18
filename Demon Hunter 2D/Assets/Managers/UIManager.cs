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

    private C_Health _playerHealthComponenent;
    private PlayerEnergy _playerEnergy;

    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerShoot _playerShoot;
    private PlayerDash _playerDash;
    private PlayerEnergyPoints _playerEnergyPoints;
    private bool _isFadingIn, _isFadingOut;

    private void Awake()
    {
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _playerDash = FindObjectOfType<PlayerDash>();
        _playerHealthComponenent = GameObject.Find("PlayerController").GetComponent<C_Health>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();
    }

    private void Start()
    {
        canChangeColorHealth = false;
        canChangeColorEnergy = false; 
    }

    private void Update()
    {
        if (canChangeColorHealth)
            BarColourReaction(false, _healthImage);

        if (canChangeColorEnergy)
            BarColourReaction(true, _energyImage);

        MoveSetColour();
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
        _pointsUI.text = _playerEnergyPoints.energyPoints.ToString();
    } 

    public void UpdateHealthSlider()
    {
        _healthSlider.value = _playerHealthComponenent.GetHealthPercent();
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
        _energySlider.value = _playerEnergy.GetEnergyPercent();
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
        if (_playerMeleeAttack.meleeIconLit)
            _meleeUI.color = colorMoveReady;
        else
            _meleeUI.color = colorMoveRecharge;

        if (_playerShoot.shootIconLit)
            _shootUI.color = colorMoveReady;
        else
            _shootUI.color = colorMoveRecharge;

        if (_playerDash.dashIconLit)
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
}
