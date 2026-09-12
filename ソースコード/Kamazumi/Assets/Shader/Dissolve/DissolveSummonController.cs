using System.Collections;
using UnityEngine;

public class DissolveSummonController : MonoBehaviour
{
    [Header("ディゾルブ用マテリアルのテンプレート")]
    [SerializeField] private Material dissolveTemplateMaterial;

    [Header("出現（サモン）設定")]
    [SerializeField] private float summonTime = 1.5f;       // 出現にかかる時間
    [SerializeField] private float waitTime = 1.0f;         // 出現が始まるまでの待ち時間

    [Header("消滅（ディゾルブ）設定")]
    [SerializeField] private float dissolveDuration = 2.0f; // 消滅にかかる時間
    [SerializeField] private bool destroyOnDissolve = true; // 消滅完了後にオブジェクトを破棄するか

    private Renderer targetRenderer;
    private Material[] originalMaterials;
    private Material[] clonedMaterials;

    private int widthID;
    private int cutoffID;

    void Start()
    {
        // 子オブジェクトを含めてRendererを探す（元のサモンの設計を踏襲）
        targetRenderer = GetComponentInChildren<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogError("Rendererが見つかりません。");
            enabled = false;
            return;
        }

        widthID = Shader.PropertyToID("_Width");
        cutoffID = Shader.PropertyToID("_CutOff");

        if (dissolveTemplateMaterial == null)
        {
            Debug.LogError("ディゾルブマテリアルのテンプレートが指定されていません。");
            return;
        }

        // 1. 元のマテリアルからテクスチャ等の見た目を引き継いだ、ディゾルブ用のクローンマテリアルを生成
        SetupClonedMaterials();

        // 2. 召喚（出現）アニメーションを開始
       // StartCoroutine(SummonRoutine());
    }

    /// <summary>
    /// 元のマテリアルの外見情報を維持したまま、ディゾルブ用マテリアルに差し替えます
    /// </summary>
    private void SetupClonedMaterials()
    {
        originalMaterials = targetRenderer.sharedMaterials;
        clonedMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            Material original = originalMaterials[i];

            // テンプレートからディゾルブマテリアルを複製（アセット破壊防止）
            Material clone = Instantiate(dissolveTemplateMaterial);

            // 元のマテリアルのメインテクスチャ（画像）を引き継ぐ
            if (original.HasProperty("_MainTex"))
            {
                clone.SetTexture("_MainTex", original.GetTexture("_MainTex"));
            }

            // 元のカラーを引き継ぐ (Standardシェーダーの _Color から ディゾルブの _MainColor へ)
            if (original.HasProperty("_Color"))
            {
                clone.SetColor("_MainColor", original.GetColor("_Color"));
            }

            // 元のマテリアルの凹凸（法線マップ）を引き継ぐ
            if (original.HasProperty("_BumpMap"))
            {
                clone.SetTexture("_BumpMap", original.GetTexture("_BumpMap"));
            }

            // 初期状態：完全に見えない状態（CutOff=1, Width=1）に設定
            clone.SetFloat(cutoffID, 1f);
            clone.SetFloat(widthID, 1f);

            clonedMaterials[i] = clone;
        }

        // レンダーにクローンマテリアル群をセット
        targetRenderer.sharedMaterials = clonedMaterials;
    }

    /// <summary>
    /// 召喚（出現）アニメーションのコルーチン
    /// </summary>
    private IEnumerator SummonRoutine()
    {
        // 指定の待ち時間だけ待機
        yield return new WaitForSeconds(waitTime);

        float elapsedTime = 0f;
        float halfTime = summonTime / 4f * 3f; // 調整用のしきい値の比率

        // アニメーション処理
        while (elapsedTime < summonTime)
        {
            elapsedTime += Time.deltaTime;
            float remaining = summonTime - elapsedTime; // 残り時間

            // しきい値のアニメーション（前半部分で 1 から 0 へ）
            float cutoff = (remaining - halfTime) / halfTime;
            cutoff = Mathf.Max(0f, cutoff);

            // 幅のアニメーション（後半部分で 1 から 0 へ）
            float width = (halfTime - remaining) / halfTime;
            width = 1f - Mathf.Max(0f, width);

            // すべてのサブマテリアルに反映
            SetMaterialsFloat(cutoffID, cutoff);
            SetMaterialsFloat(widthID, width);

            yield return null; // 1フレーム待つ
        }

        // 最後に完全に表示状態（0）に固定する
        SetMaterialsFloat(cutoffID, 0f);
        SetMaterialsFloat(widthID, 0f);
        if (destroyOnDissolve)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 消滅（ディゾルブ）効果を開始します（外部スクリプトなどからも呼び出せます）
    /// </summary>
    public void StartDissolve()
    {
        StartCoroutine(SummonRoutine());
    }

    /// <summary>
    /// 消滅（ディゾルブ）アニメーションのコルーチン
    /// </summary>
    private IEnumerator AnimateDissolve()
    {
        float elapsedTime = 0f;
        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / dissolveDuration);

            // すべてのマテリアルのしきい値（_CutOff）を 0 から 1 に更新
            SetMaterialsFloat(cutoffID, progress);

            yield return null; // 1フレーム待つ
        }

        // 完全に消滅した後、設定が有効ならオブジェクトを削除
        if (destroyOnDissolve)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 複製したすべてのマテリアルに一括で値をセットするヘルパー関数
    /// </summary>
    private void SetMaterialsFloat(int propertyID, float value)
    {
        if (clonedMaterials == null) return;

        foreach (var mat in clonedMaterials)
        {
            if (mat != null)
            {
                mat.SetFloat(propertyID, value);
            }
        }
    }

    //

    // テスト用の処理：スペースキーを押すと消滅が開始します
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(SummonRoutine());
        //    //StartDissolve();
        //}
    }
    

}