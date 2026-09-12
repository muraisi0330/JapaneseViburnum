using DG.Tweening;
using UnityEngine;

public class houseDoor : InteractionEvents
{
    //テレポート座標
    [SerializeField] private Transform teleportPoint;
    [SerializeField]
    private GameObject Fade_in;
    [SerializeField]
    private GameObject　stage;
    GameObject player;

    [SerializeField]
    TaskUIScript taskUIScript;
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
        Fade_in.SetActive(true);
            stage.SetActive(true);
            var sequence = DOTween.Sequence();
             sequence.AppendInterval(2f)
            .OnComplete(() =>
             {
                 player.transform.position = teleportPoint.position;
                 player.transform.rotation = teleportPoint.rotation;
                 taskUIScript.UpdateTaskUI(2);
             });
         
    }
}
