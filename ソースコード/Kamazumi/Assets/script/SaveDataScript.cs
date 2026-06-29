using UnityEngine;

public static class SaveDataScript
{
    //オーディオ設定のキー
    public static class AudioSettings
    {
        public static string BGM = "BGM";
        public static string SE = "SE";
        public static string Microphone = "マイク";
    }
    //操作設定のキー
    public static class ControlSettings
    {
       //マウス感度の保存
        public static string MouseSensitivity = "MouseSensitivity";
    }


    //BGMの保存
    public static void SaveBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(AudioSettings.BGM, volume);
    }
    //BGMの読み込み
    public static float LoadBGMVolume()
    {
        return PlayerPrefs.GetFloat(AudioSettings.BGM, 1.0f);
    }
    //SEの保存
    public static void SaveSEVolume(float volume)
    {
        PlayerPrefs.SetFloat(AudioSettings.SE, volume);
    }
    //SEの読み込み
    public static float LoadSEVolume()
    {
        return PlayerPrefs.GetFloat(AudioSettings.SE, 1.0f);
    }
    //マイクの保存
    public static void SaveMicrophoneVolume(float volume)
    {
        PlayerPrefs.SetFloat(AudioSettings.Microphone, volume);
    }
    //マイクの読み込み
    public static float LoadMicrophoneVolume()
    {
        return PlayerPrefs.GetFloat(AudioSettings.Microphone, 5f);
    }


    //マウス感度の保存
    public static void SaveMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(ControlSettings.MouseSensitivity, sensitivity);
    }
    //マウス感度の読み込み
    public static float LoadMouseSensitivity()
    {
        return PlayerPrefs.GetFloat(ControlSettings.MouseSensitivity, 2.0f);
    }
}


