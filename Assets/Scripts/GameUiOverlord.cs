using UnityEngine;
using UnityEngine.Audio;

public class GameUiOverlord : MonoBehaviour
{
    public static GameUiOverlord Instance;
    [SerializeField] private GameObject mainMenuGo;
    [SerializeField] private GameObject gameRunningMenuGo;
    [SerializeField] private GameObject endGameWindowGo;
    [SerializeField] private GameObject settingsWindowGo;
    [SerializeField] private GameObject blackBordureGo;

    [SerializeField]
    private AudioMixer GameAudioMixer;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
        blackBordureGo.SetActive(true);
    }

    private void Start()
    {
        ActivateAllWindows();
        MainMenu();

        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
            GameAudioMixer.SetFloat("MUSIC_vol", 0f);
        else
            GameAudioMixer.SetFloat("MUSIC_vol", -80f);

        if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
            GameAudioMixer.SetFloat("SFX_vol", 0f);
        else
            GameAudioMixer.SetFloat("SFX_vol", -80f);
    }
    


    public void ActivateAllWindows(){

        mainMenuGo.SetActive(true);
        gameRunningMenuGo.SetActive(true);
        endGameWindowGo.SetActive(true);
        settingsWindowGo.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuGo.SetActive(true);
        gameRunningMenuGo.SetActive(false);
        endGameWindowGo.SetActive(false);
        settingsWindowGo.SetActive(false);
    }

    public void PlayGameMenu()
    {
        Time.timeScale = 1;
        mainMenuGo.SetActive(false);
        gameRunningMenuGo.SetActive(true);
        endGameWindowGo.SetActive(false);
        settingsWindowGo.SetActive(false);
    }
    
    public void EndGameMenu()
    {
        mainMenuGo.SetActive(false);
        gameRunningMenuGo.SetActive(false);
        endGameWindowGo.SetActive(true);
        settingsWindowGo.SetActive(false);
    }
    
    public void ActivateSettingsWindow()
    {
        mainMenuGo.SetActive(false);
        gameRunningMenuGo.SetActive(false);
        endGameWindowGo.SetActive(false);
        settingsWindowGo.SetActive(true);
    }
}
