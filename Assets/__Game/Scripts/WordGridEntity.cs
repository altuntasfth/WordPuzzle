using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class WordGridEntity : MonoBehaviour
    {
        public TextMeshProUGUI letterTMP;
        public TileEntity tile;
        public Color initialColor;

        public void ResetGrid()
        {
            letterTMP.text = "";
            GetComponent<Image>().color = initialColor;
            if (tile != null)
            {
                tile.isOnWordGrid = false;
            }
            tile = null;
        }
    }
}