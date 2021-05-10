using System.Collections;
using System.Collections.Generic;
using Battlehub.MeshDeformer2;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager currentLevel;

    public MeshDeformer trackMeshDeformer;

    List<TrackBasePoint> baseTrackPoints;

    float trackInterpointStep = 25f;

    public float TrackLength { get; private set; }

    void Awake()
    {
        currentLevel = this;

        Initialize();
    }

    void Initialize()
    {
        baseTrackPoints = new List<TrackBasePoint>();

        TrackLength = trackMeshDeformer.GetLengthAS(0.1f);

        int trackPointsCount = Mathf.FloorToInt(TrackLength / trackInterpointStep);

        float unitDistance = 0;
        float interpointUnitStep = trackInterpointStep / TrackLength;

        for (int i = 0; i < trackPointsCount; i++)
        {
            baseTrackPoints.Add(new TrackBasePoint(trackMeshDeformer.GetPoint(unitDistance), trackMeshDeformer.GetDirection(unitDistance)));

            unitDistance += interpointUnitStep;
        }
    }

    public Queue<Vector3> GetPathPointChain(Transform callerTransform, float sideOffset = 0)
    {
        List<Vector3> points = new List<Vector3>();

        float verticalOffset = callerTransform.position.y;

        for (int i = 0; i < baseTrackPoints.Count; i++)
        {
            points.Add((sideOffset == 0 ? baseTrackPoints[i].position : baseTrackPoints[i].position + baseTrackPoints[i].crossDirection * sideOffset) + Vector3.up * verticalOffset);
        }

        int closestFrontPointIndex = 0;

        float sqrDistanceToCurrentPoint = 0;
        float minSqrDistanceToPoint = float.MaxValue;

        for (int i = 0; i < points.Count; i++)
        {
            if (Mathf.Abs(callerTransform.GetPlanarAngleTo(points[i])) < 60f)
            {
                sqrDistanceToCurrentPoint = (points[i] - callerTransform.position).sqrMagnitude;

                if (sqrDistanceToCurrentPoint < minSqrDistanceToPoint && sqrDistanceToCurrentPoint > 25f)
                {
                    closestFrontPointIndex = i;
                    minSqrDistanceToPoint = sqrDistanceToCurrentPoint;
                }
            }
        }

        points.RemoveRange(0, closestFrontPointIndex);

        return new Queue<Vector3>(points);
    }
}

public class TrackBasePoint
{
    public Vector3 position;
    public Vector3 forwardDirection;
    public Vector3 crossDirection;

    public TrackBasePoint(Vector3 position, Vector3 forwardDirection)
    {
        this.position = position;
        this.forwardDirection = forwardDirection;

        crossDirection = Vector3.Cross(Vector3.up, forwardDirection).normalized;
    }
}
