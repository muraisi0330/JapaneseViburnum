using UnityEngine;
using UnityEngine.UI;

public class Locking1 : InteractionEvents
{

    [SerializeField]
    Behaviour[] behaviour;

    [SerializeField] private GameObject key;

    public Text countText;
    public GameObject textToDisplay;
    // Start is called before the first frame update
    void Start()
    {

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
            CheckPassword(key.gameObject.name + "が必要です。");
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
            behaviour[i].enabled = true;
        }
    }
}