using DG.Tweening;
using UnityEngine;

public class SlidingDoor : DoorScript
{
    [SerializeField]
    float speed = 1f;
    //扉の移動距離
    [SerializeField]
    float moveDistance = 1f;
    AudioSource audioSource;

    float originalX;

    // Start is called before the first frame update
    void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }

    public override void CloseDoor()
    {
        var sequence = DOTween.Sequence();
        isTouching = true;
        float targetX = transform.localPosition.x - moveDistance;
        //SEを鳴らす
        audioSource.Play();
        sequence.Append(transform.DOLocalMoveX(targetX, speed))
            .AppendCallback(() =>
            {
                isOpen = false;
                isTouching = false;
              
            });
    }
    public override void OpenDoor()
    {
        var sequence = DOTween.Sequence();
        isTouching = true;
        float targetX = transform.localPosition.x + moveDistance;

        audioSource.Play();
        //扉をあけてプレイヤーを扉の奥に隠してから扉をしめる
        sequence.Append(transform.DOLocalMoveX(targetX, speed))
            .AppendCallback(() =>
            {
                isOpen = true;
                isTouching = false;
               
            });
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("hand")) return;
    //    if (isTouching) return;
    //    OpenAndClose();

    //}
}
