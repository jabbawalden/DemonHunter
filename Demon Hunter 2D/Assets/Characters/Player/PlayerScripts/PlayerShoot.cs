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
    [SerializeField] private float _projDamage;
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

            GameObject proj = Instantiate(projectile, _shootOrigin.position, projectile.transform.rotation);
            Vector2 direction = _playerController.AimDirection().normalized;

            //proj.transform.LookAt(new Vector2 (direction.x, 90));
            //Quaternion q = Quaternion.FromToRotation(Vector2.up, direction);
            //Quaternion.LookRotation(Vector2.up, direction);
            //proj.transform.rotation = q * proj.transform.rotation;
            proj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            proj.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed * Time.deltaTime;
            projBehaviour = proj.GetComponent<ProjectileBehaviour>();
            projBehaviour.damage = _projDamage;

        }
    }

    IEnumerator ShootBehaviour()
    {
        
        yield return new WaitForSeconds(_shootAnimationTime);
         
    }
}
