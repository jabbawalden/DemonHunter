using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTracker : MonoBehaviour
{
    string fileName1 = "DemonHunterSave1.Json";
    string fileName2 = "DemonHunterSave2.Json";
    string fileName3 = "DemonHunterSave3.Json";
    private MenuManager menuManager;

    //to check for each file name's existence
    string path1;
    string path2;
    string path3;

    private string chosenFileName;
    public string ChosenFileName
    {
        get { return chosenFileName; }
        private set { chosenFileName = value; }
    }

    private static GameTracker instance;

    private void Awake()
    {
        //Singleton code
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        menuManager = FindObjectOfType<MenuManager>();

        path1 = Application.persistentDataPath + "/" + fileName1;
        path2 = Application.persistentDataPath + "/" + fileName2;
        path3 = Application.persistentDataPath + "/" + fileName3;

        CheckFileExistence();
    }

    private void Start()
    {

    }

    private void CheckFileExistence()
    {
        if (System.IO.File.Exists(path1))
            if (menuManager)
                menuManager.ChangeText(1);
        else
            print("no file found");

        if (System.IO.File.Exists(path2))
            if (menuManager)
                menuManager.ChangeText(2);
        else
            print("no file found");

        if (System.IO.File.Exists(path3))
            if (menuManager)
                menuManager.ChangeText(3);
        else
            print("no file found");
    }

    public void LoadPath(int index)
    {
        //call from button as delegate then set index and set chosen file name to 1,2,3 or 4
        switch (index)
        {
            case 1:
                ChosenFileName = fileName1;
                break;
            case 2:
                ChosenFileName = fileName2;
                break;
            case 3:
                ChosenFileName = fileName3;
                break;
        }

        SceneManager.LoadScene(1);
    }


}
