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
        if (hitCount == _maxHitCount)
            Destroy(gameObject);
        else
            hitCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.GetComponentInParent<C_Health>() != null)
        {
            _healthComponent = collision.GetComponentInParent<C_Health>();
            _healthComponent.Damage(damage);
            HitCounter();
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
