using UnityEditor;

namespace WaterCausticsForURP
{
    [CustomEditor(typeof(CausticsLightDirectionFeature))]
    public class CausticsLightDirectionFeatureEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("This renderer feature is used to pass the Main Light direction to the caustics shader.", MessageType.Info, true);
        }
    }
}