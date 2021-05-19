using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public int soundEffectsIndex;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject AnyKeyPanel;
    public GameObject MainMenu;
    public Coroutine[] FadeCoroutine;
    public Coroutine[] FadeInCoroutine;

    public Dropdown resolution;
    public Resolution[] resolutions;

    public Scene[] scenes;

    public AudioMixerGroup[] Audio;
    public AudioSource musicSource;
    public AudioSource sfxsource;
    public bool muted;
    public AudioClip[] levelMusic;
    public AudioClip[] soundEffects;
    public Text musicVolumePercent;
    public Text sFXVolumePercent;

    public float markedTime;

    public void Start()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(false);

        Screen.fullScreen = true;
        resolutions = Screen.resolutions;
        resolution.ClearOptions();

        musicSource = GetComponent<AudioSource>();

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
    }

    public void Update()
    {
        if (MainMenu.activeSelf == false && Input.anyKeyDown )
        {
            MainMenu.SetActive(true);

            StartCoroutine(SickFade(1,0, AnyKeyPanel.GetComponent<CanvasGroup>()));
            StartCoroutine(SickFade(0,1, MainMenu.GetComponent<CanvasGroup>()));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void AnyKeyScreen()
    {

    }

    public void PlayGame()
    {

    }
    public void Options()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(true);
        StartCoroutine(SickFade(0, 1, OptionsMenu.GetComponent<CanvasGroup>()));
    }
    public void CloseOptions()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(false);
    }
    public void Pause()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void ToMainMenu()
    {

    }

    public void Save()
    {

    }

    public void LoadLevel()
    {

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

    public IEnumerator SickFade(float start, float  end, CanvasGroup selectedObject)
    {
        float emarkedTime = Time.time;
        float current;
        float difference = 0.01f;
        current = start;

        while (Mathf.Abs(current - end) > difference)
        {
            current = Mathf.Lerp(start, end, Time.time / (emarkedTime + 2));
            selectedObject.alpha = current;

            yield return 0;
        }
    }

    public void TimeMarker()
    {
        markedTime = Time.time;
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
