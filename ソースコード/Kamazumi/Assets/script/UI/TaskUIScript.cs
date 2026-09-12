using UnityEngine;
using UnityEngine.UI;

public class TaskUIScript : MonoBehaviour
{
    [SerializeField]
    public PINIProgressScript PINIprogressScript;
    [SerializeField]
    public Text taskText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTaskUI(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //タスクの内容を更新するメソッド
    public void UpdateTaskUI(int ID)
    {
        if (ID < 0) return; //タスクが存在しない場合は処理を終了する
        if (ID >= PINIprogressScript.PINList.Count) return; //タスクのインデックスがリストの範囲外の場合は処理を終了する
        taskText.text = PINIprogressScript.PINList[ID].taskName; //タスクの内容をタスクのUIに代入する
    }
}
