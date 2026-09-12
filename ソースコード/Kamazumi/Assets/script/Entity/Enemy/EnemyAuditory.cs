using UnityEngine;

public class EnemyAuditory : MonoBehaviour
{
    [SerializeField] private GameObject soundSource;

    [SerializeField] private SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerStay(Collider other)
    {
        //AudioSourceをもっているかどうかを確認する
        if (other.gameObject.GetComponent<AudioSource>() == null) return;
       
        if (other.GetComponentInParent<DoorScript>() != null)
        {
            if (!other.GetComponentInParent<DoorScript>().isMoving) return;
        }
      
        AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
        if(audioSource.loop) return;
        if (!audioSource.isPlaying) return;
        //本体との距離を測る
        float distance = Vector3.Distance(transform.position,
            other.transform.position);
        //距離がちかすぎる場合は
        if (distance <= sphereCollider.radius/2) return;
        if (distance >= this.GetComponent<SphereCollider>().radius/2) return;

        soundSource.transform.position = other.transform.position;
        soundSource.SetActive(true);
        Debug.Log(other.tag);

    }

  

}
