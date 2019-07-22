using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataManager : MonoBehaviour
{
    string path;
    string filename;

    //static to allow all scripts to access class
    public static GameData gameData = new GameData();

    PlayerController playerController;
    PlayerCamera playerCamera;
    TutorialManager tutorialManager;
    PlayerEnergyPoints playerEnergyPoints;

    /*
    info to be saved:
    - player location
    - player ability unlocks
    - player upgradable stats
    */

    private void Awake()
    {
        filename = "DemonHunterSave1.Json";

        path = Application.persistentDataPath + "/" + filename;

        playerController = FindObjectOfType<PlayerController>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();

    }

    // Start is called before the first frame update
    void Start()
    { 
        if (System.IO.File.Exists(path))
        {
            //read data
            ReadData();

            //call load data functions in each class reference
            playerController.LoadData();
            playerCamera.LoadData();
        }
        else
        {
            SavePlayerStats();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            System.IO.File.Delete(path);
            print(path);
        }
    }

    public void ReadData()
    {
        try
        {
            if (System.IO.File.Exists(path))
            {
                string contents = System.IO.File.ReadAllText(path);
                gameData = JsonUtility.FromJson<GameData>(contents);
                Debug.Log(path);
            }
            else
            {
                Debug.Log("Unable to read data file, file does not exist");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("File does not exist");
        }
    }

    public void SavePlayerStats()
    {
        //player stats (health, energy, points collected, upgrades etc.)
    }

    public void SavePlayerLocation()
    {
        gameData.playerStartLocation = playerController.startLocation;
        gameData.camStartLocation = playerCamera.startLocation;
    }

    public void SaveGameState()
    {
        //save other game states - npcs, booleans, tutorial etc.

        gameData.tutorialComplete = tutorialManager.tutorialComplete;

    }

    public void MainSaveCheckPoint()
    {
        SaveGameState();
        SavePlayerLocation();
        SavePlayerStats();

        //class info to save + true for pretty print
        string contents = JsonUtility.ToJson(gameData, true);
        //write contents to a file in path location
        System.IO.File.WriteAllText(path, contents);
        Debug.Log("Check Point Save");
    }

    public void MainSaveDeath()
    {
        //player loses points on death - so save energy points

        //class info to save + true for pretty print
        string contents = JsonUtility.ToJson(gameData, true);
        //write contents to a file in path location
        System.IO.File.WriteAllText(path, contents);
        Debug.Log("Check Point Save");
    }

    public void MainSaveGameExit()
    {
        SavePlayerStats();

        //class info to save + true for pretty print
        string contents = JsonUtility.ToJson(gameData, true);
        //write contents to a file in path location
        System.IO.File.WriteAllText(path, contents);
        Debug.Log("Check Point Save");
    }

}
