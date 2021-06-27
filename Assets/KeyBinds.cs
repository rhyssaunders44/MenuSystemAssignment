using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyBinds : ScriptableObject
{
    public List<Bind> Keybinds;

    public bool GetKeyDown(string key)
    {
       Bind bind = Keybinds.Find((x) => x.name == key);
       return bind != null ? Input.GetKeyDown(bind.keycode): false;
    }

    public void ChangeKeyBind(string key, KeyCode newKeyCode)
    {
        Bind bind = Keybinds.Find((x) => x.name == key);
        bind.keycode = newKeyCode;
    }

    public KeyCode GetKeyPressed()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if(Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }
}

[System.Serializable]
public class Bind
{
    public string name;
    public KeyCode keycode;
}
