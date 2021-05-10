using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform yawingContainer;
    [Space]
    public Transform leftFrontWheel;
    public Transform rightFrontWheel;
    [Space]
    public float speedLimit;
    public float acceleration;
    [Range(0, 1f)]
    public float motionSmoothness;
    [Space]
    [Range(0, 1f)]
    public float accelerationByTurningThreshold;
    [Space]
    public float angularSpeedLimit;
    [Range(0, 1f)]
    public float angularSpeedSmoothness;
    public AnimationCurve angularSpeedCurve;
    [Space]
    public AnimationCurve speedByTurningCurve;
    [Space]
    public float bodyYawLimit;

    Rigidbody rb;

    Vector3 externalImpulse;

    float accelerationDelta;

    float actualFrontSpeed;
    float actualAngularSpeed;

    float targetFrontSpeed;
    float minFrontSpeed;

    float actualYawAngle;

    float speedLerpingFactor;
    float angularSpeedLerpingFactor;

    float turnFactor;
    float absTurnFactor;
    float evaluatedTurnFactor;
    float absEvaluatedTurnFactor;

    bool isPlayer;

    public bool IsDriving { get; private set; }

    void Awake()
    {
        isPlayer = GetComponent<PlayerAgent>();

        rb = GetComponent<Rigidbody>();

        minFrontSpeed = speedLimit * speedByTurningCurve.Evaluate(1f);

        accelerationDelta = speedLimit * Time.fixedDeltaTime;

        angularSpeedLerpingFactor = 1f - angularSpeedSmoothness;
    }

    void Start()
    {
        IsDriving = true;
    }

    void FixedUpdate()
    {
        if (IsDriving)
        {
            if (targetFrontSpeed < speedLimit && absTurnFactor < accelerationByTurningThreshold)
            {
                targetFrontSpeed += accelerationDelta;
            }

            actualFrontSpeed = Mathf.Lerp(actualFrontSpeed, targetFrontSpeed * speedByTurningCurve.Evaluate(absTurnFactor), motionSmoothness);

            actualYawAngle = Mathf.Lerp(actualYawAngle, bodyYawLimit * turnFactor, angularSpeedLerpingFactor);
            actualAngularSpeed = Mathf.Lerp(actualAngularSpeed, angularSpeedLimit * evaluatedTurnFactor, angularSpeedLerpingFactor);

            yawingContainer.localEulerAngles = new Vector3(0, actualYawAngle, 0);

            leftFrontWheel.localEulerAngles = new Vector3(0, -actualYawAngle, 0);
            rightFrontWheel.localEulerAngles = new Vector3(0, -actualYawAngle, 0);

            rb.AddForce(transform.forward * acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, actualFrontSpeed);
            rb.angularVelocity = transform.up * actualAngularSpeed;

            if (externalImpulse.sqrMagnitude > 0)
            {
                externalImpulse = Vector3.Lerp(externalImpulse, Vector3.zero, 0.3f);
            }

            //print($"Actual: {actualFrontSpeed} // Target: {targetFrontSpeed * speedByTurningCurve.Evaluate(absTurnFactor)} // Max: {speedLimit}");
        }
    }

    public void SetTurnFactor(float value)
    {
        turnFactor = value;
        absTurnFactor = Mathf.Abs(value);

        absEvaluatedTurnFactor = angularSpeedCurve.Evaluate(absTurnFactor);
        evaluatedTurnFactor = absEvaluatedTurnFactor * Mathf.Sign(value);
    }
}
