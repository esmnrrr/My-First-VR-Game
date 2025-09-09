using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class VRMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject pausePanel;
    
    [Header("Settings UI Elements")]
    public Slider volumeSlider;
    public Toggle muteToggle;
    public Dropdown qualityDropdown;
    public Toggle vsyncToggle;
    
    [Header("Audio")]
    public AudioMixerGroup masterMixer;
    
    [Header("Pause System")]
    public KeyCode pauseKey = KeyCode.Escape;
    
    public bool isPaused = false;
    private bool isInGame = false;

    void Start()
    {
        ShowMainMenu();
        LoadSettings();
        
        // Pause canvas'ını başlangıçta gizle
        if (pausePanel != null)
            pausePanel.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        // Oyun içindeyken pause kontrolü
        if (isInGame && Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    #region Main Menu Functions
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        mainMenuPanel.transform.parent.gameObject.SetActive(false);
        isInGame = true;
        
        // Pause canvas'ını aktif et
        if (pausePanel != null)
            pausePanel.transform.parent.gameObject.SetActive(true);
            
        // Oyun sahnesini yükle
        // SceneManager.LoadScene("GameScene");
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        SaveSettings();
        Application.Quit();
    }
    #endregion

    #region Pause Menu Functions
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Debug.Log("Game Resumed");
    }

    public void PauseToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isInGame = false;
        
        pausePanel.transform.parent.gameObject.SetActive(false);
        mainMenuPanel.transform.parent.gameObject.SetActive(true);
        ShowMainMenu();
        
        // Ana menü sahnesini yükle
        // SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Settings Functions
    public void OnVolumeChanged()
    {
        float volume = volumeSlider.value;
        if (masterMixer != null)
        {
            masterMixer.audioMixer.SetFloat("MasterVolume", 
                volume > 0 ? Mathf.Log10(volume) * 20 : -80);
        }
        else
        {
            AudioListener.volume = volume;
        }
    }

    public void OnMuteToggled()
    {
        if (muteToggle.isOn)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = volumeSlider.value;
        }
    }

    public void OnQualityChanged()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    public void OnVsyncToggled()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
    }

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        
        if (isInGame && isPaused)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            mainMenuPanel.SetActive(true);
        }
    }
    #endregion

    #region Settings Save/Load
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Mute", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        PlayerPrefs.SetInt("Vsync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        // Volume
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            OnVolumeChanged();
        }

        // Mute
        if (muteToggle != null)
        {
            muteToggle.isOn = PlayerPrefs.GetInt("Mute", 0) == 1;
            OnMuteToggled();
        }

        // Quality
        if (qualityDropdown != null)
        {
            qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
            OnQualityChanged();
        }

        // Vsync
        if (vsyncToggle != null)
        {
            vsyncToggle.isOn = PlayerPrefs.GetInt("Vsync", 1) == 1;
            OnVsyncToggled();
        }
    }
    #endregion
}