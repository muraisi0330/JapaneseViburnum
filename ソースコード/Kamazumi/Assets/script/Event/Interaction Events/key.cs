using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class key : InteractionEvents , IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

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

   public void LoadData(GameData data)
    {
      data.itemCollected.TryGetValue(id, out bool isCollected);
        if (isCollected)
        {
            itemCollected();
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.itemCollected.ContainsKey(id))
        {
            data.itemCollected.Remove(id);
        }

        data.itemCollected.Add(id, !this.gameObject.activeSelf);
    }

    public override void interactionEvents()
    {
       
        countText.text
   = itemName + "を手に入れた";
        objectToShow.SetActive(true);
        itemCollected();
    }

    public void itemCollected()
    {
        this.gameObject.SetActive(false);
      
        inventoryUI.UpdateItemUI(itemIndex);
    }

        // Update is called once per frame
        void Update()
    {
   
    }
  
}
