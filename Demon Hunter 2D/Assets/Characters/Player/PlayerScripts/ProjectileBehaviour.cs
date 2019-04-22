using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float damage;
    private C_Health _healthComponent;
    private PlayerEnergy _playerEnergy;
    [System.NonSerialized] public int hitCount;
    [SerializeField] private int _maxHitCount;
    public int targetLayer;
    public bool isPlayerProj;
    [SerializeField] private GameObject explosion;
    public Vector2 currentDirection;
    public float currentSpeed;

    private void Awake()
    {
        _healthComponent = GetComponent<C_Health>();
        _playerEnergy = GetComponent<PlayerEnergy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HitCounter()
    {
        hitCount++;
        if (hitCount >= _maxHitCount)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == targetLayer && collision.GetComponentInParent<C_Health>() != null)
        { 
            print("Hit player");
            _healthComponent = collision.GetComponentInParent<C_Health>();
            _healthComponent.Damage(damage);
            HitCounter();
        }
        else if (collision.gameObject.layer == 13 && !isPlayerProj)
        {
            //if player melee attack
            //Destroy(gameObject);
            print("projectile reverted");
        }
        else if (collision.gameObject.layer == 14 && !isPlayerProj)
        {
            //if we are enemy and run into playerproj layer
            //explode
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 15 && isPlayerProj)
        {
            //if we are player proj and run into enemy proj
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
