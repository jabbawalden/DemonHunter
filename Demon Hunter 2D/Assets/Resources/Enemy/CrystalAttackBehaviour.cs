using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAttackBehaviour : MonoBehaviour
{
    private PlayerCamera playerCam;
    public float damage;
    CircleCollider2D circleCollider;
    [SerializeField] private GameObject windUp, crystalize, crystalExplosion; 
    [SerializeField] private float attackTime;

    private void Awake()
    {
        playerCam = FindObjectOfType<PlayerCamera>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    private void Start()
    {
        playerCam.CameraShake(0.08f, 0.06f);
        StartCoroutine(AttackAnimTime());
    }

    IEnumerator AttackAnimTime()
    {
        yield return new WaitForSeconds(attackTime);
        circleCollider.enabled = true;
        windUp.SetActive(false);
        crystalize.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponentInParent<HealthComponent>().Damage(damage);
            crystalExplosion.SetActive(true);
            crystalize.SetActive(false);
            circleCollider.enabled = false;
            playerCam.CameraShake(0.12f, 0.15f);

        }
    }
}
