using UnityEngine;

namespace WaterCausticsForURP
{
    public static class CausticsShaderUtils
    {
        public static readonly int TextureProperty = Shader.PropertyToID("_CausticsTexture");
        public static readonly int IntensityProperty = Shader.PropertyToID("_CausticsStrength");
        public static readonly int RgbSplitProperty = Shader.PropertyToID("_CausticsSplit");
        public static readonly int ScaleProperty = Shader.PropertyToID("_CausticsScale");
        public static readonly int SpeedProperty = Shader.PropertyToID("_CausticsSpeed");
        public static readonly int FadeAmountProperty = Shader.PropertyToID("_CausticsFadeAmount");
        public static readonly int FadeHardnessProperty = Shader.PropertyToID("_CausticsFadeHardness");
        public static readonly int SceneLuminanceMaskProperty = Shader.PropertyToID("_CausticsSceneLuminanceMaskStrength");
        public static readonly int ShadowMaskProperty = Shader.PropertyToID("_CausticsShadowMaskStrength");
        public static readonly int FixedLightDirectionProperty = Shader.PropertyToID("_FixedLightDirection");
    }
}
