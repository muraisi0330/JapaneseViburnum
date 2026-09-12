Shader "Unlit/Dissolve"
{
 Properties{
		_MainColor("Main Color", Color) = (1,1,1,1)	// モデルの色
		_MainTex("Base (RGB)", 2D) = "white" {}		// モデルのテクスチャー
		_Mask("Mask To Dissolve", 2D) = "white" {}	// 分解用のマスク
		_CutOff("CutOff Range", Range(0,1)) = 0		// 分解のしきい値
		_Width("Width", Range(0,1)) = 0.001			// しきい値の幅
		_ColorIntensity("Intensity", Float) = 1		// 燃え尽きる部分の明るさの強度
		_Color("Line Color", Color) = (1,0.3,0,1)	// 燃え尽きる部分の色（デフォルト：オレンジ・赤系）
		_HotColor("Hot Flame Color", Color) = (1,0.9,0.4,1) // 最も熱い部分の色（デフォルト：黄・白系）
		_BumpMap("Normalmap", 2D) = "bump" {}		// モデルのバンプマッピング
	}

	SubShader{

		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 300

		// モデルの描画順がおかしくなる問題の対処
		Pass{
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert alpha 
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Mask;
		fixed4 _Color;
		fixed4 _HotColor;
		fixed4 _MainColor;
		fixed _CutOff;
		fixed _Width;
		fixed _ColorIntensity;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			
			// バンプマッピング
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			// マスク用テクスチャから濃度を取得（0〜1）
			fixed a = tex2D(_Mask, IN.uv_MainTex).r;
			
			// 消失する範囲を求める (_CutOff + _Width * 2.0 >= a) ? 1 : 0
			// 元のC#コントローラーの時間軸と完全に同期します
			fixed b2 = step(a, _CutOff + _Width * 2.0);
			o.Alpha = b2;

			// --- よりリアルに「燃え尽きる」ためのグラデーション計算 ---
			
			// 消滅する最先端の境界（a = _CutOff + _Width * 2.0）からの距離
			float d = (_CutOff + _Width * 2.0) - a;
			
			// 燃焼幅の範囲に正規化 (0.0: 消滅する最先端、1.0: まだ燃えていない通常領域)
			// ゼロ除算を防ぐために max で安全対策
			float t = saturate(d / max(0.0001, _Width * 2.0));

			// 【改良1】炭化（焦げ）表現の追加
			// 燃える手前の部分（tが 0.4 から 0.8 の間）でテクスチャの色を黒く（炭化）していきます
			half charFactor = smoothstep(0.4, 0.8, t);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _MainColor.rgb * charFactor;

			// 【改良2】多色グラデーション発光（Emission）
			half3 flameColor = half3(0, 0, 0);
			if (t < 0.3)
			{
				// 消滅間際の最も熱い部分（黄色や白に白熱化）
				// 元の炎の色(_Color)から、最も高温な炎の色(_HotColor)へ変化させます
				flameColor = lerp(_HotColor.rgb * 1.5, _Color.rgb, t / 0.3);
			}
			else if (t < 0.6)
			{
				// 徐々に温度が下がる部分（赤・オレンジから黒煤へ消灯）
				flameColor = lerp(_Color.rgb, half3(0, 0, 0), (t - 0.3) / 0.3);
			}

			o.Emission = flameColor * _ColorIntensity;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
