using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int integer;
    public float floatExample;
    public string stringExample;
    public int saveindex;
    public int sceneIndex;
    public bool savedGame = false;

    public void SaveValues()
    {
        Debug.Log("new Data");
        saveindex = DM.currentSaveInt;
        sceneIndex = LoadLevel.sceneIndexer;
        integer = UIManager.intToSave;
        floatExample = UIManager.floatToSave;
        stringExample = UIManager.stringToSave;
    }

}
