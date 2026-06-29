using UnityEngine;

public class ClearJudgmentScript : MonoBehaviour
{
    //タイトルに戻るためのスクリプト
    // ロードするシーンの名前
    public string sceneName;

    // ロードの進捗状況を表示するUIなど
    public IoadingScreenScript loadingUI;

 

    //貫通タイプの当たり判定の関数
    private void OnTriggerEnter(Collider other)
    {
        //当たり判定の処理
        if (other.gameObject.tag == "Player")
        {
            loadingUI.StartLoad(sceneName);
        }
    }
}
