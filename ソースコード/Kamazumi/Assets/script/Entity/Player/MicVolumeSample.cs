using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MicVolumeSample : VolumeSample
{
    [Header("Microphone")]//マイク
    [SerializeField, Range(0f, 10f)]
    private float gain = 1f;

    [Header("Noise Filter")]//ノイズフィルター
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float voiceDetectTime = 0.4f;
    //周波数帯
    [SerializeField] private float voiceBandMinFreq = 85f;//85Hz～1000Hzの周波数帯を声帯として判定
    [SerializeField] private float voiceBandMaxFreq = 1000f;//85Hz～1000Hzの周波数帯を声帯として判定
    [SerializeField] private float enterThreshold = 0.08f;//声として判定する音量の閾値
    [SerializeField] private float exitThreshold = 0.04f;//声として判定を解除する音量の閾値
    [SerializeField] private float calibrationTime = 3f;//初回ノイズキャリブレーションの時間
    [SerializeField] private int peakFilterSize = 10;//短時間ピーク除去の移動平均のサンプル数
    [SerializeField] private float voiceBandThreshold = 0.0001f;//声帯周波数帯の音量の閾値
    [SerializeField]private int zeroCrossThreshold = 100;//声らしさ判定のゼロクロス数の閾値

    private float ambientNoise;
    private bool calibrated;
    private float calibrationTimer;
    private int calibrationCount;

    private Queue<float> volumeHistory = new();

    private bool voiceState;

    private int zeroCrossCount;

    private float historySum;

    private float targetVolume;
    private float voiceTimer;
    private bool isVoiceDetected;

    private AudioSource micAudioSource;
    private readonly float[] spectrum = new float[512];

   
   
    KeywordTest keywordTests = null;

    private void Start()
    {
        micAudioSource = GetComponent<AudioSource>();
        InitializeMicrophone();
    }

    private void Update()
    {
        // マイク感度の更新
        UpdateGain();
        // 音声入力の処理
        ProcessVoiceInput();
        // UIの更新
        UpdateVolumeUI();
        // 音源の表示・非表示の更新
        ifShowSoundSource();
        HideSoundSource();
        // 危険エフェクトの更新
        UpdateDangerEffect();
        // 音声コマンドの処理
        VoiceCommand();
    }

    // Microphone

    private void InitializeMicrophone()
    {
        // 既存の取得処理
        micAudioSource = GetComponent<AudioSource>(); // クラス変数に確実に代入

        if (micAudioSource == null || Microphone.devices.Length == 0)
        {
            Debug.LogError("マイクデバイスが見つからないか、AudioSourceがありません。");
            return;
        }

        string deviceName = Microphone.devices[0];
        //Debug.Log($"使用するマイク: {deviceName}");

        int minFreq;
        int maxFreq;
        Microphone.GetDeviceCaps(deviceName, out minFreq, out maxFreq);

        // サンプリングレートの決定（0の場合は44100などをデフォルトにする）
        int sampleRate = minFreq == 0 ? 44100 : minFreq;

        micAudioSource.clip = Microphone.Start(deviceName, true, 1, sampleRate);

        // 【重要】録音が開始されるまで待つコルーチンを開始
        StartCoroutine(WaitForMicrophoneStart());
    }

    private System.Collections.IEnumerator WaitForMicrophoneStart()
    {
        // Microphone.GetPosition が 0 を超えるまで待つ（録音開始の合図）
        while (!(Microphone.GetPosition(Microphone.devices[0]) > 0))
        {
            yield return null;
        }

        micAudioSource.Play();
       // Debug.Log("マイクの録音・再生が正常に開始されました。");
    }
    private void ProcessVoiceInput()
    {
        // 音量平滑化
        volumeRate = Mathf.Lerp(
            volumeRate,
            targetVolume,
            smoothSpeed * Time.deltaTime);

        // 初回ノイズキャリブレーション
        if (!calibrated)
        {
            UpdateNoiseCalibration();
            return;
        }

       // ノイズ除去
       float cleanVolume =
            Mathf.Max(0f, volumeRate - ambientNoise);
       // 短時間ピーク除去（移動平均）
       float averageVolume =
            GetSmoothedVolume(cleanVolume);

        // 声らしさ判定
        bool voiceLikeWave =
            zeroCrossCount >= zeroCrossThreshold;

        bool voiceBandDetected =
            GetVoiceBandVolume() >= voiceBandThreshold;

        // ヒステリシス
        UpdateVoiceState(
            averageVolume,
            voiceLikeWave,
            voiceBandDetected);

        // 継続判定
        UpdateVoiceTimer();
    }
    private void UpdateNoiseCalibration()
    {
        calibrationTimer += Time.deltaTime;

        // 喋っている時は学習しない
        if (volumeRate < enterThreshold)
        {
            ambientNoise += volumeRate;
            calibrationCount++;
        }

        if (calibrationTimer < calibrationTime)
            return;

        ambientNoise =
            calibrationCount > 0
            ? ambientNoise / calibrationCount
            : 0f;

        calibrated = true;

        //Debug.Log(
        //    $"Noise Calibration Complete : {ambientNoise:F4}");
    }

   

    private float GetSmoothedVolume(float volume)
    {
        volumeHistory.Enqueue(volume);
        historySum += volume;

        if (volumeHistory.Count > peakFilterSize)
        {
            historySum -= volumeHistory.Dequeue();
        }

        return historySum / volumeHistory.Count;
    }
    private void UpdateVoiceState(
    float averageVolume,
    bool voiceLikeWave,
    bool voiceBandDetected)
    {
        if (!voiceState)
        {
            if (averageVolume >= enterThreshold &&
                voiceLikeWave &&
                voiceBandDetected)
            {
                voiceState = true;
            }
        }
        else
        {
            if (averageVolume <= exitThreshold)
            {
                voiceState = false;
            }
        }
    }

    private void UpdateVoiceTimer()
    {
        if (!voiceState)
        {
            voiceTimer = 0f;
            isVoiceDetected = false;
            return;
        }

        voiceTimer += Time.deltaTime;

        if (voiceTimer >= voiceDetectTime)
        {
            isVoiceDetected = true;
        }
    }




    private float GetVoiceBandVolume()
    {
        if (micAudioSource == null)
            return 0f;
        // 周波数スペクトルを取得
        micAudioSource.GetSpectrumData(
            spectrum,
            0,
            FFTWindow.BlackmanHarris);

        float sampleRate = AudioSettings.outputSampleRate;

        float volume = 0f;
        // 指定した周波数帯の音量を計算
        for (int i = 0; i < spectrum.Length; i++)
        {
            float freq =
                i * sampleRate / 2f / spectrum.Length;

            if (freq >= voiceBandMinFreq && freq <= voiceBandMaxFreq)
            {
                volume += spectrum[i];
            }
        }

        return volume;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        float sum = 0f;
        int zeroCross = 0;

        for (int i = 0; i < data.Length; i++)
        {
            sum += Mathf.Abs(data[i]);

            if (i > 0)
            {
                bool prevPositive = data[i - 1] > 0;
                bool currentPositive = data[i] > 0;

                if (prevPositive != currentPositive)
                {
                    zeroCross++;
                }
            }
        }

        zeroCrossCount = zeroCross;

        float volume =
            sum * gain / data.Length;

        targetVolume = volume;
    }

    private void UpdateGain()
    {
        gain = SaveDataScript.LoadMicrophoneVolume(); 
    }

    private void UpdateVolumeUI()
    {
        if (volumeMeterSlider == null) return;

        volumeMeterSlider.value = volumeRate * 100f;
    }

    //// Enemy Hearing

    //private void OnTriggerStay(Collider other)
    //{

    //    VoiceCommandRange(other);
    //}

    //// 音声コマンド範囲
    //void VoiceCommandRange(Collider other)
    //{
    //    if (!other.CompareTag("InteractionObject")) return;
    //    keywordTests = other.GetComponent<KeywordTest>();
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    DisableVoiceCommandRange(other);
    //}

    //void DisableVoiceCommandRange(Collider other)
    //{
    //    if (!other.CompareTag("InteractionObject")) return;
    //    keywordTests = null;
    //}

    // Sound Source

    private void ifShowSoundSource()
    {
        if (!isEnemyCanHear) return;
        // マイク感度の閾値
        if (!isVoiceDetected) return;

        ShowSoundSource();
    }

   

    // 音声コマンド
    private void VoiceCommand()
    {
        if (keywordTests == null) return;
        if (!isVoiceDetected) return;
      //  keywordTests.interactionEvents();
    }

  
    
    // Debug UI
    //void OnGUI()
    //{
    //    GUI.Label(
    //        new Rect(20, 20, 500, 30),
    //        $"Volume : {volumeRate:F4}");

    //    GUI.Label(
    //        new Rect(20, 50, 500, 30),
    //        $"Ambient : {ambientNoise:F4}");

    //    GUI.Label(
    //        new Rect(20, 80, 500, 30),
    //        $"ZeroCross : {zeroCrossCount}");

    //    GUI.Label(
    //        new Rect(20, 110, 500, 30),
    //        $"Voice : {isVoiceDetected}");

    //    GUI.Label(
    //        new Rect(20, 140, 500, 30),
    //        $"VoiceTimer : {voiceTimer:F2}");
    //}
}