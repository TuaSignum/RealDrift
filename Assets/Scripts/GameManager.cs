using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gravityMagnitude;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void OnValidate()
    {
        Physics.gravity = new Vector3(0, -gravityMagnitude, 0);
    }
}
