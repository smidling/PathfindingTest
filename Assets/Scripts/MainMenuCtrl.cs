
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour
{

    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonSetting;


    private void Awake()
    {
        buttonStart.onClick.AddListener(delegate { ButtonClickedStart(); });
        buttonSetting.onClick.AddListener(delegate { ButtonClickedSettings(); });
        Canvas.ForceUpdateCanvases();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            ButtonClickedStart();
    }


    public void ButtonClickedStart()
    {
        // hard connection, do not change order
        LevelGenerator.Instance.StartTheGame();
        GameUiOverlord.Instance.PlayGameMenu();
    }

    public void ButtonClickedSettings()
    {
        GameUiOverlord.Instance.ActivateSettingsWindow();
    }
}
