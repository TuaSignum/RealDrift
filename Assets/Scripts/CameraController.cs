using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;
    [Range(0, 1f)]
    public float chasingFactor;
    [Space]
    public float globalYawAmplitude;
    public float localYawAmplitude;
    [Space]
    public bool inheritDirection;

    Camera mainCamera;

    Vector3 baseOffset;
    Vector3 baseLocalEulers;

    float forwardOffsetMagnitude;
    float yawIncrement;

    void Awake()
    {
        instance = this;

        mainCamera = Camera.main;

        baseLocalEulers = mainCamera.transform.localEulerAngles;

        if (target)
        {
            baseOffset = transform.position - target.position;
            forwardOffsetMagnitude = baseOffset.magnitude;
        }
    }

    void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + target.forward * forwardOffsetMagnitude, chasingFactor);

            if (inheritDirection)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y + yawIncrement, chasingFactor), transform.eulerAngles.z);
            }
        }
    }

    public void SetYaw(float eversionFactor)
    {
        yawIncrement = Mathf.LerpAngle(yawIncrement, eversionFactor * globalYawAmplitude, chasingFactor);

        mainCamera.transform.localEulerAngles = new Vector3(baseLocalEulers.x, Mathf.LerpAngle(mainCamera.transform.localEulerAngles.y, eversionFactor * localYawAmplitude, chasingFactor), baseLocalEulers.z);
    }
}
