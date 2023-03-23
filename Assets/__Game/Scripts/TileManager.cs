using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace __Game.Scripts
{
    public class TileManager : MonoBehaviour
    {
        public GameManager gameManager;
        public WordGridManager wordGridManager;
        public LevelJsonData levelJsonData;
        
        [SerializeField] private GameObject tilePrefab;
        public List<TileEntity> tiles;

        public void SetupTiles()
        {
            tiles = new List<TileEntity>();
            
            GenerateTiles();
            SetChildrenAndParentTiles();
            SetTilesVisibility();
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
                tile.Initialize();
                
                tiles.Add(tile);
            }
        }

        public List<TileEntity> NonBlockedTiles()
        {
            List<TileEntity> nonBlockedTiles = new List<TileEntity>();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].parentTiles.Count == 0)
                {
                    nonBlockedTiles.Add(tiles[i]);
                }
            }
            
            return nonBlockedTiles;
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
            for (var i = 0; i < wordGridManager.wordGrids.Count; i++)
            {
                TileEntity completedTile = wordGridManager.wordGrids[i].tile;
                tiles.Remove(completedTile);
                
                for (var j = 0; j < completedTile.childrenTiles.Count; j++)
                {
                    completedTile.childrenTiles[j].parentTiles.Remove(completedTile);
                }
            }
        }

        public void OnTileMove(TileEntity tile)
        {
            for (var i = 0; i < wordGridManager.wordGrids.Count; i++)
            {
                WordGridEntity wordGridEntity = wordGridManager.wordGrids[i];

                if (wordGridEntity.tile == null)
                {
                    //tile.transform.DOMove(wordGridEntity.GetComponent<RectTransform>().anchoredPosition, 1f);
                    
                    wordGridEntity.tile = tile;
                    wordGridEntity.GetComponent<Image>().color = tile.visibleColor;
                    wordGridEntity.letterTMP.text = tile.tileData.character;
                    tile.gameObject.SetActive(false);
                    
                    gameManager.SetUndoButtonActivate(true);
                    gameManager.SetSubmitButtonActivate();
                    break;
                }
            }
        }
    }
}