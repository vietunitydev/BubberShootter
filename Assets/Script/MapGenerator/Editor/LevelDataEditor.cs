using System;
using System.Collections.Generic;
using MapGenerator.Runtime;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGenerator.Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LevelData levelData = (LevelData)target;

            // Game Type == horizontal
            EditorGUILayout.BeginHorizontal();
            levelData.gameType = (GameType)EditorGUILayout.EnumPopup(levelData.gameType, GUILayout.Width(150));
            if (levelData.gameType == GameType.Move)
            {
                levelData.moves = EditorGUILayout.IntField(levelData.moves);
            }
            else if (levelData.gameType == GameType.Time)
            {
                levelData.times = EditorGUILayout.IntField(levelData.times);
            }
            EditorGUILayout.EndHorizontal();
            
            // Level Type == horizontal
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Level Type", GUILayout.Width(150)); 
            levelData.levelType = (LevelType)EditorGUILayout.EnumPopup(levelData.levelType);
            EditorGUILayout.EndHorizontal();

            // Holes
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Holes", GUILayout.Width(150)); 
            levelData.holes = EditorGUILayout.Toggle(levelData.holes);
            EditorGUILayout.EndHorizontal();
            
            // size y 
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size Y", GUILayout.Width(150)); 
            levelData.sizeY = EditorGUILayout.IntField(levelData.sizeY);
            EditorGUILayout.EndHorizontal();

            // Initialize stars array if necessary
            if (levelData.stars == null || levelData.stars.Length != 3)
            {
                levelData.stars = new int[3];
            }
            
            // Star Target
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stars", GUILayout.Width(150));
            for (int i = 0; i < levelData.stars.Length; i++)
            {
                levelData.stars[i] = EditorGUILayout.IntField(levelData.stars[i],GUILayout.Width((Screen.width-150) / 3 - 10));
            }
            EditorGUILayout.EndHorizontal();
            
            // Sprite BackGround
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("BackGround",GUILayout.Width(150));
            levelData.background = (Sprite)EditorGUILayout.ObjectField(levelData.background, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
            
            
            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Grid Editor", EditorStyles.boldLabel);
            
            // Ensure the grid is properly initialized
            if (levelData.Grid == null || levelData.Grid.GetLength(0) != levelData.sizeY || levelData.Grid.GetLength(1) != levelData.sizeY)
            {
                levelData.Grid = new PieceType[levelData.sizeY, levelData.sizeY];
            }
            
            // Clear button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("CLEAR",GUILayout.Width(100), GUILayout.Height(40)))
            {
                levelData.Grid = new PieceType[levelData.sizeY, levelData.sizeY];
            }

            bool random = GUILayout.Button("RANDOM",GUILayout.Width(100), GUILayout.Height(40));
            EditorGUILayout.EndHorizontal();
            
            
            
            for (int y = 0; y < levelData.sizeY; y++)
            {
                EditorGUILayout.BeginHorizontal();
                if (y % 2 == 0)
                {
                    GUILayout.Space(25);
                    for (int x = 0; x < levelData.column - 1; x++)
                    {
                        levelData.Grid[y, x] = (PieceType)EditorGUILayout.EnumPopup(levelData.Grid[y, x],GUILayout.Width(50), GUILayout.Height(50));
                        if (random)
                        {
                            levelData.Grid[y, x] = RandomPiece();
                        }
                    }

                }
                else
                {
                    for (int x = 0; x < levelData.column; x++)
                    {
                        levelData.Grid[y, x] = (PieceType)EditorGUILayout.EnumPopup(levelData.Grid[y, x],GUILayout.Width(50), GUILayout.Height(50));
                        if (random)
                        {
                            levelData.Grid[y, x] = RandomPiece();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(levelData);
            }
        }

        private PieceType RandomPiece()
        {
            var values = Enum.GetValues(typeof(PieceType));
        
            PieceType randomPiece = (PieceType)values.GetValue(Random.Range(0,values.Length-1));
        
            return randomPiece;
        }
    }
}
