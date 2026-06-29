using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class houseDoor : InteractionEvents
{
    [SerializeField] private GameObject key;
    public Text countText;
    public GameObject textToDisplay;
    //テレポート座標
    [SerializeField] private Transform teleportPoint;
    [SerializeField]
    private GameObject Fade_in;
    [SerializeField]
    private GameObject　stage;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void interactionEvents()
    {
        if (key.activeSelf == false)
        {
            Fade_in.SetActive(true);
            stage.SetActive(true);
            var sequence = DOTween.Sequence();
             sequence.AppendInterval(2f)
            .OnComplete(() =>
             {
                 player.transform.position = teleportPoint.position;
                 player.transform.rotation = teleportPoint.rotation;
             });
         
        }
        else
        {
            countText.text = key.gameObject.name + "が必要です。";
            textToDisplay.SetActive(true);
        }
    }
}
