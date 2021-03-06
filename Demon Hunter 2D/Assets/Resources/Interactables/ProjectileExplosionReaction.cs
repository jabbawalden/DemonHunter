﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionReaction : MonoBehaviour
{
    
    [SerializeField] private float damage;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float fadeColor;
    [SerializeField] private MeshRenderer explosionMat;
    [SerializeField] private float fadelerp;
    private PlayerCamera playerCam;

    private void Awake() 
    {
        playerCam = GameObject.Find("CameraHolder").GetComponent<PlayerCamera>();
    }

    private void Start()
    {
        playerCam.CameraShake(0.2f, 0.15f);
    }

    private void Update()
    {
        FadeColorsOut();
        Invoke("DestroyObject", destroyTimer);
    }

    void FadeColorsOut()
    {
        fadeColor = explosionMat.material.color.a;
        fadeColor = Mathf.Lerp(fadeColor, 0, fadelerp);
        explosionMat.material.color = new Color(explosionMat.material.color.r, explosionMat.material.color.g, explosionMat.material.color.b, fadeColor);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 8)
            if (collision.GetComponentInParent<HealthComponent>())
                collision.GetComponentInParent<HealthComponent>().Damage(damage);
    }

}
