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
    //private Transform mT;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        //mT = transform;
    }

    void LateUpdate()
    {

        mousePos = Input.mousePosition - screenCenter;

        if (transform.localPosition.x < maxPositionValue)
        {
            if (mousePos.x > movementRadius)
                xLerpPos = Mathf.Lerp(transform.localPosition.x, mousePos.x / xDivider, lerpTime);
        }

        if (transform.localPosition.x > -maxPositionValue)
        {
            if (mousePos.x < -movementRadius)
                xLerpPos = Mathf.Lerp(transform.localPosition.x, mousePos.x / xDivider, lerpTime);
        }

        if (transform.localPosition.y < maxPositionValue)
        {
            if (mousePos.y > movementRadius)
                yLerpPos = Mathf.Lerp(transform.localPosition.y, mousePos.y / yDivider, lerpTime);
        }

        if (transform.localPosition.y > -maxPositionValue)
        {
            if (mousePos.y < -movementRadius)
                yLerpPos = Mathf.Lerp(transform.localPosition.y, mousePos.y / yDivider, lerpTime);
        }

        transform.localPosition = new Vector3(xLerpPos, yLerpPos, 0);
    }
}
