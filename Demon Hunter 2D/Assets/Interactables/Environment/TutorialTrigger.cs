using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialStage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerMeleeAttack>() && tutorialStage == 1)
            collision.GetComponentInParent<PlayerMeleeAttack>().playerMeleeEnabled = true;

        if (collision.GetComponentInParent<PlayerShoot>() && tutorialStage == 2)
            collision.GetComponentInParent<PlayerShoot>().playerShootEnabled = true;

        if (collision.GetComponentInParent<PlayerDash>() && tutorialStage == 3)
            collision.GetComponentInParent<PlayerDash>().playerDashEnabled = true;
    }
}
