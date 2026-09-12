using UnityEngine;
using DG.Tweening;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform perspective;


    [SerializeField] private float viewDistance = 10f;//視野距離
    [SerializeField, Range(90, 220)] private float viewAngle = 90f;//視野角度
                                                                   //プレイヤーを取得するためのターゲット
    [SerializeField] public GameObject player;
    bool isPlayerInSight = false; //プレイヤーが視野内にいるかどうかのフラグ
    //DOTweenのシーケンスを作成するための変数
    Sequence seq;  
    public bool　isTweening = false;
    // Start is called before the first frame update
    void Start()
    {
        seq = DOTween.Sequence();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //プレイヤーの位置を取得する関数
  

    //レイを飛ばして視野内にプレイヤーがいるかどうかを判定する関数
    public bool IsPlayerInSight()
    {
       if (!isPlayerInSight) return false;
       if (player == null) return false;
       if(!CheckPlayerInSight()) return false;
       return OnPlayerEnter();
    }

    //レイを飛ばしてプレイヤーがいるかどうかを判定する関数
    public bool CheckPlayerInSight()
    {
        Vector3 target = player.transform.position - this.transform.position;
        float distance = target.magnitude;
        Ray ray = new Ray(transform.position, target.normalized);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, viewDistance);
        if (!Physics.Raycast(ray, out hit, viewDistance)) return false;
        if (!hit.collider.CompareTag("Player")) return false;
        return true;
    }

    //プレイヤーが視野内に入ったときの処理
    public bool OnPlayerEnter()
    {
        Vector3 look = perspective.position - this.transform.position;    // 視点方向ベクトル
        Vector3 target = player.transform.position - this.transform.position;
        float cosHalfSight = Mathf.Cos(viewAngle / 2 * Mathf.Deg2Rad);    // 視野角（の半分）の余弦
        float cosTarget = Vector3.Dot(look, target) / (look.magnitude * target.magnitude);  // ターゲットへの角度の余弦
        return cosTarget > cosHalfSight && target.magnitude < viewDistance;   // ターゲットへの角度が視界の角度より小さく、かつ距離が視界より近いなら見えていると判定して true を返す
    }
    //プレイヤーが視野内に入ったときの処理
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInSight = true;
        player = other.gameObject;


    }
    //プレイヤーが視野内から出たときの処理
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInSight = false;
        player = null;

    }



    //視野の範囲を可視化するための関数
    private void OnDrawGizmos()
    {
        // 視界の範囲（正面及び左右の端）をギズモとして描く
        Vector3 lookAtDirection = (perspective.position - this.transform.position).normalized;    // 正面（正規化）
        Vector3 rightBorder = Quaternion.Euler(0, viewAngle / 2, 0) * lookAtDirection;    // 右端（正規化）
        Vector3 leftBorder = Quaternion.Euler(0, -1 * viewAngle / 2, 0) * lookAtDirection;    // 左端（正規化）
        Gizmos.color = Color.cyan;  // 正面は水色で描く
        Gizmos.DrawRay(this.transform.position, lookAtDirection * viewDistance);
        Gizmos.color = Color.blue;  // 両端は青で描く
        Gizmos.DrawRay(this.transform.position, rightBorder * viewDistance);
        Gizmos.DrawRay(this.transform.position, leftBorder * viewDistance);
    }

    //頭を動かすための関数
    public void MoveHead()
    {
       if (animator == null) return;
        animator.SetLookAtPosition(perspective.position);
        animator.SetLookAtWeight(1);

    }
    //視点を動かすための関数
    public void MovePerspective()
    {
        isTweening = true;
        seq.Append(transform.DORotate(new Vector3(0, -60, 0), 0.8f).SetRelative())
        .Append(transform.DORotate(new Vector3(0, 120, 0), 1.6f).SetRelative())
        .Append(transform.DORotate(new Vector3(0, -60, 0), 0.8f).SetRelative())
        .OnComplete(() =>
        {
            isTweening = false;
        });
    }
    //視点をプレイヤーの方に動かすための関数
    public void MovePerspectiveToPlayer()
    {
        if (player == null) return;
        if (!IsPlayerInSight()) return;
        Vector3 targetPosition = player.transform.position;
        seq.Append(perspective.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutSine));
    }

}
