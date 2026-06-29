using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MicVolumeSample : MonoBehaviour
{
    [Header("Microphone")]
    [SerializeField, Range(0f, 10f)]
    private float gain = 1f;

    private float volumeRate;

    [Header("UI")]
    [SerializeField] private Slider volumeMeterSlider;
    [SerializeField] private Slider gainSlider;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private GameObject soundSource;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource seAudio;

    [Header("Post Process")]
    [SerializeField] private PostProcessVolume postProcessVolume;

    [Header("Noise Filter")]
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float voiceDetectTime = 0.4f;
    //周波数帯
    [SerializeField] private float voiceBandMinFreq = 85f;
    [SerializeField] private float voiceBandMaxFreq = 1000f;
    [SerializeField] private float enterThreshold = 0.08f;
    [SerializeField] private float exitThreshold = 0.04f;
    [SerializeField] private float calibrationTime = 3f;
    [SerializeField] private int peakFilterSize = 10;
    [SerializeField] private float voiceBandThreshold = 0.0001f;
    [SerializeField]private int zeroCrossThreshold = 100;

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

    private Vignette vignette;

    private bool isEnemyCanHear;
    private float hearingDistance = 20f;
    private float hideTimer;

    private Tween bgmFadeTween;

    bool isSoundSourceActive;

    private void Start()
    {
        micAudioSource = GetComponent<AudioSource>();

        gainSlider.value = SaveDataScript.LoadMicrophoneVolume();

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
        }

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
        ShowSoundSource();
        HideSoundSource();
        // 危険エフェクトの更新
        UpdateDangerEffect();
    }

    // Microphone
  
    private void InitializeMicrophone()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null || Microphone.devices.Length == 0)
            return;

        string deviceName = Microphone.devices[0];

        int minFreq;
        int maxFreq;

        Microphone.GetDeviceCaps(
            deviceName,
            out minFreq,
            out maxFreq);

        audioSource.clip =
            Microphone.Start(
                deviceName,
                true,
                2,
                minFreq);

        audioSource.Play();
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

        Debug.Log(
            $"Noise Calibration Complete : {ambientNoise:F4}");
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

        micAudioSource.GetSpectrumData(
            spectrum,
            0,
            FFTWindow.BlackmanHarris);

        float sampleRate = AudioSettings.outputSampleRate;

        float volume = 0f;

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
        gain = gainSlider.value;
    }

    private void UpdateVolumeUI()
    {
        if (volumeMeterSlider == null) return;

        volumeMeterSlider.value = volumeRate * 100f;
    }
    
    // Enemy Hearing

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("ear")) return;

        if (!isEnemyCanHear)
        {
            isEnemyCanHear = true;
            FadeOutBGM();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ear")) return;

        isEnemyCanHear = false;
        FadeInBGM();
    }

    // Sound Source
    
    private void ShowSoundSource()
    {
        if (!isEnemyCanHear) return;

        // マイク感度の閾値
        if (!isVoiceDetected) return;

        if (volumeRate < 1f) return;

        Vector3 playerPos = player.position;

        float distance =
            Vector3.Distance(
                enemy.position,
                player.position);

        hearingDistance =
            25f - (distance / 60f * 10f);

        hearingDistance =
            Mathf.Clamp(
                hearingDistance,
                3f,
                25f);
        SetHeardPoint(new Vector3(playerPos.x,0f,playerPos.z));

        hideTimer = 0f;

        seAudio.mute = false;
        isSoundSourceActive = true; 

        if (!soundSource.activeSelf)
        {
            soundSource.SetActive(true);
        }
    }

    private void HideSoundSource()
    {
        if (!isSoundSourceActive) return;

        hideTimer += Time.deltaTime;

        if (hideTimer < hearingDistance) return;

      

        hideTimer = 0f;

        seAudio.mute = true;
        isSoundSourceActive = false;

        soundSource.SetActive(false);


        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }
    }
    // Reset Sound Source
  

    // Danger Effect
    private void UpdateDangerEffect()
    {
        if (vignette == null)
            return;

        if (!isEnemyCanHear || !isSoundSourceActive)
        {
            vignette.intensity.value = 0f;
            return;
        }

        vignette.intensity.value =
            Mathf.PingPong(
                Time.time * 0.5f,
                0.5f);
    }

    // BGM Fade
    private void FadeOutBGM()
    {
        bgmFadeTween?.Kill();

        bgmAudio.mute = false;

        bgmFadeTween =
            bgmAudio
            .DOFade(0f, 5f)
            .OnComplete(() =>
            {
                bgmAudio.mute = true;
            });
    }
    //BGMがフェードインする処理
    private void FadeInBGM()
    {
        bgmFadeTween?.Kill();

        bgmAudio.mute = false;

        bgmFadeTween =
            bgmAudio
            .DOFade(1f, 5f);
    }

    void SetHeardPoint(Vector3 rawPosition)
    {
        NavMeshHit hit;
        // rawPosition（壁の表面など）から半径2.0m以内の「NavMeshの上」の点を探す
        if (NavMesh.SamplePosition(rawPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            // 実際に行ける場所に座標をずらして設置
            soundSource.transform.position = hit.position;
        }
        else
        {
            soundSource.transform.position = rawPosition;
        }
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