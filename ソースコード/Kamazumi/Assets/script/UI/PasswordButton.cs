using UnityEngine;
using UnityEngine.UI;
public class PasswordButton : MonoBehaviour
{
    public Text countText;
    public AudioClip sound1;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //パスワード入力の関数
    public void PasswordInput(int inputCharacters )
    {
        audioSource.PlayOneShot(sound1);
        countText.text += inputCharacters.ToString();
    }
    //パスワード削除の関数
    public void DeletePassword()
    {
        countText.text = "";
        audioSource.PlayOneShot(sound1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
