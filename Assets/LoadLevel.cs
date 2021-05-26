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
    void LoadAsynch(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);
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


    public void LoadingLevel(int sceneIndex)
    {
        LoadAsynch(sceneIndex);
    }
}