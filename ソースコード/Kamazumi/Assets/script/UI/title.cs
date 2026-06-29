using UnityEngine;
using UnityEngine.UI;

public class title : MonoBehaviour
{
    public GameObject settingsScreen;//設定画面
    public GameObject PauseScreen;//タイトル画面
    [SerializeField] Slider Slider;
    public static float num = 5;
    [SerializeField] AudioSettingsScript audioSettingsScript;

    // ロードの進捗状況を表示するUIなど
    public IoadingScreenScript loadingUI;
   

    // Start is called before the first frame update
    void Start()
    {
        Slider.value = SaveDataScript.LoadMicrophoneVolume(); ;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    //ゲーム開始の関数
    public void Password()
    {
        num = Slider.value;
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        loadingUI.StartLoad("SampleScene");
    }

    

    //設定画面に移動する関数
    public void Password2()
    {
        PauseScreen.SetActive(false);
        settingsScreen.SetActive(true);

    }
    //タイトル画面に移動する関数
    public void Password3()
    {
        PauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
        audioSettingsScript.OnBGMVolumeChanged();
    }
    //ゲーム終了の関数
    public void Password4()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
