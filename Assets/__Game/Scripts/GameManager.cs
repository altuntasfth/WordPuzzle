using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Camera mainCamera;
        public bool isGameStarted;
        public bool isGameOver;
        
        [Space(10)] [Header("Managers")]
        [SerializeField] private TileManager tileManager;
        [SerializeField] private WordGridManager wordGridManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private LevelData levelData;
        [SerializeField] private UndoButtonLongPressListener undoButtonLongPressListener;

        [Space(10)] [Header("UI")]
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private TextMeshProUGUI titleTMP;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button undoButton;
        
        private AllLevelsData data;
        private int levelIndex;
        public bool isUndoActive;

        private void OnEnable()
        {
            playerManager.MoveTile += tileManager.OnTileMove;
            undoButtonLongPressListener.onLongPress.AddListener(UndoAllWord);
        }

        private void OnDestroy()
        {
            playerManager.MoveTile -= tileManager.OnTileMove;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            
            levelIndex = PlayerPrefs.GetInt("ActiveLevelIndex", 0);
            
            data = LevelSaveLoadManager.Instance.LoadAllLevelsData();
            levelData = data.allLevelsData[levelIndex];

            tileManager.levelJsonData = levelData.levelJsonData;

            scoreTMP.text = "Score: " + levelData.levelUIData.highScore.ToString();
            titleTMP.text = levelData.levelJsonData.title.ToString();

            submitButton.onClick.AddListener(() =>
            {
                tileManager.RemoveCompletedTiles();
                tileManager.SetTilesVisibility();
                wordGridManager.GenerateWordGridArea();
                
                SetSubmitButtonActivate();
            });
            
            undoButton.onClick.AddListener(() =>
            {
                UndoLastMove();
                SetSubmitButtonActivate();
            });

            isGameStarted = true;
        }

        private void Start()
        {
            tileManager.SetupTiles();
            wordGridManager.Initialize();
        }
        
        private void Update()
        {
            DebugControl();
        }

        public void SetSubmitButtonActivate()
        {
            if (wordGridManager.IsWordComplete())
            {
                submitButton.GetComponent<Image>().color = Color.green;
                submitButton.interactable = true;
            }
            else
            {
                submitButton.GetComponent<Image>().color = Color.white;
                submitButton.interactable = false;
            }
        }

        public void SetUndoButtonActivate(bool isActive)
        {
            undoButton.interactable = isActive;
            isUndoActive = isActive;
            undoButton.GetComponent<Image>().color = isActive ? Color.blue : Color.gray;
        }
        
        private void UndoLastMove()
        {
            if (isUndoActive)
            {
                for (var i = wordGridManager.wordGrids.Count - 1; i > -1; i--)
                {
                    TileEntity lastTile = wordGridManager.wordGrids[i].tile;
                    if (lastTile != null)
                    {
                        wordGridManager.wordGrids[i].ResetGrid();
                        lastTile.gameObject.SetActive(true);
                        
                        break;
                    }
                }
                
                SetUndoButtonActivate(false);
            }
        }
        
        public void UndoAllWord()
        {
            if (isUndoActive)
            {
                for (var i = wordGridManager.wordGrids.Count - 1; i > -1; i--)
                {
                    TileEntity lastTile = wordGridManager.wordGrids[i].tile;
                    if (lastTile != null)
                    {
                        wordGridManager.wordGrids[i].ResetGrid();
                        lastTile.gameObject.SetActive(true);
                    }
                }
                
                SetUndoButtonActivate(false);
            }
        }

        private void DebugControl()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                levelIndex = PlayerPrefs.GetInt("ActiveLevelIndex");
                PlayerPrefs.SetInt("ActiveLevelIndex", Mathf.Clamp(levelIndex - 1, 0, data.allLevelsData.Count - 1));
                PlayerPrefs.Save();
                //DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                levelIndex = PlayerPrefs.GetInt("ActiveLevelIndex");
                PlayerPrefs.SetInt("ActiveLevelIndex", Mathf.Clamp(levelIndex + 1, 0, data.allLevelsData.Count - 1));
                PlayerPrefs.Save();
                //DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}