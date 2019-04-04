using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    
    private PlayerController _playerController;
    private C_Health _healthComponent;
    private UIManager _uiManager;

    [Header("Variables")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootAnimationTime;
    [SerializeField] private float _projDamage;
    [SerializeField] private float _healthCost;
    private float _newTime;
    public GameObject projectile;
    [SerializeField] private Transform _shootOrigin;
    [Space(4)]

    //[Header("Scripts")]
    private ProjectileBehaviour projBehaviour;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _healthComponent = GetComponent<C_Health>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.isAlive())
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_healthComponent._currentHealth > _healthCost)
                {
                    ShootAbility();
                    _playerController.StopVelocity();
                }     
            }
            else
            {
                _playerController.canMove = true;
            }
        }
    }

    private void ShootAbility()
    {
        _playerController.canMove = false;

        if (Time.time >= _newTime)
        {
            print("shoot");
            _newTime = Time.time + _fireRate;
            StartCoroutine(ShootBehaviour());

            GameObject proj = Instantiate(projectile, _shootOrigin.position, projectile.transform.rotation);
            Vector2 direction = _playerController.AimDirection().normalized;

            proj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            proj.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed * Time.deltaTime;
            //set damage
            projBehaviour = proj.GetComponent<ProjectileBehaviour>();
            projBehaviour.damage = _projDamage;

            _healthComponent.Damage(_healthCost);
            _uiManager.UpdateHealthSlider();

        }
    }

    IEnumerator ShootBehaviour()
    {
        
        yield return new WaitForSeconds(_shootAnimationTime);
         
    }
}
