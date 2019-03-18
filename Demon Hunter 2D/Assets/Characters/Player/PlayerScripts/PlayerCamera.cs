using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;
    [SerializeField] float sX, sY;
    private Vector2 velocity;

    private void Awake()
    {
        target = GameObject.Find("PlayerController").transform;
    }

    private void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SmoothFollowPlayer();
    }

    private void SmoothFollowPlayer()
    {
        float xPos = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, sX);
        float yPos = Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, sY);

        if (target != null)
            transform.position = new Vector3(xPos, yPos, transform.position.z);

    }
}
