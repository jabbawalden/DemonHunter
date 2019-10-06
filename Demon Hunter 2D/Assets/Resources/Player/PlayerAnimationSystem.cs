using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;


    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    private void Idle()
    {

    }

    private void Run()
    {

    }

    private void Shoot()
    {

    }

    private void Siphon()
    {

    }

    private void Dash()
    {

    }
}
