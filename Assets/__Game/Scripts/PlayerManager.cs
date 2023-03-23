using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace __Game.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        public event Action<TileEntity> MoveTile;

        private void OnEnable()
        {
            InputManager.Instance.PointerDown += HandleOnPointerDown;
            InputManager.Instance.PointerDrag += HandleOnPointerDrag;
            InputManager.Instance.PointerEnd += HandleOnPointerEnd;
        }

        private void OnDisable()
        {
            InputManager.Instance.PointerDown -= HandleOnPointerDown;
            InputManager.Instance.PointerDrag += HandleOnPointerDrag;
            InputManager.Instance.PointerEnd -= HandleOnPointerEnd;
        }
        
        private void HandleOnPointerDown(PointerEventData eventData)
        {
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }

            RaycastHit hit;
            Vector3 screenToWorldPoint = gameManager.mainCamera.
                ScreenToWorldPoint(eventData.position.x * Vector3.right + eventData.position.y * Vector3.up - 10 *Vector3.forward);
            if (Physics.Raycast(screenToWorldPoint, Vector3.forward, out hit, 1000f))
            {
                if (hit.collider != null)
                {
                    TileEntity tile = hit.collider.GetComponent<TileEntity>();
                    if (tile != null && tile.parentTiles.Count == 0)
                    {
                        MoveTile?.Invoke(tile);
                    }
                }
            }
        }

        private void HandleOnPointerDrag(PointerEventData eventData)
        {
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }
        }

        private void HandleOnPointerEnd(PointerEventData eventData)
        {
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }
        }
    }
}