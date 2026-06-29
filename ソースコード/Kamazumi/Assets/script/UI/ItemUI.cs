using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    //アイテムのUIを管理するクラス
    [SerializeField]
    public Image itemImage; //アイテムの画像を表示するUIコンポーネント
    [SerializeField]
    public Button itemButton; //アイテムを選択するUIコンポーネント

    [SerializeField]
    public int item; //アイテムのデータを保持する変数
    // Start is called before the first frame update
    void Start()
    {
    }
  
    // Update is called once per frame
    void Update()
    {

    }
    //アイテムのUIを表示するメソッド
    public void ShowItemUI(Sprite sprite)
    {
        itemImage.sprite = sprite; //アイテムの画像を表示するUIコンポーネントにアイテムの画像を代入する
        itemImage.GetComponent<Image>().enabled = true; //アイテムの画像を表示するUIコンポーネントを有効にする
        itemButton.interactable = true; //アイテムを選択するUIコンポーネントをクリックできるようにする
    }
    
}
