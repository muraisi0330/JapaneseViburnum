using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PINLock : InteractionEvents
{

    string PIN_number = "ABC";
    [SerializeField] PINDatabaseScript pinData;
    [SerializeField] int currentPINIndex = 0;

    [SerializeField] Button okButton;
    [SerializeField] Text passwordText;
    Text countText;
    
    bool operation = false;
    [SerializeField]
    DoorScript[] behaviour;
    [SerializeField] GameObject objectToShow;
    [SerializeField] GameObject passwordToShow;
    [SerializeField] GameObject player;

    [SerializeField] AudioClip selectedVoice;
    [SerializeField] AudioClip failureAudio;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        okButton.onClick.AddListener(Password);
        PIN_number = pinData.PINList[currentPINIndex].PIN.ToString();
        countText = objectToShow.GetComponentInChildren<Text>();
    }

    void Password()
    {
        if (operation != true) return;
      
        if (passwordText.text == PIN_number)
        {
            this.enabled = false;
            EnableScript();
            CheckPassword(selectedVoice, "鍵が開きました");
        }
        else
        {
            CheckPassword(failureAudio, "パスワードが違います");
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Player>().canControl = true;
        passwordToShow.SetActive(false);
        operation = false;

    }
    // Update is called once per frame
    public override void interactionEvents()
    {
            operation = true;
            player.GetComponent<Player>().canControl = false;
            passwordToShow.SetActive(true);
        　　Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
    }
    void CheckPassword(AudioClip audioClip, string message)
    {
            audioSource.PlayOneShot(audioClip);
            countText.text = message;
            passwordText.text = "";
            objectToShow.SetActive(true);
    
    }
    
    void EnableScript()
    {
        for (int i = 0; i < behaviour.Length; i++)
        {
            behaviour[i].isLocked = false;
        }
    }
}
