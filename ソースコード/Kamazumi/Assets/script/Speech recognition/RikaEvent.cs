
using UnityEngine;

public class RikaEvent : KeywordTest
{
    // Start is called before the first frame update

    [SerializeField] AudioSource audioSource;
    // Update is called once per frame
  
    public override void Events()
    {
       audioSource.Play();
      Debug.Log("RikaEvent");
    }
}
