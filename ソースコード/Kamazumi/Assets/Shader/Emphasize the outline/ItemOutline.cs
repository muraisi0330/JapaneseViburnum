using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class ItemOutline : MonoBehaviour
{
    [Header("輪郭線マテリアル (SmoothOutlineShaderを使用)")]
    [SerializeField] private Material outlineMaterial;

    private Renderer targetRenderer;
    private Material[] normalMaterials;
    private Material[] outlineMaterials;
    private bool isOutlineActive = false;
   public bool isOutlineON = false;


    private Camera mainCamera;

    // 中央とみなす許容範囲（小さいほどシビアになります）
    [SerializeField] private float threshold = 0.1f;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
        if (outlineMaterial == null)
        {
            Debug.LogWarning($"{gameObject.name} に輪郭線マテリアルが設定されていません。");
            outlineMaterial = Resources.Load<Material>("Unlit_Outline");
            enabled = false;
            return;
        }

        // 1. スムース法線の生成と適用 (MeshFilter用)
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Mesh sharedMesh = meshFilter.sharedMesh;

            // Read/Write が有効になっている場合のみ処理を行う
            if (sharedMesh.isReadable)
            {
                Mesh uniqueMesh = Instantiate(sharedMesh);
                BakeSmoothNormalsToVertexColors(uniqueMesh);
                meshFilter.mesh = uniqueMesh;
            }
            else
            {
                // 有効でない場合は警告を出し、通常の輪郭線（角が少し割れる状態）で代用する
                //Debug.LogWarning($"モデル '{sharedMesh.name}' の Read/Write が無効です。プロジェクト設定で有効にするか、通常の法線で輪郭線を描画します。");
            }
        }

        // 2. マテリアル配列の準備
        normalMaterials = targetRenderer.sharedMaterials;
        outlineMaterials = new Material[normalMaterials.Length + 1];
        for (int i = 0; i < normalMaterials.Length; i++)
        {
            outlineMaterials[i] = normalMaterials[i];
        }
        outlineMaterials[outlineMaterials.Length - 1] = outlineMaterial;
    }

    /// <summary>
    /// 重複する頂点の法線を平均化（スムース化）し、頂点カラーに埋め込みます
    /// </summary>
    private void BakeSmoothNormalsToVertexColors(Mesh mesh)
    {
        
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Color[] colors = new Color[vertices.Length];

        Dictionary<Vector3, Vector3> positionToNormal = new Dictionary<Vector3, Vector3>();

        // 全頂点を走査して同じ位置の法線を足し合わせる
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = vertices[i];
            Vector3 normal = normals[i];

            if (positionToNormal.TryGetValue(pos, out Vector3 accumulatedNormal))
            {
                positionToNormal[pos] = accumulatedNormal + normal;
            }
            else
            {
                positionToNormal[pos] = normal;
            }
        }

        // 頂点カラーに保存
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = vertices[i];
            Vector3 accumulated = positionToNormal[pos];
            Vector3 smoothedNormal;

            // 【安全対策】法線が正反対で互いに完全に打ち消し合ってしまった場合
            if (accumulated.sqrMagnitude < 0.001f)
            {
                smoothedNormal = normals[i]; // 打ち消し合った場合は、平均化せず元の個別の法線を使用する
            }
            else
            {
                smoothedNormal = accumulated.normalized;
            }

            colors[i] = new Color(
                smoothedNormal.x * 0.5f + 0.5f,
                smoothedNormal.y * 0.5f + 0.5f,
                smoothedNormal.z * 0.5f + 0.5f,
                1f
            );
        }

        mesh.colors = colors;
    }

    
    public void SetOutlineActive(bool active)
    {
        if (isOutlineActive == active) return;
        isOutlineActive = active;

        // パフォーマンスに配慮し、sharedMaterialsの参照を切り替える
        targetRenderer.sharedMaterials = active ? outlineMaterials : normalMaterials;
    }

    void Update()
    {
        if (isOutlineActive != isOutlineON)
        {
            SetOutlineActive(isOutlineON);
        }
        isOutlineON = false;
    }
    //
    void IsLnFrontOfCamera()
    {
        // オブジェクトのワールド座標をビューポート座標（0〜1）に変換
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // カメラの前方にいるか、かつ画面中央（x=0.5, y=0.5）付近にいるか判定
        bool isVisible = viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
        bool isAtCenter = Mathf.Abs(viewportPos.x - 0.5f) < threshold && Mathf.Abs(viewportPos.y - 0.5f) < threshold;

        if (isVisible && isAtCenter)
        {
            isOutlineON = true;
        }
    }

    ////　カメラから外れた
    //private void OnBecameInvisible()
    //{

    //    SetOutlineActive(false);
    //}
    ////　カメラ内に入った
    //private void OnBecameVisible()
    //{

    //    SetOutlineActive(true);
    //}
}