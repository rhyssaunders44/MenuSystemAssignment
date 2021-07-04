using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    static public SaveLoad instance;
    public static bool[] savecheck = new bool[6];
    [SerializeField] private string filePath;


    private void Awake()
    {
        
        filePath = Application.persistentDataPath + "/save";

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("back2lumby");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < savecheck.Length; i++)
        {
            if (File.Exists(filePath + i + ".data"))
            {
                savecheck[i] = true;
            }
            else
            {
                savecheck[i] = false;
            }
        }
    }

    //save data to file
    public void SaveGame(GameData saveData, int saveIndex)
    {
        Debug.Log("saving");
        FileStream dataStream = new FileStream(filePath + saveIndex.ToString() + ".data", FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData);
        dataStream.Close();
        savecheck[saveIndex] = true;
    }
    
    //load data from files
    public GameData LoadGame(int loadIndex)
    {

        if (File.Exists(filePath + loadIndex.ToString() + ".data"))
        {
            Debug.Log(filePath + loadIndex.ToString() + ".data");
            Debug.Log("loading");
            DM.runload = true;
            FileStream dataStream = new FileStream(filePath + loadIndex.ToString() + ".data", FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;

            dataStream.Close();
            return saveData;
        }
        else
        {

            DM.runload = false;
            return null;
        }
    }
}
