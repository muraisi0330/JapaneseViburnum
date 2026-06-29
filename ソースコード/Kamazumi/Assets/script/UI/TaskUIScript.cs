using UnityEngine;
using UnityEngine.UI;

public class TaskUIScript : MonoBehaviour
{
    [SerializeField]
    public PINIProgressScript PINIprogressScript;
    [SerializeField]
    public Text taskText;

    int taskNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTaskUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //タスクの内容を更新するメソッド
    public void UpdateTaskUI()
    {
        if (taskNum < 0) return; //タスクが存在しない場合は処理を終了する
        if (taskNum >= PINIprogressScript.PINList.Count) return; //タスクのインデックスがリストの範囲外の場合は処理を終了する
        taskText.text = PINIprogressScript.PINList[taskNum].taskName; //タスクの内容をタスクのUIに代入する
        taskNum++; //次のタスクに進むためにタスクのインデックスを増やす
    }
}
