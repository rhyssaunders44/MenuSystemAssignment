using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM : MonoBehaviour
{
    GameData saveData = new GameData();
    public static int currentSaveInt;
    public static int currentSceneInt;
    public static int savedint;
    public static float savedfloat;
    public static string savedstring;

    public static bool runload;

    public void Start()
    {
        SaveCheck();

        if(SaveLoad.savecheck[currentSaveInt])
        {
            RunLoad(currentSaveInt);
            currentSaveInt = saveData.saveindex;
            savedint = saveData.integer;
            savedfloat = saveData.floatExample;
            savedstring = saveData.stringExample;
            Debug.Log(savedint);
        }
    }

    // save data instance
    public void RunSave(int saveIndex)
    {
        saveData.savedGame = true;
        saveData.SaveValues();
        SaveLoad.instance.SaveGame(saveData, saveIndex);
        PlayerPrefs.SetInt("currentSave", saveIndex);
        currentSaveInt = saveIndex;
    }

    //load and update scores
    public void RunLoad(int loadIndex)
    {
        PlayerPrefs.SetInt("currentSave", loadIndex);
        saveData = SaveLoad.instance.LoadGame(loadIndex);
        Debug.Log("DM load");
        runload = true;
        if (saveData != null)
        {
            currentSceneInt = saveData.sceneIndex;
        }
    }

    public void Continue()
    {
        if(PlayerPrefs.HasKey("currentSave"))
            saveData = SaveLoad.instance.LoadGame(PlayerPrefs.GetInt("currentSave"));
    }

    private void SaveCheck()
    {
        if (PlayerPrefs.HasKey("currentSave"))
        {
            currentSaveInt = saveData.saveindex;
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("currentSave");
        Debug.Log(PlayerPrefs.HasKey("currentSave"));
    }
}
