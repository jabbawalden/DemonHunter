using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerEnergy playerEnergy;
    private HealthComponent playerHealthComp;
    private HealthComponent enemyHealthComp;
    private UIManager uiManager;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private PlayerCamera playerCamera;

    [SerializeField] private int[] layersToIgnore;
    [SerializeField] private float dashEnergyCostSet;
    public float dashEnergyCost
    {
        get
        {
            return dashEnergyCostSet;
        }
    }
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDamage;
    [SerializeField] private float dashHealAmount;
    [SerializeField] private float dashTime;
    [SerializeField] private BoxCollider2D playerBodyCollision;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private GameObject playerDashCollider;

    private bool canDashDamage;
    public bool playerDashEnabled;
    public bool dashIconLit;
    private bool isDashing;
    private Vector2 currentAimDirection;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealthComp = GetComponent<HealthComponent>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void Start()
    {
        playerDashCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthComp.IsAlive())
            DashIconUI();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = currentAimDirection * dashSpeed;
        }
    }

    public void LoadData()
    {
        playerDashEnabled = JsonDataManager.gameData.dashEnabled;
    }

    public void Dash()
    {
        playerEnergy.RemoveEnergy(dashEnergyCost);
        StartCoroutine(DashBehaviour(dashTime));
        uiManager.UpdateEnergySlider();
        uiManager.DamageEnergyBar();
        playerCamera.CameraShake(0.15f, 0.11f);

        gameManager.TutorialCheckDash();
    }

    private void DashIconUI()
    {
        if (playerEnergy.currentEnergy >= dashEnergyCost)
            dashIconLit = true;
        else
            dashIconLit = false;
    }

    IEnumerator DashBehaviour(float time)
    {
        float count = 0;
        currentAimDirection = playerController.AimDirection();
        playerDashCollider.SetActive(true);
        playerController.canMove = false;
        canDashDamage = true;
        circleCollider.enabled = true;
        playerBodyCollision.enabled = false;
        isDashing = true;
        /*
        while (count < time)
        {
            count += speed;
            //_rb.velocity = currentAimDirection * _dashSpeed;
            transform.Translate(currentAimDirection * _dashSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        */
        yield return new WaitForSeconds(time);
        isDashing = false;
        playerController.canMove = true;
        canDashDamage = false;
        circleCollider.enabled = false;
        playerBodyCollision.enabled = true;
        yield return new WaitForSeconds(0.05f);
        playerDashCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && canDashDamage && collision.GetComponentInParent<HealthComponent>() != null && collision.GetComponentInParent<EnemyController>() != null)
        {
            //if damage from dash is enabled i.e. we can "see" the enemy
            if (collision.GetComponentInParent<EnemyController>().canRecieveDamage)
            {
                enemyHealthComp = collision.GetComponentInParent<HealthComponent>();
                playerHealthComp.Heal(dashHealAmount);
                enemyHealthComp.Damage(dashDamage * playerController.DamageMultiplier);
            }


            uiManager.UpdateHealthSlider();
            //_uiManager.DamageHealthBar();
        }
    }
}
