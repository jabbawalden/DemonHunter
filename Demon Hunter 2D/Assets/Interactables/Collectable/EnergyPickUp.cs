using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour
{
    private PlayerController _playerController;
    Rigidbody2D _rb;
    [SerializeField] private int _energyToAdd;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSpeedIncrease;
    [SerializeField] private float _bounceForce;
    [SerializeField] private float _timeBounceStart; 
    [SerializeField] private float _distanceReference;
    [SerializeField] private float _distanceToMove;
    
    bool canBounce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canBounce = true;
        //Invoke("BounceyBehaviour", 0.4f);
        float r = Random.Range(-_bounceForce, _bounceForce);
        _rb.AddForce(new Vector2(r, _bounceForce));
        Invoke("NewRigidbodyBehaviour", 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        EnergyBehaviour();
    }

    private void EnergyBehaviour()
    {
        if (_playerController != null)
        {
            float distance = Vector3.Distance(_playerController.transform.position, transform.position);
            Vector2 direction = _playerController.transform.position - transform.position;
            print(distance);

            if (distance <= _distanceReference)
            {
                _rb.velocity = direction.normalized * _moveSpeed * Time.deltaTime;
                _moveSpeed += _moveSpeedIncrease;
            }

            if (distance <= _distanceToMove)
            {
                _playerController.AddEnergy(_energyToAdd);
                Destroy(gameObject);
            }
           

        }
    }
    
   //
   //void BounceyBehaviour()
   //{
   //    StartCoroutine(BounceBehaviour());
   //}

   //IEnumerator BounceBehaviour()
   //{
   //    int bounceTimes = 5;

   //    while (bounceTimes > 0)
   //    {
   //        bounceTimes--;

   //        if (_timeBounceStart >= 0.1f)
   //            _timeBounceStart *= 0.8f;

   //        print("bounce");
   //        _rb.AddForce(new Vector2(0, _bounceForce));
   //        yield return new WaitForSeconds(_timeBounceStart);
   //    }

   //}
   
    void NewRigidbodyBehaviour()
    {
        _rb.gravityScale = 0;
        _rb.velocity = new Vector3(0,0,0);
        canBounce = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            _playerController = collision.GetComponentInParent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            _playerController = null;
        }
    }

}
