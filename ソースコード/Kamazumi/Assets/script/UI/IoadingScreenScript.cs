using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IoadingScreenScript : MonoBehaviour
{
    [SerializeField] GameObject loadingObject;
    [SerializeField] GameObject loadingUI;

    AsyncOperation asyncOperation;

    //loadingObjectの回転速度
    int rotationSpeed = -360;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    // ロードを開始するメソッド
    public void StartLoad(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    // コルーチンを使用してロードを実行するメソッド
    private IEnumerator Load(string sceneName)
    {
        // ロード画面を表示する
        loadingUI.SetActive(true);


        // シーンを非同期でロードする
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // ロードが完了するまで待機する
        while (!asyncOperation.isDone)
        {
            loadingObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }
}
