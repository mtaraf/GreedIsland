using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float yLowerBound = 0.59f;
    public float xLowerBound;
    public float xUpperBound;
    public float yUpperBound;

    public float cameraX;
    private float cameraOffsetX = 16f;
    private float cameraOffsetY = 5.5f;

    private GameObject player;
    private Camera camaera;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camaera = GetComponent<Camera>();

        cameraX = camaera.orthographicSize * 2;
    }

    
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        CheckBoundaries();
    }

    public void SetBoundaries(float yLower, float yUpper, float xLower, float xUpper)
    {
        yLowerBound = yLower;
        yUpperBound = yUpper;
        xLowerBound = xLower;
        xUpperBound = xUpper;
    }

    void CheckBoundaries()
    {
        float rightBounds = transform.position.x + cameraOffsetX;
        float leftBounds = transform.position.x - cameraOffsetX;
        float upperBounds = transform.position.y + cameraOffsetY;
        float lowerBounds = transform.position.y - cameraOffsetY;
        if (rightBounds > xUpperBound)
        {
            transform.position = new Vector3(xUpperBound - cameraOffsetX, transform.position.y,-10);
        }
        else if (leftBounds < xLowerBound)
        {
            transform.position = new Vector3(xLowerBound + cameraOffsetX, transform.position.y,-10);
        }

        if (upperBounds > yUpperBound)
        {
            transform.position = new Vector3(transform.position.x, yUpperBound - cameraOffsetY,-10);
        }
        else if (lowerBounds < yLowerBound)
        {
            transform.position = new Vector3(transform.position.x, yLowerBound + cameraOffsetY, -10);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}
