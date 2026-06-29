using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;




public class tobira : DoorScript
{
    [SerializeField] private GameObject key;
    public Text countText;
    public GameObject textToDisplay;
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
    public override void interactionEvents()
    {
        Renderer renderer = GetComponent<Renderer>();
        var sequence = DOTween.Sequence();

        if (key.activeSelf == false)
        {
            OpenAndClose();
        }
        else
        {
            countText.text = key.gameObject.name + "が必要です。";
            textToDisplay.SetActive(true);
        }
    }
    public override void CloseDoor()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        var sequence = DOTween.Sequence();

        if (other.CompareTag("hand"))
        {
            if (isTouching == false)
            {
                isTouching = true;
                sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, 4f, 0), 1f))
                .AppendInterval(3)
                .Append(transform.DOBlendableMoveBy(new Vector3(0, -4f, 0), 1f))
                .AppendCallback(() =>
                {
                    isTouching = false;
                });
            }
        }
    }
}
