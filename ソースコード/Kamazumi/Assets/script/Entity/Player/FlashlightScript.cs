using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
        private KeyCode toggleKey = KeyCode.F; // 点灯/消灯の切り替えキー

        private Light flashlight; // フラッシュライトのLightコンポーネント

    [SerializeField]
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        flashlight = GetComponent<Light>();
        toggleKey = OperationSettingsScript.OperationSettings.toggleKey;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlight == null) return; // Lightコンポーネントが見つからない場合は処理を中断
        if (!player.canControl) return; // プレイヤーが操作できない場合は処理を中断
        if (player.isPaused) return; // ゲームが一時停止している場合は処理を中断
        if (player.isSmartphoneOpen) return;

        if (Input.GetKeyDown(toggleKey))
        {
             
            flashlight.enabled = !flashlight.enabled; // フラッシュライトのON/OFFを切り替える 

        }
    }
}
