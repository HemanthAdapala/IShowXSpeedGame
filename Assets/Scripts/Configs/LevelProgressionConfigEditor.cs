using UnityEditor;
using UnityEngine;

namespace Configs
{
    [CustomEditor(typeof(LevelProgressionConfig))]
    public class LevelProgressionConfigEditor : Editor
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
