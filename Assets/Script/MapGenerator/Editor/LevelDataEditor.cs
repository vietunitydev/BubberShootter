using System.Collections.Generic;
using MapGenerator.Runtime;
using UnityEditor;
using UnityEngine;

namespace MapGenerator.Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LevelData levelData = (LevelData)target;

            levelData.moves = EditorGUILayout.IntField("Moves", levelData.moves);
            levelData.levelType = EditorGUILayout.TextField("Level Type", levelData.levelType);
            levelData.holes = EditorGUILayout.Toggle("Holes", levelData.holes);
            levelData.sizeY = EditorGUILayout.IntField("Size Y", levelData.sizeY);

            // Initialize stars array if necessary
            if (levelData.stars == null || levelData.stars.Length != 3)
            {
                levelData.stars = new int[3];
            }
            for (int i = 0; i < levelData.stars.Length; i++)
            {
                levelData.stars[i] = EditorGUILayout.IntField($"Star {i + 1}", levelData.stars[i]);
            }

            levelData.background = EditorGUILayout.TextField("Background", levelData.background);
            levelData.randomizeColors = EditorGUILayout.Toggle("Randomize Colors", levelData.randomizeColors);

            EditorGUILayout.LabelField("Level targets to complete the level");
            if (levelData.targets == null)
                levelData.targets = new List<LevelTarget>();

            for (int i = 0; i < levelData.targets.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                levelData.targets[i].targetName = EditorGUILayout.TextField($"Target {i + 1}", levelData.targets[i].targetName);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    levelData.targets.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Target"))
            {
                levelData.targets.Add(new LevelTarget());
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grid Editor", EditorStyles.boldLabel);

            // Ensure the grid is properly initialized
            if (levelData.grid == null || levelData.grid.GetLength(0) != levelData.sizeY || levelData.grid.GetLength(1) != levelData.sizeY)
            {
                levelData.grid = new GameObject[levelData.sizeY, levelData.sizeY];
            }

            for (int y = 0; y < levelData.sizeY; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < levelData.sizeY; x++)
                {
                    levelData.grid[y, x] = (GameObject)EditorGUILayout.ObjectField(levelData.grid[y, x], typeof(GameObject), false, GUILayout.Width(50));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(levelData);
            }
        }
    }
}
