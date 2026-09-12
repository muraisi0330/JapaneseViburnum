using DG.Tweening;
using UnityEngine;

public class StandardDoor : DoorScript
{
    public AudioClip sound1;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
     
    }
    public override void CloseDoor()
    {
        if (!isOpen) return;
        var sequence = DOTween.Sequence();
        isTouching = true;
        audioSource.PlayOneShot(sound1);
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -4f, 0), 1f))
           .AppendCallback(() =>
           {
               isOpen = false;
               isTouching = false;
           });

    }
    public override void OpenDoor()
    {
        if (isOpen) return;
        var sequence = DOTween.Sequence();
        isTouching = true;
        audioSource.PlayOneShot(sound1);
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, 4f, 0), 1f))
            .AppendCallback(() =>
            {
                isOpen = true;
                isTouching = false;
            });
    }
}
