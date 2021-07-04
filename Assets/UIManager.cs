using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public int soundEffectsIndex;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject AnyKeyPanel;
    public GameObject MainMenu;
    public GameObject pauseEnabler;
    public GameObject SavedGames;
    public GameObject SaveGames;
    [SerializeField] private GameObject[] LoadButtons;
    [SerializeField] private GameObject AreYouSurePanel;

    public Text chatToPlayer;
    public Coroutine[] FadeCoroutine;
    public Coroutine[] FadeInCoroutine;

    public Dropdown resolution;
    public Resolution[] resolutions;

    public AudioMixerGroup[] Audio;
    public AudioSource musicSource;
    public AudioSource sfxsource;
    public bool muted;
    public AudioClip[] levelMusic;
    public AudioClip[] soundEffects;
    public Text musicVolumePercent;
    public Text sFXVolumePercent;

    public static int intToSave;
    public static float floatToSave;
    public static string stringToSave = "No Text Saved";

    public Text intDisplay;
    public Text floatDisplay;
    public Text stringDisplay;
    public InputField inGameString;
    public bool loadPanelOn;
    public bool savePanelOn;

    public void Start()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(false);

        Screen.fullScreen = true;
        resolutions = Screen.resolutions;
        resolution.ClearOptions();

        muted = false;
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
        chatToPlayer.text = "Welcome to the game";

        if (PlayerPrefs.HasKey("currentSave"))
        {
            updateText(true);
        }
        else
        {
            ResetValues();
        }
    }

    public void Update()
    {

        if (!MainMenu.activeSelf && Input.anyKeyDown)
        {
            MainMenu.SetActive(true);

            StartCoroutine(SickFade(1,0, AnyKeyPanel.GetComponent<CanvasGroup>()));
            StartCoroutine(SickFade(0,1, MainMenu.GetComponent<CanvasGroup>()));
        }

        if (pauseEnabler.activeSelf && Input.GetKeyDown(KeyCode.Escape) && !PauseMenu.activeSelf)
        {
            Pause();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && SavedGames.activeSelf)
        {
            SavedGames.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SaveGames.activeSelf)
        {
            SaveGames.SetActive(false);
        }
    }
    
    public void LoadGamePanel()
    {
        if (!loadPanelOn)
        {
            SavedGames.SetActive(true);

            for (int i = 0; i < LoadButtons.Length; i++)
            {
                if(!SaveLoad.savecheck[i])
                {
                    LoadButtons[i].SetActive(false);
                }
            }

            StartCoroutine(SickFade(0, 1, SavedGames.GetComponent<CanvasGroup>()));
            loadPanelOn = true;
        }
        else
        {
            SavedGames.SetActive(false);
            loadPanelOn = false;
        }
    }

    public void SaveGamePanel()
    {
        if (!savePanelOn)
        {
            SaveGames.SetActive(true);
            StartCoroutine(SickFade(0, 1, SaveGames.GetComponent<CanvasGroup>()));
            savePanelOn = true;
        }
        else
        {
            chatToPlayer.text = "The game has been saved!";
            SaveGames.SetActive(false);
            savePanelOn = false;
        }
    }

    public void Options()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(true);
        StartCoroutine(SickFade(0, 1, OptionsMenu.GetComponent<CanvasGroup>()));
        chatToPlayer.text = "Options?!?";
    }

    public void CloseOptions()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(false);
        chatToPlayer.text = "Options Closed!";
    }

    public void Pause()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        StartCoroutine(SickFade(0, 1, PauseMenu.GetComponent<CanvasGroup>()));
        chatToPlayer.text = "Paused!";
    }

    public void ResumeButton()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        chatToPlayer.text = "Game Resumed!";
    }

    public void ResetValues()
    {
        intToSave = 0;
        floatToSave = 0;
        stringToSave = "No Text";
    }

    public void updateText(bool loaded)
    {
        if (LoadLevel.sceneIndexer == 0)
            return;

        if (loaded)
        {
            intToSave = DM.savedint;
            Debug.Log(DM.savedint);
            floatToSave = DM.savedfloat;
            stringToSave = DM.savedstring;
        }
        if (!loaded)
        {
            ResetValues();
        }

        intDisplay.text = intToSave.ToString();
        floatDisplay.text = floatToSave.ToString();
        stringDisplay.text = stringToSave;
        
    }

    public void AreYouSure(bool turnOn)
    {
        if (turnOn)
        {
            AreYouSurePanel.SetActive(true);
            StartCoroutine(SickFade(0, 1, AreYouSurePanel.GetComponent<CanvasGroup>()));
        }
        else
        {
            AreYouSurePanel.SetActive(false);
        }

    }

    public void DeleteSave()
    {
        chatToPlayer.text = "Save Deleted";
    }

    public void RaiseInt()
    {
        intToSave++;
        intDisplay.text = intToSave.ToString();
        chatToPlayer.text = "Integer Increased!";
    }

    public void RaiseFloat()
    {
        floatToSave = floatToSave + Random.Range(0.1f, 1.0f);
        floatDisplay.text = floatToSave.ToString();
        chatToPlayer.text = "Float Increased!";
    }

    public void SetString()
    {
        if (stringToSave == "")
        {
            stringToSave = "No Text";
        }
        stringDisplay.text = inGameString.text;
        stringToSave = stringDisplay.text.ToString();

        chatToPlayer.text = "String Changed!";
    }

    public void Mute(bool isMuted)
    {
        if (isMuted)
        {
            Audio[0].audioMixer.SetFloat("isMutedVolume", -80);
        }
        else
        {
            Audio[0].audioMixer.SetFloat("isMutedVolume", 0);
        }
    }

    public void PlaySFX(int sfxIndex)
    {
        sfxsource.clip = soundEffects[sfxIndex];
        sfxsource.Play();
    }

    public void MusicVolume(float musicVolume)
    {
        Audio[0].audioMixer.SetFloat("musicVolume", musicVolume);
        musicVolumePercent.text = (Mathf.Round((musicVolume + 80) * 100f / 100)).ToString() + " %";
    }

    public void SoundEffectsVolume(float soundEffectsVolume)
    {
        Audio[1].audioMixer.SetFloat("soundEffectsVolume", soundEffectsVolume);
        sFXVolumePercent.text = (Mathf.Round((soundEffectsVolume + 80) * 100f / 100)).ToString() + " %";
    }

    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public IEnumerator SickFade(float start, float end, CanvasGroup selectedObject)
    {
        float emarkedTime = Time.unscaledTime;
        float current;
        float difference = 0.01f;
        current = start;

        while (Mathf.Abs(current - end) > difference)
        {
            current = Mathf.Lerp(start, end, (Time.unscaledTime - emarkedTime) /  1);
            selectedObject.alpha = current;

            yield return 0;
        }
    }

    public void Quit()
    {
        soundEffectsIndex = 0;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}