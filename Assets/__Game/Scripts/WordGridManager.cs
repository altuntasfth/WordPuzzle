using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class WordGridManager : MonoBehaviour
    {
        public GameObject wordGridPrefab;
        public TileManager tileManager;
        public List<WordGridEntity> wordGrids;
        public List<string> completedWords;

        public bool IsWordComplete()
        {
            bool isComplete = false;
            string completedWord = "";

            for (int i = 0; i < wordGrids.Count; i++)
            {
                if (wordGrids[i].tile == null)
                {
                    return isComplete;;
                }

                completedWord += wordGrids[i].tile.tileData.character;
            }

            isComplete = true;
            completedWords.Add(completedWord);
            
            return isComplete;
        }
    }
}