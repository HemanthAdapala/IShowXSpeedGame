using Configs;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelProgressionConfig))]
    public class LevelProgressionConfigEditor : UnityEditor.Editor
    {
        //Draw the default inspector
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelProgressionConfig levelProgressionConfig = (LevelProgressionConfig)target;

            if (GUILayout.Button("Structured Values Initilizer"))
            {
                levelProgressionConfig.RandomizeValues();
            }
        }
        
        
    }
}
