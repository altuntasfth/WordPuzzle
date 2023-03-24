using System;
using DG.Tweening;
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
        public int totalScore;
        [SerializeField] private LevelData levelData;
        
        [Space(10)] [Header("Managers")]
        [SerializeField] private TileManager tileManager;
        [SerializeField] private WordSearchManager wordSearchManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private UndoButtonLongPressListener undoButtonLongPressListener;

        [Space(10)] [Header("UI")]
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private TextMeshProUGUI titleTMP;
        [SerializeField] private TextMeshProUGUI wordScoreTMP;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button undoButton;
        
        private AllLevelsData data;
        private int levelIndex;
        public bool isUndoActive;

        private void OnEnable()
        {
            playerManager.MoveTile += tileManager.OnTileMove;
            undoButtonLongPressListener.onLongPress += UndoMove;
        }

        private void OnDestroy()
        {
            playerManager.MoveTile -= tileManager.OnTileMove;
            undoButtonLongPressListener.onLongPress -= UndoMove;
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
                totalScore += wordSearchManager.GetWordScore();
                scoreTMP.text = "Score: " + totalScore.ToString();
                
                tileManager.RemoveCompletedTiles();
                tileManager.SetTilesVisibility();
                wordSearchManager.SaveCompletedWord();
                wordSearchManager.WriteCompletedWord();
                wordSearchManager.ResetWordGrids();
                
                SetSubmitButtonActivate();
            });
            
            undoButton.onClick.AddListener(() =>
            {
                UndoMove(false);
                SetSubmitButtonActivate();
            });

            isGameStarted = true;
        }

        private void Start()
        {
            tileManager.SetupTiles();
        }
        
        private void Update()
        {
            DebugControl();
        }

        public void SetSubmitButtonActivate()
        {
            if (wordSearchManager.IsWordComplete())
            {
                submitButton.GetComponent<Image>().color = Color.green;
                submitButton.interactable = true;

                wordScoreTMP.text = "Word Score: " + wordSearchManager.GetWordScore().ToString();
            }
            else
            {
                submitButton.GetComponent<Image>().color = Color.white;
                submitButton.interactable = false;

                wordScoreTMP.text = "";
            }
        }

        public void SetUndoButtonActivate(bool isActive)
        {
            undoButton.interactable = isActive;
            isUndoActive = isActive;
            undoButton.GetComponent<Image>().color = isActive ? Color.blue : Color.gray;
        }
        
        private void UndoMove(bool isLongPress)
        {
            if (isUndoActive)
            {
                for (var i = wordSearchManager.wordGrids.Count - 1; i > -1; i--)
                {
                    TileEntity lastTile = wordSearchManager.wordGrids[i].tile;
                    if (lastTile != null)
                    {
                        wordSearchManager.wordGrids[i].ResetGrid();
                        lastTile.UndoMove();

                        if (!isLongPress)
                        {
                            break;
                        }
                    }
                }
                
                SetUndoButtonActivate(false);
            }
        }
        
        private void SaveCompletedLevelData()
        {
            if (levelData.levelUIData.highScore > totalScore)
            {
                return;
            }
            
            levelData.levelUIData.highScore = totalScore;
            LevelSaveLoadManager.Instance.Save(data, levelData, levelIndex);
        }
        
        private void SetReadyToPlayNextLevel()
        {
            int nextLevelIndex = levelIndex + 1;
            LevelData nextLevelData = data.allLevelsData[nextLevelIndex];
            
            if (nextLevelIndex < data.allLevelsData.Count && nextLevelData.levelUIData.highScore == -1)
            {
                nextLevelData.levelUIData.highScore = 0;
                nextLevelData.levelUIData.isReadyToPlay = true;
                LevelSaveLoadManager.Instance.Save(data, nextLevelData, nextLevelIndex);
            }
        }

        private void DebugControl()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                levelIndex = PlayerPrefs.GetInt("ActiveLevelIndex");
                PlayerPrefs.SetInt("ActiveLevelIndex", Mathf.Clamp(levelIndex - 1, 0, data.allLevelsData.Count - 1));
                PlayerPrefs.Save();
                DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                levelIndex = PlayerPrefs.GetInt("ActiveLevelIndex");
                PlayerPrefs.SetInt("ActiveLevelIndex", Mathf.Clamp(levelIndex + 1, 0, data.allLevelsData.Count - 1));
                PlayerPrefs.Save();
                DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveCompletedLevelData();
                SetReadyToPlayNextLevel();
                
                DOTween.KillAll();
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}