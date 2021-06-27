using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image progressBar;
    public Text progressText;

    bool isLoading = false;
    AsyncOperation operation;

    public static bool loadDataAvailable;

    public void LoadAsynch(int sceneIndex)
    {
        var loadGameData = PlayerPrefs.HasKey("SavedInteger") ? loadDataAvailable = true : loadDataAvailable = false;
        if (loadDataAvailable == true)
        {
            operation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingScreen.SetActive(true);
            isLoading = true;
        }
    }
   
    public void NewGame(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        isLoading = true;
        loadDataAvailable = false;
    }

    private void Update()
    {
        if (isLoading && operation != null)
        { 
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.fillAmount = progress;
            progressText.text = "LOADING: " + (progress * 100).ToString() + " %";
        }
    }
}