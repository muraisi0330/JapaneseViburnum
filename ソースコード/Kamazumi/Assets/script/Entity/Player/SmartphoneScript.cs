using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartphoneScript : MonoBehaviour
{
    KeyCode SmartphoneUIKey = KeyCode.Mouse1;
    [SerializeField] private SmartphoneUIScript smartphoneUI;
    //プレイヤー
    [SerializeField] private Player player;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        SmartphoneUIKey = OperationSettingsScript.OperationSettings.smartphoneUIKey;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSmartphoneUI();
    }
    void HandleSmartphoneUI()
    {
        if(player.isPaused) return;
        if (!player.canControl) return;
        if (!Input.GetKeyDown(SmartphoneUIKey)) return;
        isOpen = !isOpen;
        player.isSmartphoneOpen = isOpen;
        smartphoneUI.ToggleSmartphoneUI(isOpen);
        ToggleCursor(isOpen);
    }
    //カーソルの表示を切り替える
    public void ToggleCursor(bool isOpen)
    {
        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
