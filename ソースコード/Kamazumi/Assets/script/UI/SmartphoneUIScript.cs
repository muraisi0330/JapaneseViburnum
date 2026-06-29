using UnityEngine;

public class SmartphoneUIScript : MonoBehaviour
{
    //ホーム画面のUI
    [SerializeField] private GameObject homeUI;
    //検索画面のUI
    [SerializeField] private GameObject searchUI;
    //インベントリ画面のUI
    [SerializeField] private GameObject inventoryUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //スマートフォンのUIの状態を切り替える
    public void ToggleSmartphoneUI(bool isOpen)
    {
        if (isOpen)
        {
            OpenSmartphoneUI();
        }
        else
        {
            CloseSmartphoneUI();
        }
    }
    //スマートフォンのUIを閉じる
    void CloseSmartphoneUI()
    {
        homeUI.SetActive(false);
        searchUI.SetActive(false);
        inventoryUI.SetActive(false);
    }
    //スマートフォンのUIを開く
    void OpenSmartphoneUI()
    {
        homeUI.SetActive(true);
        searchUI.SetActive(false);
        inventoryUI.SetActive(false);
    }

    //スマートフォンのUIをホーム画面に切り替える
    public void SwitchToHomeUI()
    {
        homeUI.SetActive(true);
        searchUI.SetActive(false);
        inventoryUI.SetActive(false);
    }
    //スマートフォンのUIを検索画面に切り替える
    public void SwitchToSearchUI()
    {
        homeUI.SetActive(false);
        searchUI.SetActive(true);
        inventoryUI.SetActive(false);
    }
    //スマートフォンのUIをインベントリ画面に切り替える
    public void SwitchToInventoryUI()
    {
        homeUI.SetActive(false);
        searchUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

}
