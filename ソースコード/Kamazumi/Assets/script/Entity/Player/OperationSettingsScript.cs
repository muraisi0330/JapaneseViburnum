using UnityEngine;

public static class OperationSettingsScript
{
    //操作設定のキー
    public static class OperationSettings
    {
       // 点灯/消灯の切り替えキー
          //移動のキー
        public static string horizontal = "Horizontal";
        public static string vertical = "Vertical";
        //走るキー
        public static KeyCode sprintKey = KeyCode.LeftShift;
        //視点移動のマウス
        public static string mouseX = "Mouse X";
        public static string mouseY = "Mouse Y";
        //フラッシュライトの切り替えキー
        public static KeyCode toggleKey = KeyCode.E;
        //インタラクトのキー左クリックでインタラクション
        public static KeyCode interactKey = KeyCode.Mouse0;
        //スマホUIの切り替えキー
        public static KeyCode smartphoneUIKey = KeyCode.Mouse1;
    }

    //操作設定の初期化
    public static void InitializeOperationSettings(KeyCode key)
    {
        OperationSettings.toggleKey = key;
    }

}
