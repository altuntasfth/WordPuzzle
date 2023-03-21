using System;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{
    [CreateAssetMenu(fileName = "BaseLevel", menuName = "Level/Create Levels Data")]
    public class BaseLevelDataSO : ScriptableObject
    {
        public List<TextAsset> levelsDataList;
    }
    
    [Serializable]
    public class AllLevelsData
    {
        public List<LevelData> allLevelsData;
    }

    [Serializable]
    public class LevelData
    {
        public LevelUIData levelUIData;
        public LevelJsonData levelJsonData;
    }

    [Serializable]
    public class LevelUIData
    {
        public int levelIndex;
        public bool isReadyToPlay;
        public int highScore;
    }
    
    [Serializable]
    public class LevelJsonData
    {
        public string title;
        public Tile[] tiles;
    }

    [Serializable]
    public class Tile
    {
        public int id;
        public Position position;
        public string character;
        public int[] children;
    }

    [Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }
}