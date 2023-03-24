using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class WordSearchManager : MonoBehaviour
    {
        public TextMeshProUGUI completedWordsTMP;
        public List<WordGridEntity> wordGrids;
        public List<string> completedWords;
        
        private const string url = "https://raw.githubusercontent.com/lorenbrichter/Words/master/Words/en.txt";
        private string text;
        private string[] words;
        private Trie trie;

        private IEnumerator Start()
        {
            using (WWW www = new WWW(url))
            {
                yield return www;

                if (www.error == null)
                {
                    text = www.text;
                    words = text.Split('\n');
                    
                    trie = new Trie();
                    foreach (var word in words)
                    {
                        trie.Insert(word);
                    }
                }
                else
                {
                    Debug.Log("Error while fetching text file: " + www.error);
                }
            }
        }

        public bool IsWordComplete()
        {
            string completedWord = GetCompletedWord();
            
            if (completedWord == "" || completedWord == " " || completedWords.Contains(completedWord))
            {
                return false;
            }

            bool  isWordInDictionary = trie.Search(completedWord.ToLower());
            if (!isWordInDictionary)
            {
                return false;
            }

            return true;
        }

        private string GetCompletedWord()
        {
            string completedWord = "";

            for (int i = 0; i < wordGrids.Count; i++)
            {
                if (wordGrids[i].tile != null)
                {
                    completedWord += wordGrids[i].tile.tileData.character;
                }
                else
                {
                    break;
                }
            }

            return completedWord;
        }

        public void WriteCompletedWord()
        {
            completedWordsTMP.text += GetCompletedWord() + "\n";
        }

        public void SaveCompletedWord()
        {
            completedWords.Add(GetCompletedWord());
        }

        public void ResetWordGrids()
        {
            for (int i = 0; i < wordGrids.Count; i++)
            {
                wordGrids[i].ResetGrid();
            }
        }
    }
}