using UnityEngine;
using UnityEngine.Playables;

public class SurprisedScript : MonoBehaviour
{
    public PlayableDirector asdf;
    bool a = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!a) return;
                asdf.Play();
                a = false;
    }
}
