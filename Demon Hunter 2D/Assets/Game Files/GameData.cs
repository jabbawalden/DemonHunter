using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //player position
    public Vector3 playerStartLocation;
    public Vector3 camStartLocation;

    //player stats (health, energy, points collected, upgrades etc.)
    public int energyPoints;

    //game state
    public bool tutorialComplete;
}
