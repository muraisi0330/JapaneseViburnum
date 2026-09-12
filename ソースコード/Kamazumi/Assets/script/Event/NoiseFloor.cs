using UnityEngine;

public class NoiseFloor : MonoBehaviour
{
    [Header("Audio")]//オーディオ
    [SerializeField] private AudioSource audioSource;
    [SerializeField][Range(0, 10)]  private int randomRange = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if(Random.Range(1, 10) > randomRange) return;
        audioSource.Play();
    }
}
