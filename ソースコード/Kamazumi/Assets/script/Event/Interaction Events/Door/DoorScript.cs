using DG.Tweening;
using UnityEngine;

public class DoorScript : InteractionEvents
{
    public bool isOpen = false;//扉が開いているかどうか
    public bool isTouching = false;//扉が動いているかどうか
    public bool isLocked = false; //ロックされているかどうか
    //プレイヤーの手が触れているかどうか
    public bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void interactionEvents()
    {
        isMoving = true;
        OpenAndClose();
    }

    public void enemyEvents()
    {
        if (isLocked) return;
        if (isTouching != false) return;
        if (isOpen) return;
        isMoving = false;
        OpenAndClose();
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(2)
           .AppendCallback(() =>
           {
               OpenAndClose();
           });
    }


    //開け閉め
    public void OpenAndClose() 
    {
        if(isLocked) return;
        if (isTouching != false) return;
        if (isOpen == false) OpenDoor();
        else CloseDoor();
    }
    //
    public virtual void OpenDoor()
    {
    }
  
    public virtual void CloseDoor()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("hand")) return;
        if (isTouching) return;
        if (isOpen) return;
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
                {
                    OpenDoor();
                })
                .AppendInterval(3)
                .AppendCallback(() =>
                 {
                     CloseDoor();
                 });

    }
}
