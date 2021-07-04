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
    public static int sceneIndexer;

    bool isLoading = false;
    AsyncOperation operation;

    public void Start()
    {
        sceneIndexer = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadAsynch()
    {
        operation = SceneManager.LoadSceneAsync(DM.currentSceneInt);
        loadingScreen.SetActive(true);
        isLoading = true;
    }

    public void LoadGame()
    {
        if (DM.runload)
            LoadAsynch();
    }

    public void Continue()
    {
        if (PlayerPrefs.HasKey("currentSave"))
            LoadAsynch();
    }

    public void ToMain()
    {
        operation = SceneManager.LoadSceneAsync(0);
    }

    public void NewGame()
    {
        operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        isLoading = true;
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