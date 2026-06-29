using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUIScript : MonoBehaviour
{
    //スタッフロールの移動速度
    [SerializeField] private float rollSpeed = 20f;

    //特定の位置に来たらシーンを切り替えるための位置
    [SerializeField] private float changeScenePositionY = 100f;

    public RectTransform rectTransform;

    // ロードするシーンの名前
    public string sceneName;

    // ロードの進捗状況を表示するUIなど
    public IoadingScreenScript loadingUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeScene();
        if (rectTransform.anchoredPosition.y >= changeScenePositionY) return;
        //スタッフロールを上に移動させる
        rectTransform.Translate(Vector3.up * rollSpeed * Time.deltaTime);
    }
    //シーン移動の関数
    void ChangeScene()
    {
       
        if (!Input.anyKeyDown) return;
        loadingUI.StartLoad(sceneName);
    }
}
