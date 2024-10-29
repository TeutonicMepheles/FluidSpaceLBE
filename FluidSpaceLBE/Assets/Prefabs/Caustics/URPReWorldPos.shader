Shader "FluidSpaceLBE/URPReWorldPos"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    // 包含 Shader 代码的 SubShader 代码块。
    SubShader
    {
        // SubShader Tags 定义何时以及在何种条件下执行某个 SubShader 代码块
        // 或某个通道。
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 计算屏幕空间的坐标
                float2 positionNDC = IN.positionCS.xy / _ScaledScreenParams.xy;
        
                // 对屏幕空间坐标做深度采样
                #if UNITY_REVERSED_Z
                real depth = SampleSceneDepth(positionNDC);
                #else
                    real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
                #endif

                // calculate position in world-space coordinates
                float3 positionWS = ComputeWorldSpacePosition(positionNDC, depth, UNITY_MATRIX_I_VP);

                // calculate position in object-space coordinates 
                float3 positionOS = TransformWorldToObject(positionWS);

                // create bounding box mask
                float boundingBoxMask = all(step(positionOS, 0.5) * (1 - step(positionOS, -0.5)));
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
