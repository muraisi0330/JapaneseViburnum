using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    // ロードの進捗状況を表示するUIなど
    public IoadingScreenScript loadingUI;
   
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Password()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        loadingUI.StartLoad("SampleScene");
    }
    public void Password2()
    {
        loadingUI.StartLoad("Title Scene");
    }

  


    public void Password3()
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
