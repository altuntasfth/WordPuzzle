using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace __Game.Scripts
{
    public class TileManager : MonoBehaviour
    {
        public GameManager gameManager;
        public WordSearchManager wordSearchManager;
        public LevelJsonData levelJsonData;
        
        [SerializeField] private GameObject tilePrefab;
        public List<TileEntity> tiles;

        private List<string> letters = new List<string>()
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

        private List<string> twoPoints = new List<string>() { "D", "G" };
        private List<string> threePoints = new List<string>() { "B", "C", "M", "P" };
        private List<string> fourPoints = new List<string>() { "F", "H", "V", "W", "Y" };
        private List<string> fivePoints = new List<string>() { "K" };
        private List<string> eightPoints = new List<string>() { "J", "X" };
        private List<string> tenPoints = new List<string>() { "Q", "Z" };
        
        private Dictionary<string, int> lettersWithValues = new Dictionary<string, int>();

        public string[] visibleLetters;

        public void SetupTiles()
        {
            tiles = new List<TileEntity>();
            SetLetterValues();

            GenerateTiles();
            SetChildrenAndParentTiles();
            SetTilesVisibility();

            GetVisibleLetters();
        }

        private void GenerateTiles()
        {
            for (var i = 0; i < levelJsonData.tiles.Length; i++)
            {
                Vector3 tilePosition = levelJsonData.tiles[i].position.x * Vector3.right +
                                       levelJsonData.tiles[i].position.y * Vector3.up +
                                       levelJsonData.tiles[i].position.z * Vector3.forward;
                TileEntity tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform).GetComponent<TileEntity>();
                tile.tileData = levelJsonData.tiles[i];
                
                int value = 0;
                lettersWithValues.TryGetValue(tile.tileData.character, out value);
                tile.value = value;
                
                tile.Initialize();

                tiles.Add(tile);
            }
        }

        private void SetLetterValues()
        {
            for (var i = 0; i < letters.Count(); i++)
            {
                int value = 1;
                string letter = letters[i];

                if (twoPoints.Contains(letter))
                {
                    value = 2;
                }
                else if (threePoints.Contains(letter))
                {
                    value = 3;
                }
                else if (fourPoints.Contains(letter))
                {
                    value = 4;
                }
                else if (fivePoints.Contains(letter))
                {
                    value = 5;
                }
                else if (eightPoints.Contains(letter))
                {
                    value = 8;
                }
                else if (tenPoints.Contains(letter))
                {
                    value = 10;
                }
                
                lettersWithValues.Add(letter, value);
            }
        }

        private List<TileEntity> GetVisibleTiles()
        {
            List<TileEntity> visibleTiles = new List<TileEntity>();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].parentTiles.Count == 0 && tiles[i].gameObject.activeInHierarchy)
                {
                    visibleTiles.Add(tiles[i]);
                }
            }

            return visibleTiles;
        }

        public string[] GetVisibleLetters()
        {
            List<TileEntity> visibleTiles = GetVisibleTiles();
            visibleLetters = new string [visibleTiles.Count];
            
            for (int i = 0; i < visibleTiles.Count; i++)
            {
                visibleLetters[i] = visibleTiles[i].tileData.character;
            }

            return visibleLetters;
        }

        private void SetChildrenAndParentTiles()
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                TileEntity tile = tiles[i];
                
                for (var j = 0; j < tiles[i].tileData.children.Length; j++)
                {
                    TileEntity childrenTile = tiles[tile.tileData.children[j]];
                    
                    tile.childrenTiles.Add(childrenTile);
                    childrenTile.parentTiles.Add(tile);
                }
            }
        }

        public void SetTilesVisibility()
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                tiles[i].SetVisibility();
            }
        }

        public void RemoveCompletedTiles()
        {
            for (var i = 0; i < wordSearchManager.wordGrids.Count; i++)
            {
                TileEntity completedTile = wordSearchManager.wordGrids[i].tile;
                tiles.Remove(completedTile);
            }
        }

        public void OnTileMove(TileEntity tile)
        {
            for (var i = 0; i < wordSearchManager.wordGrids.Count; i++)
            {
                WordGridEntity wordGridEntity = wordSearchManager.wordGrids[i];

                if (wordGridEntity.tile == null)
                {
                    wordGridEntity.tile = tile;
                    wordGridEntity.GetComponent<Image>().color = tile.visibleColor;
                    wordGridEntity.letterTMP.text = tile.tileData.character;
                    
                    tile.Move();
                    
                    gameManager.SetUndoButtonActivate(true);
                    gameManager.SetSubmitButtonActivate();
                    break;
                }
            }
        }
    }
}