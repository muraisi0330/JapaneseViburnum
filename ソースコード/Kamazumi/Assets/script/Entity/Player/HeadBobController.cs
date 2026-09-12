using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float walkSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;

    private float timer = 0f;
    private Vector3 startPos;

    [SerializeField] private Player player;

    void Start()
    {
        // カメラの初期位置を保存
        startPos = transform.localPosition;
    }

    void Update()
    {
        // 移動していない場合はカメラを初期位置に戻す
        if (player.IsMoving())
        {
            ResetCameraPosition();
        }
        else
        {
            HandleHeadBob();
        }
    }
    
    void HandleHeadBob()
    {
            timer += Time.deltaTime * walkSpeed;
            Vector3 pos = startPos;
            pos.y += Mathf.Sin(timer) * bobAmount;
            pos.x += Mathf.Cos(timer / 2f) * bobAmount; // 左右の揺れは少し遅らせる
            transform.localPosition = pos;
    }
    // 移動していない場合はカメラを初期位置に戻す
    void ResetCameraPosition()
    {
        timer = 0f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * walkSpeed);

    }


}
