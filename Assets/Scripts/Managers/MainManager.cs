using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public float playerAttack;
    public float playerDefense;
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public Vector3 playerPosition;
    public float playerJump;
    public float playerSpeed;

    public int sceneNumber = 2;
    public string spawnPortalPosition = "right";

    public bool loadLocal = false;
    public bool isNewGame = false; 
    public bool isGameActive = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public float maxHealth;
        public float currentHealth;
        public float attack;
        public float defense;
        public float playerSpeed;
        public float playerJump;
        public Vector3 position;
        public int currentScene;
        public bool isNewGame;
    }

    public void SavePlayerData()
    {
        SaveData saveData = new SaveData();
        saveData.attack = playerAttack;
        saveData.defense = playerDefense;
        saveData.maxHealth = playerMaxHealth;
        saveData.currentHealth = playerCurrentHealth;
        saveData.playerJump = playerJump;
        saveData.playerSpeed = playerSpeed;
        saveData.position = playerPosition;
        saveData.currentScene = sceneNumber;
        saveData.isNewGame = isNewGame;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/playerSaveFile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerSaveFile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            playerMaxHealth = saveData.maxHealth;
            playerCurrentHealth = saveData.currentHealth;
            playerDefense = saveData.defense;
            playerPosition = saveData.position;
            playerAttack = saveData.attack;
            playerSpeed = saveData.playerSpeed;
            playerJump = saveData.playerJump;
            sceneNumber = saveData.currentScene;
            isNewGame = saveData.isNewGame;
        }
    }

}
