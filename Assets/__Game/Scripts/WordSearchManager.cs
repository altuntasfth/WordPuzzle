using System;
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
        public TileManager tileManager;
        public GameManager gameManager;
        public TextMeshProUGUI completedWordsTMP;
        public List<WordGridEntity> wordGrids;
        public List<string> completedWords;
        
        public List<string> possibleWordList;
        
        private const string url = "https://raw.githubusercontent.com/lorenbrichter/Words/master/Words/en.txt";
        private string text;
        private string[] words;
        private Trie trie;
        private HashSet<string> possibleWords;

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
                    
                    DebugPossibleWords();
                    gameManager.possibleWordsTiles = tileManager.GetSortedVisibleTilesToPossibleWord();

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
        
        public List<TileEntity> GetTilesOnWordGrids()
        {
            List<TileEntity> tilesOnWordGrids = new List<TileEntity>();

            for (int i = 0; i < wordGrids.Count; i++)
            {
                if (wordGrids[i].tile != null)
                {
                    tilesOnWordGrids.Add(wordGrids[i].tile);
                }
                else
                {
                    break;
                }
            }

            return tilesOnWordGrids;
        }
        
        private void GeneratePossibleWordsWithVisibleLetters(string currentWord, string[] availableLetters)
        {
            if (currentWord.Length > 1 && !completedWords.Contains(currentWord) && trie.Search(currentWord.ToLower()))
            {
                possibleWords.Add(currentWord);
            }
            for (int i = 0; i < availableLetters.Length; i++)
            {
                string newWord = currentWord + availableLetters[i];
                List<string> newAvailableLetters = new List<string>(availableLetters);
                newAvailableLetters.RemoveAt(i);
                GeneratePossibleWordsWithVisibleLetters(newWord, newAvailableLetters.ToArray());
            }
        }
        
        public HashSet<string> GetPossibleWords()
        {
            possibleWords = new HashSet<string>();
            string[] visibleLetters = tileManager.GetVisibleLetters();
            
            GeneratePossibleWordsWithVisibleLetters("", visibleLetters);

            return possibleWords;
        }

        public void DebugPossibleWords()
        {
            HashSet<string> possibleWordHashSet = GetPossibleWords();
            possibleWordList = new List<string>();

            foreach (string word in possibleWordHashSet)
            {
                possibleWordList.Add(word);
            }

            // MergeSort kullanarak sıralayalım
            string[] sortedWords = MergeSort(possibleWordList.ToArray());
            possibleWordList = sortedWords.ToList();
        }

        public void WriteCompletedWord()
        {
            completedWordsTMP.text += GetCompletedWord() + "\n";
        }

        public void SaveCompletedWord()
        {
            completedWords.Add(GetCompletedWord());
        }

        public int GetWordScore()
        {
            int wordScore = 0;

            for (int i = 0; i < wordGrids.Count; i++)
            {
                if (wordGrids[i].tile != null)
                {
                    wordScore += wordGrids[i].tile.value;
                }
                else
                {
                    break;
                }
            }

            return wordScore * GetCompletedWord().Length * 10;
        }

        public void ResetWordGrids()
        {
            for (int i = 0; i < wordGrids.Count; i++)
            {
                wordGrids[i].ResetGrid();
            }
        }
        
        // MergeSort kullanarak bir string arrayi sıralayan metod
        public string[] MergeSort(string[] arr)
        {
            // temel durum: arrayin tek elemanlı olması
            if (arr.Length <= 1)
            {
                return arr;
            }
        
            // arrayi ikiye bölelim
            int mid = arr.Length / 2;
            string[] left = arr.Take(mid).ToArray();
            string[] right = arr.Skip(mid).ToArray();
        
            // her iki yarısını da tekrar sıralayalım
            left = MergeSort(left);
            right = MergeSort(right);
        
            // sıralanmış yarıları birleştirelim
            return Merge(left, right);
        }
    
        // iki sıralı string arrayi birleştiren metod
        public string[] Merge(string[] left, string[] right)
        {
            string[] result = new string[left.Length + right.Length];
            int leftIndex = 0;
            int rightIndex = 0;
            int resultIndex = 0;
        
            // sol ve sağ arrayleri karşılaştırıp birleştirelim
            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                if (left[leftIndex].Length >= right[rightIndex].Length)
                {
                    result[resultIndex] = left[leftIndex];
                    leftIndex++;
                }
                else
                {
                    result[resultIndex] = right[rightIndex];
                    rightIndex++;
                }
                resultIndex++;
            }
        
            // sol arrayde kalan elemanları result arrayine ekleyelim
            while (leftIndex < left.Length)
            {
                result[resultIndex] = left[leftIndex];
                leftIndex++;
                resultIndex++;
            }
        
            // sağ arrayde kalan elemanları result arrayine ekleyelim
            while (rightIndex < right.Length)
            {
                result[resultIndex] = right[rightIndex];
                rightIndex++;
                resultIndex++;
            }
        
            return result;
        }
    }
}