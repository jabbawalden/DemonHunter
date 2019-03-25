using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    
    private PlayerController _playerController;
    private C_Health _healthComponent;

    [Header("Variables")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootAnimationTime; 
    private float _newTime; 

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _healthComponent = GetComponent<C_Health>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_healthComponent.isAlive())
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                ShootAbility();
                _playerController.StopVelocity();
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
        }
    }

    IEnumerator ShootBehaviour()
    {
        
        yield return new WaitForSeconds(_shootAnimationTime);
         
    }
}
