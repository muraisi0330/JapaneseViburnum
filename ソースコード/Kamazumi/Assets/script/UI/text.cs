using UnityEngine;

public class text : MonoBehaviour
{
    public float displayDelay = 2.0f; // 表示までの待機時間（秒）

    private float timer;

    void Start()
    {
        timer = 0.0f;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf != true) return;
       
        timer += Time.deltaTime; // 経過時間をカウント

        if (timer < displayDelay) return;

        // 一定時間経過したらオブジェクトを表示
        this.gameObject.SetActive(false);
        timer = 0.0f;

    }
}
