using UnityEngine;
using UnityEngine.UI;

public class locking : InteractionEvents
{

    [SerializeField]
    DoorScript[] behaviour;
    [SerializeField]
    locking[] lockings;

    [SerializeField] private GameObject key;

    public Text countText;
    public GameObject textToDisplay;
    string itemName;
    // Start is called before the first frame update
    void Start()
    {
        if (key.GetComponentInParent<key>() != null)
        {
            itemName = key.GetComponent<key>().PINITemDatabase.PINList[key.GetComponent<key>().itemIndex].itemName;
        }
        else
        {
            itemName = key.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void interactionEvents()
    {
        if (key.activeSelf == false)
        {
            this.enabled = false;
            EnableScript();
            CheckPassword("鍵が開きました");
        }
        else
        {
          
            CheckPassword(itemName + "が必要です。");
        }
    }

    void CheckPassword(string message)
    {
       
        countText.text = message;
        textToDisplay.SetActive(true);

    }

    void EnableScript()
    {
        for (int i = 0; i < behaviour.Length; i++)
        {
            behaviour[i].isLocked = false;
        }
        foreach (var locking in lockings)
        {
            locking.enabled = false;
        }
    }
}
