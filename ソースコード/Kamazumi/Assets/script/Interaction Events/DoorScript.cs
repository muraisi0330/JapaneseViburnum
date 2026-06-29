using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractionEvents
{
    public bool isOpen = false;//扉が開いているかどうか
    public bool isTouching = false;//扉が動いているかどうか
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
        OpenAndClose();
    }

    //開け閉め
    public void OpenAndClose() 
    {
        if (isTouching != false) return;
        if (isOpen == false) OpenDoor();
        else CloseDoor();
    }
    
    public virtual void OpenDoor()
    {
    }
  
    public virtual void CloseDoor()
    {
    }

}
