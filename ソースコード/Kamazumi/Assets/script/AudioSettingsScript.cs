using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider MicrophoneSlider;
    //マウス感度のスライダー
    [SerializeField] Slider　MouseSensitivitySlider;
    // Start is called before the first frame update
    void Start()
    {
        //スライダーの値を保存データから読み込む
        BGMSlider.value = SaveDataScript.LoadBGMVolume();
        SESlider.value = SaveDataScript.LoadSEVolume();
        MicrophoneSlider.value = SaveDataScript.LoadMicrophoneVolume();
        MouseSensitivitySlider.value = SaveDataScript.LoadMouseSensitivity();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //スライダーの値を保存データに保存する
    public void OnBGMVolumeChanged()
    {
        SaveDataScript.SaveBGMVolume(BGMSlider.value);
        SaveDataScript.SaveSEVolume(SESlider.value);
        SaveDataScript.SaveMicrophoneVolume(MicrophoneSlider.value);
        SaveDataScript.SaveMouseSensitivity(MouseSensitivitySlider.value);
    }
}
