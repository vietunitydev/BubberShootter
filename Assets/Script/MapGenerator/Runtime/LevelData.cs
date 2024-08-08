using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapGenerator.Runtime
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level Design/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("General")] 
        public GameType gameType;
        public LevelType levelType;
        public int moves;
        public int times;
        public bool holes;
        public int sizeY;
        public int column = 10;
        public int[] stars;
        // Tutorial
        public Sprite background;
        public List<Sprite> template;

        public Sprite[] templateSprites;
        
        public bool randomColorBalls;
        // public GameObject[]
        // public List<LevelTarget> targets;
        [Header("Editor")]
        public PieceType[,] Grid;
    }
    public enum GameType
    {
        Move,
        Time
    }

    public enum LevelType
    {
        Vertical,
        Rotating
    }
    
}