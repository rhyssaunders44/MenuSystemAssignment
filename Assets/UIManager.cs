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

    int intToSave;
    float floatToSave;
    string stringToSave = "No Text Saved";

    public Text intDisplay;
    public Text floatDisplay;
    public Text strongDisplay;
    public InputField inGameString;



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
    }

    public void Update()
    {
        if (MainMenu.activeSelf == false && Input.anyKeyDown)
        {
            MainMenu.SetActive(true);

            StartCoroutine(SickFade(1,0, AnyKeyPanel.GetComponent<CanvasGroup>()));
            StartCoroutine(SickFade(0,1, MainMenu.GetComponent<CanvasGroup>()));
        }

        if (pauseEnabler.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && PauseMenu.activeSelf == false)
        {
            Pause();
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
        chatToPlayer.text = "options Closed!";
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


    public void SaveGame()
    {
        PlayerPrefs.SetInt("SavedInteger", intToSave);
        PlayerPrefs.SetFloat("SavedFloat", floatToSave);
        PlayerPrefs.SetString("SavedString", stringToSave);
        PlayerPrefs.Save();
        chatToPlayer.text = "The game has been saved!";
    }


    public void LoadLevel()
    {
        if (PlayerPrefs.HasKey("SavedInteger"))
        {
            intToSave = PlayerPrefs.GetInt("SavedInteger");
            floatToSave = PlayerPrefs.GetFloat("SavedFloat");
            stringToSave = PlayerPrefs.GetString("SavedString");

            intDisplay.text = intToSave.ToString();
            floatDisplay.text = floatToSave.ToString();
            strongDisplay.text = stringToSave;

            chatToPlayer.text = "Game Loaded!";
        }
        else
        {
            chatToPlayer.text = "No Saves Found!";
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("SavedInteger");
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
        strongDisplay.text = inGameString.text;
        stringToSave = strongDisplay.text.ToString();

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
    public void PlaySFX()
    {
        sfxsource.clip = soundEffects[soundEffectsIndex];
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
            current = Mathf.Lerp(start, end, (Time.unscaledTime - emarkedTime) /  2);
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
