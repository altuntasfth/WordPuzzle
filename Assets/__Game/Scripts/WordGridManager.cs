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
        public GameObject wordGridArea;
        public List<WordGridEntity> wordGrids;
        public List<string> completedWords;

        public int maxColumnLength = 8;

        public void Initialize()
        {
            GenerateWordGridArea();
        }

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

        public void GenerateWordGridArea()
        {
            ClearOldWordGrids();
            
            int column = tileManager.NonBlockedTiles().Count;
            
            for (var i = 0; i < column; i++)
            {
                WordGridEntity wordGridEntity =
                    Instantiate(wordGridPrefab, Vector3.zero, Quaternion.identity, wordGridArea.transform)
                        .GetComponent<WordGridEntity>();
                
                wordGridEntity.ResetGrid();

                wordGrids.Add(wordGridEntity);
            }
        }

        private void ClearOldWordGrids()
        {
            int wordGridsCount = wordGrids.Count();
            for (int i = 0; i < wordGridsCount; i++)
            {
                Destroy(wordGrids[i].gameObject);
            }
            wordGrids.Clear();
        }
    }
}