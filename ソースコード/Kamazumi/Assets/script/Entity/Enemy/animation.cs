using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class animation : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip footstepSound;

    [SerializeField]
    IoadingScreenScript ioadingScreenScript;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
    public void PlayFootstepSound2()
    {
        ioadingScreenScript.StartLoad("GameOverScene");
    }
}
