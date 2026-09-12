using DG.Tweening;
using UnityEngine;

public class HingedDoor : DoorScript
{
    BoxCollider boxCollider;
    //回転値
    [SerializeField]
    float rotationValue = 90f;
    //開閉速度
    [SerializeField]
    float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void CloseDoor()
    {
        var sequence = DOTween.Sequence();
        boxCollider.enabled = false;
        isTouching = true;
        sequence.Append(transform.DORotate(new Vector3(0, -rotationValue, 0), speed, RotateMode.LocalAxisAdd))
            .AppendCallback(() =>
            {
                isOpen = false;
                isTouching = false;
                boxCollider.enabled = true;
            });
    }
    
    public override void OpenDoor()
    {
        var sequence = DOTween.Sequence();
        boxCollider.enabled = false;
        isTouching = true;
        sequence.Append(transform.DORotate(new Vector3(0, rotationValue, 0), speed, RotateMode.LocalAxisAdd))
            .AppendCallback(() =>
            {
                isOpen = true;
                isTouching = false;
                boxCollider.enabled = true;

            });
    }
}
