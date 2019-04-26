using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseFocus : MonoBehaviour
{
    [SerializeField] private Vector3 screenCenter;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private float xDivider, yDivider;
    [SerializeField] private float movementRadius;
    [SerializeField] private float maxPositionValue;
    [SerializeField] private float lerpTime;
    [SerializeField] float xLerpPos;
    [SerializeField] float yLerpPos;
    private Transform mT;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        mT = transform;
    }

    // Update is called once per frame
    void Update()
    {

        mousePos = Input.mousePosition - screenCenter;
        //mousePos = mousePos.normalized;
        print(transform.localPosition.x);

        if (transform.localPosition.x < maxPositionValue /*|| transform.localPosition.x > -maxPositionValue*/)
        {
            if (mousePos.x > movementRadius)
                xLerpPos = Mathf.Lerp(transform.localPosition.x, transform.localPosition.x + mousePos.x / xDivider, lerpTime);
        }

        if (transform.localPosition.x > -maxPositionValue)
        {
            if (mousePos.x < -movementRadius)
                xLerpPos = Mathf.Lerp(transform.localPosition.x, transform.localPosition.x + mousePos.x / xDivider, lerpTime);
        }

        //if (transform.localPosition.x )

        transform.localPosition = new Vector3(xLerpPos, /*mT.position.y + mousePos.y / yDivider*/ 0 , mT.position.z);
    }
}
