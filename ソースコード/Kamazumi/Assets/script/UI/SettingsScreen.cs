using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SettingsScreen : MonoBehaviour
{
    public GameObject homeScreen;//ホーム画面
    public GameObject pauseScreen;//ポーズ画面
    public GameObject settingsScreen;//設定画面
    public GameObject player;
    public AudioSettingsScript　audioSettingsScript;

    // ロードの進捗状況を表示するUIなど
    public IoadingScreenScript loadingUI;
    // Start is called before the first frame update
    void Start()
    {
    }
    //設定画面からポーズ画面に戻る関数
    public void Password()
    {
        this.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Player>().isPaused = false;
        Time.timeScale = 1;
    }
    //ポーズ画面から設定画面に行く関数
    public void Password2()
    {
       
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        
    }
    //設定画面からポーズ画面に戻る関数
    public void Password3()
    {
        audioSettingsScript.OnBGMVolumeChanged();
        pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }
    //タイトル画面に戻る関数
    public void Password4()
    {
        loadingUI.StartLoad("Title Scene");
        Time.timeScale = 1;
    }
   

  

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf == false) return;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
    }
}
