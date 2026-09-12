using UnityEngine;


public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip footstepSound;

    [SerializeField]
    LoadingScreenManager ioadingScreenScript;

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
