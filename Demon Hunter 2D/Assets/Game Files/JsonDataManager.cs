using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataManager : MonoBehaviour
{
    string filePath;
    string fileName;

    //static to allow all scripts to access class
    public static GameData gameData = new GameData();

    private PlayerController _playerController;
    private PlayerCamera _playerCamera;
    private TutorialManager _tutorialManager;
    private PlayerEnergyPoints _playerEnergyPoints;
    private PlayerMeleeAttack _playerMeleeAttack;
    private PlayerShoot _playerShoot;
    private PlayerDash _playerDash;
    private PlayerEnergy _playerEnergy;
    private GameManager _gameManager;
    private PlayerUpgradesManager _playerUpgradesManager;
    private NPCManager _npcManager;

    private void OnEnable()
    {
        GameEvents.EventSaveCheckPoint += WriteDataFiles;
        GameEvents.EventSaveCheckPoint += CheckPointSave;

        GameEvents.EventSaveDeath += WriteDataFiles;
        GameEvents.EventSaveDeath += DeathSave;

        GameEvents.EventSaveTownExit += WriteDataFiles;
        GameEvents.EventSaveTownExit += TownSave;

        GameEvents.EventSaveNormalExit += WriteDataFiles;
        GameEvents.EventSaveNormalExit += GameExitSave;
    }

    private void Awake()
    {
        fileName = "DemonHunterSave1.Json";

        filePath = Application.persistentDataPath + "/" + fileName;

        _playerController = FindObjectOfType<PlayerController>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _tutorialManager = FindObjectOfType<TutorialManager>();
        _playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();
        _playerMeleeAttack = FindObjectOfType<PlayerMeleeAttack>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _playerDash = FindObjectOfType<PlayerDash>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _gameManager = FindObjectOfType<GameManager>();
        _playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        _npcManager = FindObjectOfType<NPCManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.File.Exists(filePath))
        {
            //read data
            ReadData();
            //call load data functions in each class reference
            _playerController.LoadData();
            _playerCamera.LoadData();
            _tutorialManager.LoadData();
            _playerEnergyPoints.LoadData();
            _playerMeleeAttack.LoadData();
            _playerShoot.LoadData();
            _playerDash.LoadData();
            _gameManager.LoadData();
            _playerUpgradesManager.LoadData();
            _npcManager.LoadData();
        }
        else
        {
            //begin tutorial
            _tutorialManager.FadeTutMove(true);
            _playerUpgradesManager.StartGameInitiate();
            _playerController.StartGameInitiate();
            StartGameSave();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            System.IO.File.Delete(filePath);
            print(filePath);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (/*!_playerController.inCombat || */_gameManager.playerInTown)
                GameEvents.ReportSaveFileExitTown();
            else
                Debug.Log("Cannot save outside of town area");
        }

    }

    public void ReadData()
    {
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                string contents = System.IO.File.ReadAllText(filePath);
                gameData = JsonUtility.FromJson<GameData>(contents);
                Debug.Log(filePath);
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
        gameData.playerHealth = _playerController.playerHealthComp.CurrentHealth;
        gameData.playerMaxHealth = _playerController.playerHealthComp.maxHealth;
        gameData.defaultMovementSpeed = _playerController.defaultMovementSpeed;
        gameData.playerMaxEnergy = _playerEnergy.playerMaxEnergy;
        gameData.canHealthRegen = _playerController.playerHealthComp.canHealthRegen;
        gameData.damageMultiplier = _playerController.DamageMultiplier;

        gameData.speedUpgradeAmount = _playerUpgradesManager.SpeedUpgradeAmount;
        gameData.healthUpgradeAmount = _playerUpgradesManager.HealthUpgradeAmount;
        gameData.energyUpgradeAmount = _playerUpgradesManager.EnergyUpgradeAmount;
        gameData.speedUpgradeCost = _playerUpgradesManager.SpeedUpgradeCost;
        gameData.healthUpgradeCost = _playerUpgradesManager.HealthUpgradeCost;
        gameData.energyUpgradeCost = _playerUpgradesManager.EnergyUpgradeCost;
        gameData.damageMultiplierUpgradeCost = _playerUpgradesManager.DamageMultiplierUpgradeCost;

        gameData.speedUpgradesLeft = _playerUpgradesManager.SpeedUpgradesLeft;
        gameData.healthUpgradesLeft = _playerUpgradesManager.HealthUpgradesLeft;
        gameData.energyUpgradesLeft = _playerUpgradesManager.EnergyUpgradesLeft;
        gameData.damageUpgradesLeft = _playerUpgradesManager.DamageUpgradesLeft;

        print("speed upgrade amount is: " + gameData.speedUpgradeAmount);
    }

    public void SaveTutorialState()
    {
        gameData.meleeEnabled = _playerMeleeAttack.playerMeleeEnabled;
        gameData.shootEnabled = _playerShoot.playerShootEnabled;
        gameData.dashEnabled = _playerDash.playerDashEnabled;
        gameData.gameIntroMove = _gameManager.gameIntroMove;
        gameData.gameIntroMelee = _gameManager.gameIntroMelee;
        gameData.gameIntroShoot = _gameManager.gameIntroShoot;
        gameData.gameIntroDash = _gameManager.gameIntroDash;
    }

    public void SavePlayerEnergyPoints()
    {
        gameData.energyPoints = _playerEnergyPoints.energyPoints;
    }

    public void SavePlayerLocationCheckPoint()
    {
        gameData.playerStartLocation = _playerController.startLocation;
        gameData.camStartLocation = _playerCamera.startLocation;
    }

    public void SavePlayerLocationWorld()
    {
        gameData.playerStartLocation = _playerController.transform.position;
        gameData.camStartLocation = _playerCamera.transform.position;
        print("world location saved");
    }

    public void SaveGameState()
    {
        //save other game states - npcs, booleans, tutorial etc.

        gameData.tutorialComplete = _tutorialManager.tutorialComplete;
    }

    public void SaveNPCState()
    {
        gameData.oldWomanConv = _npcManager.oldWomanConv;
        gameData.shopConv = _npcManager.shopConv;
        gameData.upgradeConv = _npcManager.upgradeConv;
    }

    public void StartGameSave()
    {
        SaveGameState();
        SavePlayerLocationWorld();
        SavePlayerStats();
        SavePlayerEnergyPoints();
        SaveTutorialState();
        //remove later
        SaveNPCState();

        Debug.Log("Start Game Save");
    }

    public void CheckPointSave()
    {
        SaveGameState();
        SavePlayerLocationCheckPoint();
        SavePlayerStats();
        SavePlayerEnergyPoints();
        SaveTutorialState();
        //remove later
        SaveNPCState();

        Debug.Log("Check Point Save");
    }

    public void DeathSave()
    {
        SavePlayerEnergyPoints();
        SaveTutorialState();
        //remove later
        SaveNPCState();

        Debug.Log("Death Save");
    }

    public void TownSave()
    {
        SavePlayerStats();
        SavePlayerLocationWorld();
        SavePlayerEnergyPoints();
        SaveTutorialState();
        SaveNPCState();

        Debug.Log("Town Save Exit");
    }

    public void GameExitSave()
    {
        SavePlayerStats();
        SavePlayerEnergyPoints();
        SaveTutorialState();
        SaveNPCState();

        Debug.Log("Main Game Exit Save");
    }

    public void WriteDataFiles()
    {        //class info to save + true for pretty print
        string contents = JsonUtility.ToJson(gameData, true);
        //write contents to a file in path location
        System.IO.File.WriteAllText(filePath, contents);
    }

}
