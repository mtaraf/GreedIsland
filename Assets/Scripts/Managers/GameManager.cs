using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject map;
    public GameObject mainCamera;

    private PlayerController playerController;
    private MapBoundaries mapBoundaries;
    private CameraMovement cameraMovement;
    private GameObject [] portals;
    private bool isGameActive;

    public bool GetIsGameActive()
    {
        return isGameActive;
    }

    public void SetIsGameActive(bool gameActive)
    {
        isGameActive = gameActive;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        map = GameObject.FindGameObjectWithTag("Map");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerController = player.GetComponent<PlayerController>();
        mapBoundaries = map.GetComponent<MapBoundaries>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        portals = GameObject.FindGameObjectsWithTag("Portal");

        MainManager.instance.isNewGame = false;
        MainManager.instance.isGameActive = true;

        //StartCoroutine(SpawnEnemies());
        SetCameraBoundaries();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //pause menu
        }


        Tuple<bool, int> playerNextToPortal = CheckIfNextToPortal();
        if (playerNextToPortal.Item1 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            TransitionToNextScene(playerNextToPortal.Item2);
        }
    }

    private Tuple<bool,int> CheckIfNextToPortal()
    {
        float portalAreaBufferX = 0.2f;
        float portalAreaBufferY = 0.5f;
        Vector3 playerPosition = player.transform.position;
        if (portals.Length > 0)
        {
            for (int i = 0; i < portals.Length; i++)
            {
                Vector3 portalPosition = portals[i].transform.position;
                if ((playerPosition.x < portalPosition.x + portalAreaBufferX) && (playerPosition.x > portalPosition.x - portalAreaBufferX) &&
                    (playerPosition.y < portalPosition.y + portalAreaBufferY) && (playerPosition.y > portalPosition.y - portalAreaBufferY)) //checks if player in on portal
                {
                    Portal portal = portals[i].GetComponent<Portal>();

                    //check which portal player is standing on
                    if (portal.location.Item1.Equals("right"))
                    {
                        MainManager.instance.spawnPortalPosition = portal.location.Item2;
                        return new Tuple<bool,int>(true,1);
                    }
                    else if (portal.location.Item1.Equals("left"))
                    {
                        MainManager.instance.spawnPortalPosition = portal.location.Item2;
                        return new Tuple<bool, int>(true, -1);
                    }
                    else
                    {
                        MainManager.instance.spawnPortalPosition = portal.location.Item2;
                        return new Tuple<bool, int>(true, 2);
                    }
                }
            }
        }
        return new Tuple<bool, int>(false, 0); //if player is not on portal, return false
    }

    private void TransitionToNextScene(int scenePath)
    {
        Debug.Log("a");
        if (scenePath == -1) //left portal
        {
            //Transition to next scene
            int nextScene = mapBoundaries.left;
            //Save Data in MainManager
            playerController.SavePlayerDataBetweenScenes();
            MainManager.instance.sceneNumber = nextScene;
            SceneManager.LoadScene(nextScene);
        }
        else if (scenePath == 1) //right portal
        {
            //Transition to next scene
            int nextScene = mapBoundaries.right;
            //Save Data in MainManager
            playerController.SavePlayerDataBetweenScenes();
            MainManager.instance.sceneNumber = nextScene;
            SceneManager.LoadScene(nextScene);
        }
        else if (scenePath == 2) //middle portal
        {
            //Transition to next scene
            int nextScene = mapBoundaries.middle;
            //Save Data in MainManager
            playerController.SavePlayerDataBetweenScenes();
            MainManager.instance.sceneNumber = nextScene;
            SceneManager.LoadScene(nextScene);
        }
    }

    private void SetCameraBoundaries()
    {
        cameraMovement.SetBoundaries(mapBoundaries.yLowerBound, mapBoundaries.yUpperBound, mapBoundaries.xLowerBound, mapBoundaries.xUpperBound);
    }


    public void ResumeGame()
    {
        //pauseMenu.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        isGameActive = false;

        // save player data
        if (playerController != null)
        {
            playerController.SavePlayerData();
        }
        SceneManager.LoadScene(0);
    }


    /*
    IEnumerator SpawnEnemies()
    {
        while (isGameActive)
        {
            if (numEnemies < maxEnemies)
            {
                Vector3 spawnPosition = GenerateSpawnPosition();
                Instantiate(slimePrefab, spawnPosition, player.transform.rotation);
                numEnemies++;
            }
            yield return new WaitForSeconds(5);
        }
    }
    */
}
