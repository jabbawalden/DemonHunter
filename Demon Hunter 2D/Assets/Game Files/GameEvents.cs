using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{

    public static event Action EventSaveCheckPoint;
    public static event Action EventSaveDeath;

    //sent event to notify exiting game
    public static event Action EventGameExitMain;
    //events called based on whether in town or not
    public static event Action EventSaveTownExit;
    public static event Action EventSaveNormalExit;

    public static event Action EventPlayerDeath;
    public static event Action EventLoadLastSave;
    public static event Action EventQuitToMenu;


    public static void ReportSaveCheckPoint()
    {
        EventSaveCheckPoint?.Invoke();
    }

    public static void ReportSaveDeath()
    {
        EventSaveDeath?.Invoke();
    }

    public static void ReportGameExitMain()
    {
        EventGameExitMain?.Invoke();
    }
    public static void ReportSaveFileExitTown()
    {
        EventSaveTownExit?.Invoke();
    }

    public static void ReportSaveFileExitNormal()
    {
        EventSaveNormalExit?.Invoke();
    }
    public static void ReportPlayerDeath()
    {
        EventPlayerDeath?.Invoke();
    }

    public static void ReportLoadLastSave()
    {
        EventLoadLastSave?.Invoke();
    }

    public static void ReportQuitToMenu()
    {
        EventQuitToMenu?.Invoke();
    }

}
