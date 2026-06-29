using UnityEngine;
using UnityEngine.UI;

public class SearchScreenScript : MonoBehaviour
{
    
    public GameObject hintscreen;
    public GameObject failureText;
    public GameObject inputScreen;
    public InputField input;
    public Image hintImage;
    public PINSearchDatabaseScript pinData;
    public int currentPINIndex = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (input.isFocused) return;
            input.ActivateInputField();
    }
    public void Input()
    {
        string inputText = input.text;

        // 【検索処理】
        // PINListの中から、入力された文字列とPIN(を文字列にしたもの)が一致する最初の要素を探す
        var foundData = pinData.PINList.Find(x => x.PIN.ToString() == inputText);

        if (foundData != null)
        {
            // 一致するものがあった場合
            currentPINIndex = pinData.PINList.IndexOf(foundData); // 見つかった要素の番号を保存
            hintImage.sprite = pinData.PINList[currentPINIndex].searchImage; // ヒント画像を更新

            hintscreen.SetActive(true);
            inputScreen.SetActive(false);
            failureText.SetActive(false);
        }
        else
        {
            // データベースのどこにも一致するものがなかった場合
            failureText.SetActive(true);
            input.text = "";
            input.ActivateInputField();
        }

    }
    //リセット関数    
    public void ResetScreen()
    {
        input.text = "";
    }

    //画面を閉じる関数
    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
        hintscreen.SetActive(false);
        inputScreen.SetActive(true);
        failureText.SetActive(false);
        ResetScreen();

    }
    //画面を開く関数
    public void OpenScreen()
    {
        this.gameObject.SetActive(true);
        hintscreen.SetActive(false);
        inputScreen.SetActive(true);
        failureText.SetActive(false);
    }

    //画面を開く閉じる 関数
    public void OpenCloseScreen(bool isOpen)
    {
        if (isOpen)
        {
            OpenScreen();
        }
        else
        {
            CloseScreen();
        }
    }
}
