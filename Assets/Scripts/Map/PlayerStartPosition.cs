using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPosition : MonoBehaviour
{
    private GameObject player;
    private MapBoundaries mapBoundaries;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mapBoundaries = GetComponent<MapBoundaries>();

        if (mapBoundaries.startPositions.Count == 1)
        {
            player.transform.position = mapBoundaries.startPositions[0];
        }
        else if (mapBoundaries.startPositions.Count == 2)
        {
            if (MainManager.instance != null)
            {
                if (MainManager.instance.spawnPortalPosition.Equals("right"))
                {
                    player.transform.position = mapBoundaries.startPositions[1];
                }
                else
                {
                    player.transform.position = mapBoundaries.startPositions[0];
                }
            }
        }
        else if (mapBoundaries.startPositions.Count == 3)
        {
            if (MainManager.instance != null)
            {
                if (MainManager.instance.spawnPortalPosition.Equals("right"))
                {
                    player.transform.position = mapBoundaries.startPositions[2];
                }
                else if (MainManager.instance.spawnPortalPosition.Equals("middle"))
                {
                    player.transform.position = mapBoundaries.startPositions[1];
                }
                else
                {
                    player.transform.position = mapBoundaries.startPositions[0];
                }
            }
        }
    }
}
