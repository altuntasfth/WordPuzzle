using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace __Game.Scripts
{
    public class TileEntity : MonoBehaviour
    {
        public TileData tileData;
        public List<TileEntity> childrenTiles;
        public List<TileEntity> parentTiles;

        [SerializeField] private TextMeshPro letterTMP;
        public SpriteRenderer background;
        public Color visibleColor;

        public void Initialize()
        {
            this.gameObject.name = "Tile_" + tileData.id + "_" + tileData.character;
            letterTMP.text = tileData.character;

            visibleColor = background.color;
        }
        
        public void SetVisibility()
        {
            background.color = parentTiles.Count == 0 ? visibleColor : Color.gray;
        }

        public void Move()
        {
            
        }
    }
}