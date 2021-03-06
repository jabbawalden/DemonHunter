﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialStage;
    private TutorialManager _tutorialManager;
    private bool haveActivated;

    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerShoot _playerShoot;
    private PlayerDash _playerDash;

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();

        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _playerDash = FindObjectOfType<PlayerDash>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_tutorialManager.tutorialComplete)
        {
            if (collision.GetComponentInParent<PlayerMeleeAttack>() && tutorialStage == 1 && !haveActivated && !_playerMeleeAttack.playerMeleeEnabled)
            {
                _tutorialManager.FadeTutMelee(true);
                collision.GetComponentInParent<PlayerMeleeAttack>().playerMeleeEnabled = true;
                haveActivated = true;
            }

            if (collision.GetComponentInParent<PlayerShoot>() && tutorialStage == 2 && !haveActivated && !_playerShoot.playerShootEnabled)
            {
                _tutorialManager.FadeTutShoot(true);
                collision.GetComponentInParent<PlayerShoot>().playerShootEnabled = true;
                haveActivated = true;
            }

            if (collision.GetComponentInParent<PlayerDash>() && tutorialStage == 3 && !haveActivated && !_playerDash.playerDashEnabled)
            {
                _tutorialManager.FadeTutDash(true);
                collision.GetComponentInParent<PlayerDash>().playerDashEnabled = true;
                haveActivated = true;
            }

            if (collision.gameObject.layer == 8 && tutorialStage == 4 && !haveActivated)
            {
                _tutorialManager.tutorialComplete = true;
            }
        }
    }
}
