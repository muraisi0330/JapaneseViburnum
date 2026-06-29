using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using DG.Tweening;

public class password : DoorScript
{
    string PIN_number = "ABC";
    public PINDatabaseScript pinData;
    public int currentPINIndex = 0; 
    public Button okButton;
    public Text passwordText;
    public Text countText;
    bool isOpen2 = false;
    bool operation = false;
    public GameObject objectToShow;
    public GameObject passwordToShow;
    public GameObject player;
    public AudioClip selectedVoice;
    public AudioClip failureAudio;
    public AudioClip startupVoice;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        okButton.onClick.AddListener(Password);
        PIN_number = pinData.PINList[currentPINIndex].PIN.ToString();
    }
    void Password()
    {
        if (operation != true) return;
      
            if (passwordText.text == PIN_number)
            {
                audioSource.PlayOneShot(selectedVoice);
                isOpen2 = true;
                countText.text = "鍵が開きました";
                passwordText.text = "";
                objectToShow.SetActive(true);
            }
            else
            {
                audioSource.PlayOneShot(failureAudio);
                countText.text = "パスワードが違います";
                passwordText.text = "";
                objectToShow.SetActive(true);
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
        if (isOpen2 == true)
        {
            Renderer renderer = GetComponent<Renderer>();
            OpenAndClose();
        }
        else
        {
            operation = true;
            player.GetComponent<Player>().canControl = false;
            passwordToShow.SetActive(true);
        }
    }
    public override void CloseDoor()
    {
        var sequence = DOTween.Sequence();
        isTouching = true;
        audioSource.PlayOneShot(startupVoice);
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -4f, 0), 1f))
           .AppendCallback(() =>
           {
               isOpen = false;
               isTouching = false;
           });

    }
    public override void OpenDoor()
    {
        var sequence = DOTween.Sequence();
        isTouching = true;
        audioSource.PlayOneShot(startupVoice);
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, 4f, 0), 1f))
            .AppendCallback(() =>
            {
                isOpen = true;
                isTouching = false;
            });
    }
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        var sequence = DOTween.Sequence();

        if (other.CompareTag("hand"))
        {
            if (isTouching == false)
            {
                isTouching = true;
                sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, 4f, 0), 1f))
                .AppendInterval(3)
                .Append(transform.DOBlendableMoveBy(new Vector3(0, -4f, 0), 1f))
                .AppendCallback(() =>
                  {
                      isTouching = false;
                  });
            }
        }
    }
}
