using UnityEngine;

namespace WaterCausticsForURP
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    [AddComponentMenu("Effects/Caustics Volume")]
    [HelpURL("https://alexander-ameye.gitbook.io/water-caustics-for-urp/")]
    public class CausticsVolume : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        public const string CausticsShaderName = "Ameye/Water Caustics for URP";
        
        [SerializeField] private Material material;
        public Material Material
        {
            get => material;
            set
            {
                material = value;
            }
        }
        
        // shader properties
        [SerializeField] private Texture texture;
        [SerializeField] [Range(0f, 1.0f)] private float intensity;
        [SerializeField] [Range(0f, 0.5f)] private float rgbSplit;
        [SerializeField] [Range(0.01f, 4.0f)] private float scale;
        [SerializeField] [Range(0.0f, 0.3f)] private float speed;
        [SerializeField] [Range(0.0f, 1.0f)] private float sceneLuminanceMaskStrength;
        [SerializeField] [Range(0.0f, 1.0f)] private float shadowMaskStrength;
        [SerializeField]  [Range(0.0f, 1.0f)] private float fadeAmount;
        [SerializeField]  [Range(0.5f, 1.0f)] private float fadeHardness;
        
        public Vector3 fixedLightDirection;
        
        public enum LightDirectionSource
        {
            Fixed,
            MainLight
        }
        public LightDirectionSource lightDirectionSource;

        [SerializeField] private bool displayDebugOverlay;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Effects/Caustics Volume", priority = 7)]
        static void CreateCausticsVolume()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:Prefab " + "Caustics Volume");
            if (guids.Length == 0) Debug.Log("Error: caustics volume not found");
            else
            {
                GameObject prefab =
                    UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                        UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]));
                GameObject instance = (GameObject) UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
                UnityEditor.PrefabUtility.UnpackPrefabInstance(instance, UnityEditor.PrefabUnpackMode.Completely,
                    UnityEditor.InteractionMode.AutomatedAction);
                UnityEditor.Undo.RegisterCreatedObjectUndo(instance, "Create Caustics Volume");
                UnityEditor.Selection.activeObject = instance;
                UnityEditor.SceneView.FrameLastActiveSceneView();
            }
        }
#endif

        private void OnEnable()
        {
            if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
            if (!meshFilter) meshFilter = GetComponent<MeshFilter>();

            material = meshRenderer.sharedMaterial;

            ReadMaterialProperties();
            WriteMaterialProperties();
        }

        public void ReadMaterialProperties()
        {
            if (meshRenderer) material = meshRenderer.sharedMaterial;
            if (!material) return;
            if (material.shader.name != CausticsShaderName) return;

            texture = material.GetTexture(CausticsShaderUtils.TextureProperty);
            intensity = material.GetFloat(CausticsShaderUtils.IntensityProperty);
            rgbSplit = material.GetFloat(CausticsShaderUtils.RgbSplitProperty);
            scale = material.GetFloat(CausticsShaderUtils.ScaleProperty);
            speed = material.GetFloat(CausticsShaderUtils.SpeedProperty);
            sceneLuminanceMaskStrength = material.GetFloat(CausticsShaderUtils.SceneLuminanceMaskProperty);
            shadowMaskStrength = material.GetFloat(CausticsShaderUtils.ShadowMaskProperty);
            fadeAmount = material.GetFloat(CausticsShaderUtils.FadeAmountProperty);
            fadeHardness = material.GetFloat(CausticsShaderUtils.FadeHardnessProperty);

            if (material.IsKeywordEnabled("FIXED_LIGHT_DIRECTION")) lightDirectionSource = LightDirectionSource.Fixed;
            else lightDirectionSource = LightDirectionSource.MainLight;
        }

        public void WriteMaterialProperties()
        {
            if (!material) return;
            if (material.shader.name != CausticsShaderName) return;

            material.SetTexture(CausticsShaderUtils.TextureProperty, texture);
            material.SetFloat(CausticsShaderUtils.IntensityProperty, intensity);
            material.SetFloat(CausticsShaderUtils.RgbSplitProperty, rgbSplit);
            material.SetFloat(CausticsShaderUtils.ScaleProperty, scale);
            material.SetFloat(CausticsShaderUtils.SpeedProperty, speed);
            material.SetFloat(CausticsShaderUtils.SceneLuminanceMaskProperty, sceneLuminanceMaskStrength);
            material.SetFloat(CausticsShaderUtils.ShadowMaskProperty, shadowMaskStrength);
            material.SetFloat(CausticsShaderUtils.FadeAmountProperty, fadeAmount);
            material.SetFloat(CausticsShaderUtils.FadeHardnessProperty, fadeHardness);

            if (lightDirectionSource == LightDirectionSource.Fixed) material.EnableKeyword("FIXED_LIGHT_DIRECTION");
            else material.DisableKeyword("FIXED_LIGHT_DIRECTION");
            
            Matrix4x4 fixedDirectionMatrix = Matrix4x4.TRS(Vector3.zero,
                Quaternion.Euler(fixedLightDirection.x, fixedLightDirection.y, fixedLightDirection.z),
                Vector3.one);
            material.SetMatrix(CausticsShaderUtils.FixedLightDirectionProperty, fixedDirectionMatrix);
        }
        
         public void HideMeshFilterRenderer() {
             const HideFlags flags = HideFlags.HideInInspector; 
            meshRenderer.hideFlags = flags;
            meshFilter.hideFlags = flags;
        }
    }
}