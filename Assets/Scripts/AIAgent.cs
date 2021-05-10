using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public AnimationCurve eversionCurve;
    public float eversionTargetingAngularLimit;
    [Space]
    [Range(0, 1f)]
    public float eversionSmoothness;
    [Space]
    public float pathPointSwitchRadius;

    CarController carController;

    Queue<Vector3> pathPointChain;

    Vector3 nextPathPoint;

    float pathPointSwitchSqrRadius;

    float eversionFactor;
    float evaluatedEversionFactor;

    float eversionLerpingFactor;

    void Awake()
    {
        carController = GetComponent<CarController>();

        eversionLerpingFactor = 1f - eversionSmoothness;

        pathPointSwitchSqrRadius = pathPointSwitchRadius * pathPointSwitchRadius;
    }

    void Start()
    {
        pathPointChain = LevelManager.currentLevel.GetPathPointChain(transform);

        print(pathPointChain.Count);

        nextPathPoint = pathPointChain.Dequeue();

        //ShowPoint(nextPathPoint);
    }

    void Update()
    {
        if (carController)
        {
            if (carController.IsDriving)
            {
                eversionFactor = -transform.GetPlanarAngleTo(nextPathPoint) / eversionTargetingAngularLimit;
                evaluatedEversionFactor = Mathf.Lerp(evaluatedEversionFactor, eversionCurve.Evaluate(Mathf.Abs(eversionFactor)) * Mathf.Sign(eversionFactor), eversionLerpingFactor);

                carController.SetTurnFactor(evaluatedEversionFactor);

                if (pathPointChain.Count > 0)
                {
                    if ((nextPathPoint - transform.position).sqrMagnitude < pathPointSwitchSqrRadius)
                    {
                        nextPathPoint = pathPointChain.Dequeue();

                        //ShowPoint(nextPathPoint);
                    }
                }
            }
        }
    }

    void ShowPoint(Vector3 pointPosition)
    {
        GameObject nextPointObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        nextPointObject.transform.position = pointPosition;
        nextPointObject.GetComponent<SphereCollider>().enabled = false;
    }
}
