using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class VolumeSample : MonoBehaviour
{
    public float volumeRate { get; protected set; }

    [Header("UI")]//UI
    [SerializeField] protected Slider volumeMeterSlider;

    [Header("References")]//参照
    [SerializeField] private Transform enemy;
    [SerializeField] private GameObject soundSource;

    [Header("Audio")]//オーディオ
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource seAudio;

    [Header("Post Process")]//ポストプロセス
    [SerializeField] private PostProcessVolume postProcessVolume;


    private Vignette vignette;

    protected bool isEnemyCanHear;
    private float hearingDistance = 20f;
    private float hideTimer;

    private Tween bgmFadeTween;

    bool isSoundSourceActive;


    protected float enemyHearingThreshold = 1f;
    private void Start()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
        }
    }


    // Enemy Hearing

    private void OnTriggerStay(Collider other)
    {
        EnemyHearing(other);
    }
    void EnemyHearing(Collider other)
    {
        if (!other.CompareTag("ear")) return;
        if (isEnemyCanHear) return;
        isEnemyCanHear = true;
        FadeOutBGM();
    }
  
    private void OnTriggerExit(Collider other)
    {
        DisableEnemyHearing(other);
    }
    void DisableEnemyHearing(Collider other)
    {
        if (!other.CompareTag("ear")) return;
        if (!isEnemyCanHear) return;
        isEnemyCanHear = false;
        FadeInBGM();
    }
  

    public void ShowSoundSource()
    {
        Vector3 playerPos = this.transform.position;

        float distance =
            Vector3.Distance(
                enemy.position,
                playerPos);
        if (volumeRate < enemyHearingThreshold) return;// マイク感度の閾値を1fに設定
        hearingDistance =
            25f - (distance / 60f * 10f);

        hearingDistance =
            Mathf.Clamp(
                hearingDistance,
                3f,
                25f);
        SetHeardPoint(new Vector3(playerPos.x, 0f, playerPos.z));

        hideTimer = 0f;

        seAudio.mute = false;
        isSoundSourceActive = true;

        if (!soundSource.activeSelf)
        {
            soundSource.SetActive(true);
        }
    }

    public void HideSoundSource()
    {
        if (!isSoundSourceActive) return;

        hideTimer += Time.deltaTime;
        IsSoundSourceActive();
        if (hideTimer < hearingDistance) return;

     
        seAudio.mute = true;
        soundSource.SetActive(false);


        if (vignette == null) return;
        vignette.intensity.value = 0f;
    }
    //soundSourceがアクティブかどうかを返す
    public void IsSoundSourceActive()
    {
        if (soundSource.activeSelf) return;
        hideTimer = 0f;
        isSoundSourceActive = false;
        seAudio.mute = true;

    }


    // Danger Effect
    public void UpdateDangerEffect()
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

}
