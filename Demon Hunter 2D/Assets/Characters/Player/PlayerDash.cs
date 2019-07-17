using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerEnergy _playerEnergy;
    private C_Health _playerHealthComp;
    private C_Health _enemyHealthComp;
    private UIManager _uiManager;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    [SerializeField] private PlayerCamera _playerCamera;

    [SerializeField] private int[] layersToIgnore;
    [SerializeField] private float _dashEnergyCost;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDamage;
    [SerializeField] private float _dashHealAmount;
    [SerializeField] private BoxCollider2D playerBodyCollision;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private GameObject playerDashCollider;

    private bool _canDashDamage;
    public bool playerDashEnabled;
    public bool dashIconLit;
    

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerHealthComp = GetComponent<C_Health>();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

    }

    private void Start()
    {
        playerDashCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealthComp.IsAlive())
            Dash();

        if (_playerEnergy.currentEnergy >= _dashEnergyCost)
            dashIconLit = true;
        else
            dashIconLit = false;
    }


    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerEnergy.currentEnergy >= _dashEnergyCost && playerDashEnabled)
        {
            _playerEnergy.RemoveEnergy(_dashEnergyCost);
            StartCoroutine(DashBehaviour(4.5f, 0.3f));
            _uiManager.UpdateEnergySlider();
            _uiManager.DamageEnergyBar();
            _playerCamera.CameraShake(0.15f, 0.11f);

            _gameManager.TutorialCheckDash();
        }
    }



    IEnumerator DashBehaviour(float time, float speed)
    {
        float count = 0;
        Vector2 currentAimDirection = _playerController.AimDirection();

        playerDashCollider.SetActive(true);
        _playerController.canMove = false;
        _canDashDamage = true;
        _circleCollider.enabled = true;
        //ensures that attacks will not detect player collider, thus invulnerable
        playerBodyCollision.enabled = false;

        while (count < time)
        {
            count += speed;
            _rb.velocity = currentAimDirection * _dashSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        _playerController.canMove = true;
        _canDashDamage = false;
        _circleCollider.enabled = false;
        playerBodyCollision.enabled = true;
        yield return new WaitForSeconds(0.05f);
        playerDashCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && _canDashDamage && collision.GetComponentInParent<C_Health>() != null && collision.GetComponentInParent<EnemyController>() != null)
        {
            //if damage from dash is enabled i.e. we can "see" the enemy
            if (collision.GetComponentInParent<EnemyController>().canRecieveDamage)
            {
                _enemyHealthComp = collision.GetComponentInParent<C_Health>();
                _playerHealthComp.Heal(_dashHealAmount);
                _enemyHealthComp.Damage(_dashDamage);
            }


            _uiManager.UpdateHealthSlider();
            //_uiManager.DamageHealthBar();
        }
    }
}
