using UnityEngine;
using UnityEngine.UI;

public class key : InteractionEvents
{
   
    public Text countText;
    public GameObject objectToShow;
    public PINITemDatabaseDcript PINITemDatabase;
    public InventoryUI inventoryUI;
    public int itemIndex = 0;
    //name
    string itemName;
    // Start is called before the first frame update
    void Start()
    {
        itemName = PINITemDatabase.PINList[itemIndex].itemName;
    }
    public override void interactionEvents()
    {
        this.gameObject.SetActive(false);
        countText.text
   = itemName + "を手に入れた";
        objectToShow.SetActive(true);
        inventoryUI.UpdateItemUI(itemIndex);
    }
    // Update is called once per frame
    void Update()
    {
   
    }
  
}
