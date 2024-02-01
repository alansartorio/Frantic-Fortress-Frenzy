Shader "Custom/Shell"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

        Pass
        {
            // Since we can see through the shells technically, we don't want backface culling because then there will be occasional
            // mysterious random holes in the mesh and it'll look really weird
            // also backface culling is when we do not render triangles that are on the backside of a mesh, because that would be a waste
            // of resources since generally you can't see those triangles but in this case we can, so we disable the backface culling
            //            Cull Off
            //            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            // These inform the shader what functions to use for the rendering pipeline, since below my vertex shader is named 'vp' then we tell the shader
            // to use 'vp' for the vertex shader and 'fp' for the fragment shader
            #pragma vertex vp
            #pragma fragment fp

            // #include "UnityCG.cginc"

            // Unity has a lot of built in useful graphics functions, all this stuff is on github which you can look at and read there aren't really any
            // docs on it lmao
            // #include "UnityStandardBRDF.cginc" // for shader lighting info and some utils
            // #include "UnityStandardUtils.cginc" // for energy conservation


            // This is the struct that holds all the data that vertices contain when being passed into the gpu, such as the initial vertex position,
            // the normal, and the uv coordinates
            struct VertexData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            // this is called 'v2f' which I call it that cause it stands for like 'vertex to fragment' idk i think it's a cool simple name you can name it anything!!!
            // This holds all the interpolated information that is passed into the fragment shader such as the screenspace position, the uv coordinates, the interpolated normals,
            // and the world position which even though that was not initially passed in with the vertex data we can still calculate it and pass it over to the fragment shader
            // because we can send over anything to be interpolated, it doesn't have to be only what came in with the vertices
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            uniform int _ShellIndex;
            // This is the current shell layer being operated on, it ranges from 0 -> _ShellCount 
            uniform int _ShellCount; // This is the total number of shells, useful for normalizing the shell index
            uniform float _ShellLength;
            // This is the amount of distance that the shells cover, if this is 1 then the shells will span across 1 world space unit
            uniform float _Density; // This is the density of the strands, used for initializing the noise
            uniform float _NoiseMin, _NoiseMax;
            // This is the range of possible hair lengths, which the hash then interpolates between 
            uniform float _Thickness; // This is the thickness of the hair strand
            uniform float _Attenuation;
            // This is the exponent on the shell height for lighting calculations to fake ambient occlusion (the lack of ambient light)
            uniform float _OcclusionBias;
            // This is an additive constant on the ambient occlusion in order to make the lighting less harsh and maybe kind of fake in-scattering
            uniform float _ShellDistanceAttenuation;
            // This is the exponent on determining how far to push the shell outwards, which biases shells downwards or upwards towards the minimum/maximum distance covered
            uniform float _Curvature;
            // This is the exponent on the physics displacement attenuation, a higher value controls how stiff the hair is
            uniform float _DisplacementStrength; // The strength of the displacement (very complicated)
            uniform float3 _ShellColor; // The color of the shells (very complicated)
            uniform float3 _ShellDirection;
            // The direction the shells are going to point towards, this is updated by the CPU each frame based on user input/movement
            uniform float3 _TilePosition;
            // The direction the shells are going to point towards, this is updated by the CPU each frame based on user input/movement


            //get a scalar random value from a 3d value
            float rand3dTo1d(float3 value, float3 dotDir = float3(12.9898, 78.233, 37.719))
            {
                //make value smaller to avoid artefacts
                float3 smallValue = sin(value);
                //get scalar value from 3d vector
                float random = dot(smallValue, dotDir);
                //make value more random by making it bigger and then taking the factional part
                random = frac(sin(random) * 143758.5453);
                return random;
            }

            v2f vp(VertexData v)
            {
                v2f OUT;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(v.vertex.xyz);
                OUT.pos = positionInputs.positionCS;
                // Or :
                //OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = v.uv;
                OUT.normal = GetVertexNormalInputs(v.normal).normalWS;
                return OUT;
            }

            float4 fp(v2f i) : SV_TARGET
            {
                float2 newUV = i.uv * _Density;

                uint2 tid = newUV;
                float3 seed = _TilePosition + float3(tid * 13, 100);

                float shellIndex = _ShellIndex;
                float shellCount = _ShellCount;

                float rand = lerp(_NoiseMin, _NoiseMax, rand3dTo1d(seed));

                float h = shellIndex / shellCount;
                if (h > rand && _ShellIndex > 0) discard;
                // return float4(1.0, h, 0, 1.0);
                // float ndotl = DotClamped(i.normal, _WorldSpaceLightPos0) * 0.5f + 0.5f;
                // float ndotl = saturate(dot(i.normal, _WorldSpaceCameraPos)) * 0.5f + 0.5f;
                // ndotl = ndotl * ndotl;
                float ambientOcclusion = pow(h, _Attenuation);

                ambientOcclusion += _OcclusionBias;

                ambientOcclusion = saturate(ambientOcclusion);

                // return float4(_ShellColor * ndotl * ambientOcclusion, 1.0);
                return float4(_ShellColor * ambientOcclusion, 1.0);
                // return float4(_ShellColor * shellHeight * ambientOcclusion, 1.0);
                // return float4(i.normal.xyz, 1.0);
            }

            // This indicates the end of the CG code block
            ENDHLSL
        }
    }
}