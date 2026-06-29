using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float walkSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;

    private float timer = 0f;
    private Vector3 startPos;


    void Start()
    {
        // カメラの初期位置を保存
        startPos = transform.localPosition;
    }

    void Update()
    {
        // WASDや矢印キーの入力量を取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 移動していない場合はカメラを初期位置に戻す
        if (Mathf.Abs(horizontal) == 0f && Mathf.Abs(vertical) == 0f)
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * walkSpeed);
        }
        else
        {
            // 移動している場合はサイン波でカメラを揺らす
            timer += Time.deltaTime * walkSpeed;
            Vector3 pos = startPos;
            pos.y += Mathf.Sin(timer) * bobAmount;
            pos.x += Mathf.Cos(timer / 2f) * bobAmount; // 左右の揺れは少し遅らせる
            transform.localPosition = pos;
        }
    }
}
