using UnityEngine;
using DG.Tweening;

public class EnemyRaySensing : MonoBehaviour
{
    [Header("Ray")]
    [SerializeField]
    private Transform rayOrigin;

    [SerializeField]
    private float rayLength = 2f;

    [Header("Door")]
    [SerializeField]
    private float doorWaitTime = 1f;

    private DoorScript targetDoor;

    // 扉を開ける処理中か
    private bool isOpeningDoor = false;



    private void Update()
    {
        if (rayOrigin == null)
            return;

        // Ray作成
        Ray ray = new Ray(
            rayOrigin.position,
            rayOrigin.forward
        );

        // Ray表示
        Debug.DrawRay(
            ray.origin,
            ray.direction * rayLength,
            Color.red
        );

        // 扉がなければ終了
        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            rayLength))
        {
            targetDoor = null;
            return;
        }

        // 扉を探す
        DoorScript door =
            hit.collider.GetComponentInParent<DoorScript>();

        if (door == null)
        {
            targetDoor = null;
            return;
        }

        // 無効なDoorScriptなら終了
        if (!door.enabled)
            return;

        // 同じ扉を検出
        targetDoor = door;

        // すでに開ける処理中なら何もしない
        if (isOpeningDoor)
            return;

        OpenDoor();
    }

    private void OpenDoor()
    {
        if (targetDoor == null)
            return;

        isOpeningDoor = true;

        // 扉を開く
        targetDoor.enemyEvents();

        // 1秒待つ
        DOVirtual.DelayedCall(doorWaitTime, () =>
        {
            isOpeningDoor = false;
        });
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("InteractionObject"))
    //    {
    //        DoorScript door = other.GetComponentInParent<DoorScript>();
    //        if (door != null)
    //        {
    //            door.enemyEvents();
    //        }
    //    }
    //}
}


