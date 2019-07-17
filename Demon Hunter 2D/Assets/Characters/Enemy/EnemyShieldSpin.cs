using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    //[SerializeField] private float sLerp;
    //public Quaternion newRotation;

    //[SerializeField] private float turnRate;
    //private float newTime;
    //bool canTurn;
    //bool canSetTarget;
    //public float targetRotation;

    // Update is called once per frame

    private void Start()
    {
        //set rotated starting position
        spinSpeed = Random.Range(spinSpeed, spinSpeed * 1.5f);
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, spinSpeed);
    }


}
