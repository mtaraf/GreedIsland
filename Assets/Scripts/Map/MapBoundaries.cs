using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    public float yLowerBound = 0.59f;
    public float xLowerBound;
    public float xUpperBound;
    public float yUpperBound;
    public List<Vector3> startPositions;
    public int left, middle, right; // scene numbers for each portal, -1 if no portal
}
