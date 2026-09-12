using UnityEngine;

public class PinEntryWindowTrigger : MonoBehaviour
{
    public GameObject objectToShow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectToShow.activeSelf) return;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
      
    }
  
}
