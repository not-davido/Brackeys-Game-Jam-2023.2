using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //public int levelNumber;
    [SerializeField] Transform startingPoint;
    //[SerializeField] bool hasEndingPoint;
    [SerializeField] Transform endingPoint;

    public Vector2 StartingPosition => startingPoint.position;
    public Vector2 EndingPosition {
        get {
            if (endingPoint != null) {
                return endingPoint.position;
            } else {
                Debug.LogWarning("The level does not have a ending position.");
                return startingPoint.position;
            }
        }
    }
}
