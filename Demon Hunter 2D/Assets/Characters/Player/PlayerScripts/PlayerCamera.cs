using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;
    [SerializeField] float sX, sY;
    private Vector2 velocity;
    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
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

    public void CameraShake()
    {
        StartCoroutine(Shake(0.5f, 0.5f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            magnitude *= 0.975f;
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            //wait until the next frame starts before updating
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
