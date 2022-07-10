using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables / References
    public Transform Ball;

    private float cameraFollowSpeed = 1.5f;

    private Vector3 offsetPosition;
    #endregion

    void Update()
    {
        offsetPosition = Ball.transform.position;
        offsetPosition.y = 12f;
        offsetPosition.z = 7f;

        transform.position = Vector3.Lerp(transform.position, offsetPosition, cameraFollowSpeed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5f, 5f),transform.position.y, transform.position.z);
    }
}
