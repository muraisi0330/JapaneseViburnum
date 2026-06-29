using DG.Tweening;
using UnityEngine;

public class locker : DoorScript
{
     BoxCollider boxCollider;
    public MeshCollider hitJudgment;
    GameObject player;
    Transform behindTheDoor;
    public Transform inFrontOfTheDoor;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        behindTheDoor = transform.parent.gameObject.transform;
    }
    public override void CloseDoor()
    {
        EventsDesignedToReleaseThePlayer();
    }
    public override void OpenDoor()
    {
        EventsDesignedTohideThePlayer();
    }
    //プレイヤーを扉の奥に隠すためのイベント
    void EventsDesignedTohideThePlayer()
    {
        var sequence = DOTween.Sequence();
        boxCollider.enabled = false;
        isTouching = true;
        player.GetComponent<Player>().isHiding = true;
        //扉をあけてプレイヤーを扉の奥に隠してから扉をしめる
        sequence.Append(transform.DORotate(new Vector3(0, -90f, 0), 1f, RotateMode.LocalAxisAdd))
            .Append(player.transform.DOLocalMove(new Vector3(behindTheDoor.position.x, player.transform.position.y, behindTheDoor.position.z),0.5f))
            .AppendCallback(() =>
            {
                 //プレイヤーを扉の方に向ける
                player.transform.rotation = Quaternion.Euler(0, behindTheDoor.rotation.eulerAngles.y, 0);

            })
            .AppendInterval(1)
            .Append(transform.DORotate(new Vector3(0, 90f, 0), 1f, RotateMode.LocalAxisAdd))
            .AppendCallback(() =>
            {
                isOpen = true;
                isTouching = false;
                boxCollider.enabled = true;

            });

    }
    //プレイヤーを扉から出すためのイベント
    void EventsDesignedToReleaseThePlayer()
    {
        var sequence = DOTween.Sequence();
        boxCollider.enabled = false;
        isTouching = true;
        sequence.Append(transform.DORotate(new Vector3(0, -90f, 0), 1f, RotateMode.LocalAxisAdd))
             .Append(player.transform.DOLocalMove(new Vector3(inFrontOfTheDoor.position.x, player.transform.position.y, inFrontOfTheDoor.position.z), 0.5f))
            .AppendCallback(() =>
            {
               //プレイヤーを扉の方に向ける
                player.transform.rotation = Quaternion.Euler(0, inFrontOfTheDoor.rotation.eulerAngles.y, 0);
            })
            .AppendInterval(1)
            .Append(transform.DORotate(new Vector3(0, 90f, 0), 1f, RotateMode.LocalAxisAdd))
            .AppendCallback(() =>
            {
                isOpen = false;
                isTouching = false;
                boxCollider.enabled = true;
                player.GetComponent<Player>().isHiding = false;
            });
    }



    private void OnTriggerEnter(Collider other)
    {
        var sequence = DOTween.Sequence();
        if (other.CompareTag("hand"))
        {
            if (isTouching == false)
            {
                boxCollider.enabled = false;
              
                sequence.Append(transform.DORotate(new Vector3(0, -90f, 0), 1f, RotateMode.LocalAxisAdd))
                    .AppendCallback(() =>
                {
                    isOpen = false;
                    isTouching = false;
                    boxCollider.enabled = true;
                    hitJudgment.enabled = false;
                })
                    .AppendInterval(1)
                    .AppendCallback(() =>
                    {
                        hitJudgment.enabled = true;
                    });
            }
        }
    }
}
