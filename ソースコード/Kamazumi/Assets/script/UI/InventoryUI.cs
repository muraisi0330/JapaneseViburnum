using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    //インベントリのUIを管理するクラス
    [SerializeField]
    public ItemUI[] inventory;
    [SerializeField]
    public PINITemDatabaseDcript PINITemDatabase;
    //アイテムのUIを管理するクラス
    [SerializeField]
    public Image itemImage; //アイテムの画像を表示するUIコンポーネント
    [SerializeField]
    public Text itemNameText; //アイテムの名前を表示するUIコンポーネント
    [SerializeField]
    public Text itemDescriptionText; //アイテムの説明を表示するUIコンポーネント

    //アイテムのUIを更新するメソッド
    public void UpdateItemUI(int item)
    {
        //アイテムのデータを取得する
        if(item < 0) return; //アイテムが存在しない場合は処理を終了する
        if(item >= inventory.Length) return;//アイテムのインデックスが配列の範囲外の場合は処理を終了する
        inventory[item].ShowItemUI(PINITemDatabase.PINList[item].searchImage);
        inventory[item].item = item; //アイテムのインデックスをアイテムのUIに代入する
    }

    //アイテムのUIを調べるメソッド
    public void CheckItemUI(ItemUI UI)
    {
        //デバック
        Debug.Log("CheckItemUI");
        if (UI == null) return;
        int item = UI.item; //アイテムのインデックスを取得する
        if (item < 0) return; //アイテムが存在しない場合は処理を終了する
        itemImage.sprite = PINITemDatabase.PINList[item].searchImage; //アイテムの画像を表示するUIコンポーネントにアイテムの画像を代入する
        itemNameText.text = PINITemDatabase.PINList[item].itemName; //アイテムの名前を表示するUIコンポーネントにアイテムの名前を代入する
        itemDescriptionText.text = PINITemDatabase.PINList[item].itemDescription; //アイテムの説明を表示するUIコンポーネントにアイテムの説明を代入する
    }
}
