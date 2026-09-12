using UnityEngine;

public class LightingTheBonfireBrazier : InteractionEvents
{
    public bool  isOnOff = false;
    [SerializeField] GameObject fireParticle;
    // Start is called before the first frame update
    void Start()
    {
        interactionEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void interactionEvents()
    {
        isOnOff = !isOnOff;
        if (isOnOff)
        {
            fireParticle.SetActive(true);
        }
        else
        {
            fireParticle.SetActive(false);
        }
    }
}
