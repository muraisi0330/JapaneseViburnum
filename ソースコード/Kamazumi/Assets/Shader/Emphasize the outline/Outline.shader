Shader "Unlit/Outline"
{
     Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.01
    }
     SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }

        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZTest LEqual
            ZWrite Off
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                
                // 1. スムース法線の取得と安全な正規化
                float3 smoothNormal = v.color.rgb * 2.0 - 1.0;
                float3 norm;

                float len = length(smoothNormal);
                if (len < 0.01 || 
                    dot(abs(v.color.rgb - 1.0), 1.0) < 0.01 || 
                    dot(abs(v.color.rgb), 1.0) < 0.01)
                {
                    norm = normalize(v.normal);
                }
                else
                {
                    norm = smoothNormal / len;
                }

                // 2. 【改善点】ワールド空間（世界基準）での押し出し処理
                // これにより、モデルのサイズやTransformのScale設定に影響されず、
                // 常に世界基準の物理単位（メートル）で均一な太さの輪郭線を描画します。
                
                // 頂点をワールド座標へ変換
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                // 法線ベクトルをワールド座標の向きへ変換
                float3 worldNormal = UnityObjectToWorldNormal(norm);
                
                // ワールド空間上で法線方向に押し出し（_OutlineWidth の単位は「メートル」になります）
                worldPos += normalize(worldNormal) * _OutlineWidth;
                
                // ワールド座標からカメラのスクリーン座標（投影クリップ空間）へ変換
                o.pos = UnityWorldToClipPos(worldPos);
                
                o.color = _OutlineColor;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
               return _OutlineColor;
            }
            ENDCG
        }
    }
}