using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private GameObject map;
    private MapBoundaries mapBoundaries;

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        mapBoundaries = map.GetComponent<MapBoundaries>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < mapBoundaries.yLowerBound - 10 || transform.position.y > mapBoundaries.yUpperBound + 10 || transform.position.x < mapBoundaries.xLowerBound - 10 || transform.position.x > mapBoundaries.xUpperBound + 10)
        {
            Destroy(gameObject);
        }
    }
}
