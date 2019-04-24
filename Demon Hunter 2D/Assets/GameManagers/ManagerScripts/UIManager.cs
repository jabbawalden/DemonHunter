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
    [SerializeField] private float lerpColourTime;
    [SerializeField] private float colourResetTime;
    private bool canChangeColorHealth;
    private bool canChangeColorEnergy;

    private C_Health _playerHealthComponenent;
    private PlayerEnergy _playerEnergy;

    private void Awake()
    {
        _playerHealthComponenent = GameObject.Find("PlayerController").GetComponent<C_Health>();
        _playerEnergy = GameObject.Find("PlayerController").GetComponent<PlayerEnergy>();
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
        //else
        //    BarColourReaction(true, _healthImage);

        if (canChangeColorEnergy)
            BarColourReaction(true, _energyImage);
        /*
        else
            BarColourReaction(true, _energyImage);*/

    }
    
    private void BarColourReaction(bool energy, Image barImage)
    {
        if (!energy)
        {
            Color newColor;
            newColor = barImage.color;
            newColor = new Color(Mathf.Lerp(barImage.color.r, 0, lerpColourTime), newColor.g, Mathf.Lerp(barImage.color.b, 0, lerpColourTime), newColor.a);
            barImage.color = newColor;
        }
        else
        {
            Color newColor;
            newColor = barImage.color;
            newColor = new Color(Mathf.Lerp(barImage.color.r, 0, lerpColourTime), newColor.g, newColor.b, newColor.a);
            barImage.color = newColor;
        }
        /*
        else
        {
            newColor = barImage.color;
            barImage.color = new Color(0, newColor.g, newColor.b, newColor.a);
        }*/
    }
    
    public void UpdateHealthSlider()
    {
        _healthSlider.value = _playerHealthComponenent.GetHealthPercent();
        StartCoroutine(ReturnFalse(canChangeColorHealth));
    }

    public void DamageHealthBar()
    {
        //set new colour
        _healthImage.color = new Color(1, _healthImage.color.g, 1, _healthImage.color.a);
        canChangeColorHealth = true;
    }

    public void UpdateEnergySlider()
    {
        _energySlider.value = _playerEnergy.GetEnergyPercent();
        StartCoroutine(ReturnFalse(canChangeColorEnergy));
    }

    public void DamageEnergyBar()
    {
        //set new colour
        _energyImage.color = new Color(1, _energyImage.color.g, _energyImage.color.b, _energyImage.color.a);
        canChangeColorEnergy = true;
    }

    IEnumerator ReturnFalse (bool boolean)
    {
        yield return new WaitForSeconds(colourResetTime);
        boolean = false;
    }

}
