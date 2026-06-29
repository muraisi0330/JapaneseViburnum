using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCpassword : MonoBehaviour
{
    public GameObject hintscreen;
    public GameObject failureText;
    public GameObject inputScreen;
    public InputField input;
    public PINDatabaseScript pinData;
    public int currentPINIndex = 1;
    string password;
    // Start is called before the first frame update
    void Start()
    {
      password = pinData.PINList[currentPINIndex].PIN.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        input.ActivateInputField();
    }
    public void Input()
    {
        if(input.text == password)
        {
            hintscreen.SetActive(true);
            inputScreen.SetActive(false);
        }
        else
        {
            failureText.SetActive(true);
            input.text = "";
            input.ActivateInputField();
        }

    }
}
