using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float sX, sY;
    private Vector2 velocity;
    private Vector3 originalPos;
    public Vector3 startLocation;
    [SerializeField] private Transform cam;

    private void Awake()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        originalPos = cam.localPosition;
    }

    private void Start()
    {

    }
    // Update is called once per frame

    void FixedUpdate()
    {
        SmoothFollowPlayer();
    }

    public void LoadData()
    {
        startLocation = JsonDataManager.gameData.camStartLocation;
        transform.position = startLocation;
    }

    private void SmoothFollowPlayer()
    {
        float xPos = Mathf.SmoothDamp(transform.localPosition.x, target.position.x, ref velocity.x, sX);
        float yPos = Mathf.SmoothDamp(transform.localPosition.y, target.position.y, ref velocity.y, sY);

        if (target != null)
            transform.position = new Vector3(xPos, yPos, transform.position.z);

    }

    public void CameraShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude)); 
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            magnitude *= 0.975f;
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            cam.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            //wait until the next frame starts before updating
            yield return null;
        }

        cam.localPosition = originalPos;
    }
}
