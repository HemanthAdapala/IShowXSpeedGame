using Configs;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Add a button to randomize values
            GameConfig gameConfig = (GameConfig)target;
            if (GUILayout.Button("Randomize Values"))
            {
                RandomizeValues(gameConfig);
            }
        }

        void RandomizeValues(GameConfig gameConfig)
        {
            // Mark the ScriptableObject as dirty so changes are saved
            EditorUtility.SetDirty(gameConfig);
        }
    }
}