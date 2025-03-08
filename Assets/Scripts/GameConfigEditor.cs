using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameConfig))]
public class GameConfigEditor : Editor
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
        // Randomize values for each difficulty level
        foreach (var level in gameConfig.DifficultyLevels)
        {
            level.MinSpawnInterval = Random.Range(0.5f, 3f);
            level.MaxSpawnInterval = Random.Range(level.MinSpawnInterval, 4f);
            level.CubeMoveSpeed = Random.Range(1f, 5f);
        }

        // Mark the ScriptableObject as dirty so changes are saved
        EditorUtility.SetDirty(gameConfig);
    }
}