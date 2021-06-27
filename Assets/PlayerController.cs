using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class KeyBind
    {
        public string keyName;
        public Text keyDisplayText;
        public string defaultKey;
    }

    [SerializeField] private Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>();

    public KeyBind[] defaultSetup;
    public GameObject currentKey;
    public Color32 changedKey = Color.white;
    public Color32 selectedKey = Color.cyan;
    [SerializeField] private int arrayIndex;


    private void Start()
    {
        for (int i = 0; i < defaultSetup.Length; i++)
        {
            keyBinds.Add(defaultSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(defaultSetup[i].keyName, defaultSetup[i].defaultKey)));
            defaultSetup[i].keyDisplayText.text = keyBinds[defaultSetup[i].keyName].ToString();
        }
    }

    public void SaveKeys()
    {
        foreach(var key in keyBinds)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void ChangeKey(GameObject clickedKey)
    {
        currentKey = clickedKey;

        if(clickedKey != null)
        {
            currentKey.GetComponent<Image>().color = selectedKey;
        }
        
    }

    public void OnGUI()
    {
        string newKey = "";
        Event e = Event.current;

        if(currentKey != null)
        {
            if (e.isKey)
            {
                newKey = e.keyCode.ToString();
 
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }

            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }

            if (newKey != "")
            {

                keyBinds[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                defaultSetup[arrayIndex].keyDisplayText.text = newKey.ToString();
                currentKey.GetComponent<Image>().color = changedKey;
                currentKey = null;
            }
        }
    }

    public void Keyint(int select)
    {
        arrayIndex = select;
    }
}
