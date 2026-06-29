using UnityEngine;
using DG.Tweening;

public class trickdoor : MonoBehaviour
{
    public GameObject[] passwordObject;
    public AudioClip startupVoice;
    AudioSource audioSource;
    bool touching = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
          Renderer renderer = GetComponent<Renderer>();
        var sequence = DOTween.Sequence();
        if (passwordObject[0].GetComponent<rotatingimage>().isTouched && passwordObject[1].GetComponent<rotatingimage>().isTouched)
        {
            if(touching == true)
            {
                audioSource.PlayOneShot(startupVoice);
                sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, 4f, 0), 1f));
                touching = false;
            }
          
        }
        else if (touching == false)
        {
            audioSource.PlayOneShot(startupVoice);
            sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -4f, 0), 1f));
            touching = true;
        }
      
    }
}
