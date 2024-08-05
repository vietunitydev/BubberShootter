using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.Runtime
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level Design/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int moves;
        public string levelType;
        public bool holes;
        public int sizeY;
        public int[] stars;
        public string background;
        public bool randomizeColors;
        public List<LevelTarget> targets;
        public GameObject[,] grid;
    }

    [System.Serializable]
    public class LevelTarget
    {
        public string targetName;
    }
}