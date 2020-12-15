using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuCtrl : MonoBehaviour
{
    public static SettingsMenuCtrl Instance;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    [SerializeField]
    private Toggle musicToggle;
    [SerializeField]
    private Toggle soundsToggle;
    [SerializeField]
    private Toggle debugToggle;
    [SerializeField]
    private Text levelSizeText;
    [SerializeField]
    private Slider levelSizeSlider;
    [SerializeField]
    private Text obstaclesText;
    [SerializeField]
    private Slider obstaclesSlider;
    [SerializeField]
    private Button buttonMenu;

    private bool debugEnabled = false;
    public bool DebugEnabled
    {
        get { return debugEnabled; }
    }
    private int minLevelSize = 5;
    private int maxLevelSize = 20;

    [SerializeField]
    private AudioMixer GameAudioMixer;

    void Start()
    {
        buttonMenu.onClick.AddListener(delegate { ButtonClickedMenu(); });
        musicToggle.onValueChanged.AddListener(delegate { MuteMusic(); });
        debugToggle.onValueChanged.AddListener(delegate { DebugToggle(); });
        soundsToggle.onValueChanged.AddListener(delegate { MuteSounds(); });
        levelSizeSlider.onValueChanged.AddListener(delegate { LevelSizeChanged(); });
        obstaclesSlider.onValueChanged.AddListener(delegate { ObstacleCountChanged(); });
        LevelSizeChanged();
        ObstacleCountChanged();
        minLevelSize = LevelGenerator.Instance.MinLevelSize;
        maxLevelSize = LevelGenerator.Instance.MaxLevelSize;
    }

    void OnEnable()
    {
        debugEnabled = debugToggle.isOn;
        musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        soundsToggle.isOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        levelSizeSlider.value = PlayerPrefs.GetFloat("LevelSize", 1);
        obstaclesSlider.value = PlayerPrefs.GetFloat("Obstacle", 0.5f);
    }
    

    public void ButtonClickedMenu()
    {
        // update settings
        LevelGenerator.Instance.LevelSize = (int)(minLevelSize + levelSizeSlider.value*(maxLevelSize - minLevelSize));
        LevelGenerator.Instance.LevelDensity = (int)(obstaclesSlider.value*100);
        // go to menu
        GameUiOverlord.Instance.MainMenu();
    }

    public void RefreshSettings()
    {
        levelSizeSlider.value = PlayerPrefs.GetFloat("LevelSize", 1);
        obstaclesSlider.value = PlayerPrefs.GetFloat("Obstacle", 0.5f);
        LevelGenerator.Instance.LevelSize = (int)(minLevelSize + levelSizeSlider.value * (maxLevelSize - minLevelSize));
        LevelGenerator.Instance.LevelDensity = (int)(obstaclesSlider.value * 100);
    }



    public void LevelSizeChanged()
    {
        PlayerPrefs.SetFloat("LevelSize", levelSizeSlider.value) ; 
        levelSizeText.text = (minLevelSize + levelSizeSlider.value*(maxLevelSize - minLevelSize)).ToString("F0");
    }
    public void ObstacleCountChanged()
    {
        PlayerPrefs.SetFloat("Obstacle", obstaclesSlider.value);
        obstaclesText.text = (obstaclesSlider.value * 100).ToString("F0") + "%";
    }


    public void MuteMusic()
    {
        if (musicToggle.isOn)
        {
            PlayerPrefs.SetInt("MusicOn", 1);
            GameAudioMixer.SetFloat("MUSIC_vol", 0f);
        }
        else
        {
            PlayerPrefs.SetInt("MusicOn", 0);
            GameAudioMixer.SetFloat("MUSIC_vol", -80f);
        }
    }

    public void DebugToggle()
    {
        debugEnabled = debugToggle.isOn;
    }

    public void MuteSounds()
    {
        if (soundsToggle.isOn)
        {
            PlayerPrefs.SetInt("SoundOn", 1);
            GameAudioMixer.SetFloat("SFX_vol", 0f);
        }
        else
        {
            PlayerPrefs.SetInt("SoundOn", 0);
            GameAudioMixer.SetFloat("SFX_vol", -80f);
        }
    }
}
