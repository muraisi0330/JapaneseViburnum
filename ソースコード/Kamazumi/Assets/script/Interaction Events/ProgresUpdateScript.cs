using UnityEngine;

public class ProgresUpdateScript : InteractionEvents
{
    [SerializeField]
    TaskUIScript taskUIScript;

   

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
        taskUIScript.UpdateTaskUI();
    }
}
