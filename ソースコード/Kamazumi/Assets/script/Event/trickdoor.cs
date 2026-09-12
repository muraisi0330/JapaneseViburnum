using UnityEngine;
using DG.Tweening;

public class trickdoor : MonoBehaviour
{
    public GameObject[] passwordObject;
    public AudioClip startupVoice;
    public float moveDistance = 4f;
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
        foreach(GameObject obj in passwordObject)
        {
            if (!obj.GetComponent<rotatingimage>().isTouched) return;
        }
            if(touching == true)
            {
            if (audioSource != null) {
                audioSource.PlayOneShot(startupVoice);
            }
               
                sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, moveDistance, 0), 1f));
                touching = false;
            }
        foreach (GameObject obj in passwordObject)
        {
           obj.tag = "Untagged";
        }

        //else if (touching == false)
        //{
        //    audioSource.PlayOneShot(startupVoice);
        //    sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -moveDistance, 0), 1f));
        //    touching = true;
        //}

    }
}
