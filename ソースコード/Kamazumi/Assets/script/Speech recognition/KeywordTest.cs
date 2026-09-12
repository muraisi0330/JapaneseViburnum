using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordTest : InteractionEvents
{
    private KeywordController keyCon;
   private string[][] keywords;
    [System.Serializable]
    public struct StringRow
    {
        public string row;
        public string row2;
    }

    // インスペクターで2次元配列のように扱える構造体の配列
    public StringRow[] string2DArray;
    // Use this for initialization
    void Start()
    {
        keywords = new string[string2DArray.Length][];

        for (int i = 0; i < string2DArray.Length; i++)
        {
            keywords[i] = new string[]
            {
            string2DArray[i].row,
            string2DArray[i].row2
            };
        }

        // 同じGameObjectに付いているKeywordControllerを取得
        keyCon = GetComponent<KeywordController>();

        if (keyCon == null)
        {
            Debug.LogError("KeywordControllerがGameObjectに付いていません。");
            return;
        }

        // KeywordControllerに設定
        keyCon.Initialize(keywords, true);

        // KeywordRecognizerを作成
        keyCon.SetKeywords();

        // 全てのRecognizerを開始
        for (int i = 0; i < keywords.Length; i++)
        {
            keyCon.StartRecognizing(i);
        }
    }

    void Update()
    {
        interactionEvents();
     
    }

    public override void interactionEvents()
    {
        if (keywords == null) return;
          
        for (int i = 0; i < keyCon.hasRecognized.Length; i++)
        {
            if (!keyCon.hasRecognized[i]) continue;
                Debug.Log("keyword[" + i + "] was recognized");
                keyCon.hasRecognized[i] = false;
                Events();
            return;
        }
    }
    public virtual void Events()
    {

    }
}
