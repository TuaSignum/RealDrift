using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public Transform steeringWheelTransform;
    public float steeringWheelAngularAmplitude;
    [Space]
    public float eversionSlideLimit;
    public float reversionLerpingFactor;

    CarController carController;
    CameraController cameraController;

    float baseTouchCoordX;
    float slideOffsetX;

    float eversionFactor;

    bool isTouchPresent;

    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    void Start()
    {
        cameraController = CameraController.instance;
    }

    void FixedUpdate()
    {
        if (carController)
        {
            if (carController.IsDriving)
            {
                if (Input.GetMouseButton(0))
                {
                    if (isTouchPresent)
                    {
                        slideOffsetX = Input.mousePosition.x - baseTouchCoordX;

                        eversionFactor = Mathf.Clamp(slideOffsetX / eversionSlideLimit, -1f, 1f);
                    }
                    else
                    {
                        isTouchPresent = true;

                        baseTouchCoordX = Input.mousePosition.x;
                    }
                }
                else
                {
                    isTouchPresent = false;

                    eversionFactor = Mathf.Lerp(eversionFactor, 0, reversionLerpingFactor);
                }

                steeringWheelTransform.localEulerAngles = new Vector3(0, 0, -steeringWheelAngularAmplitude * eversionFactor);

                carController.SetTurnFactor(eversionFactor);
                cameraController.SetYaw(eversionFactor);
            }
        }
    }
}
