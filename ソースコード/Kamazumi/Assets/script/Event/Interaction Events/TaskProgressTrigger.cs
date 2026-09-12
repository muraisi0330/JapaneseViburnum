using UnityEngine;

public class TaskProgressTrigger : InteractionEvents
{
    [SerializeField]
    TaskUIScript taskUIScript;

    [SerializeField]
    int taskID;

    bool isEnabled = false;

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
        if (isEnabled) return;
        taskUIScript.UpdateTaskUI(taskID);
        isEnabled = true;
    }
}
