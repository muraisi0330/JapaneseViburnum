using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMASEScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    //それぞれのスライダーを入れるとこです。。
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
  
    private void Start()
    {
        //ミキサーのvolumeにスライダーのvolumeを入れてます。
        BGMSlider.value = SaveDataScript.LoadBGMVolume();
        SESlider.value = SaveDataScript.LoadSEVolume();
        audioMixer.SetFloat("BGM", BGMSlider.value);
        audioMixer.SetFloat("SE", SESlider.value);
    }
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SE", volume);
    }
}
