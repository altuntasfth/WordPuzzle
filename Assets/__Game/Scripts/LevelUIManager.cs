using System;
using System.Collections;
using System.Collections.Generic;
using __Game.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private BaseLevelDataSO baseLevelData;
    [SerializeField] private GameObject mainScreenUI;
    [SerializeField] private GameObject levelSelectionScreenUI;
    [SerializeField] private Transform levelUIContentParent;
    [SerializeField] private GameObject levelUIPrefab;
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button clearButton;

    private void Awake()
    {
        InitializeLevelsData();
        CreateLevelUIs();
        
        levelsButton.onClick.AddListener(OnLevelsButton);
        clearButton.onClick.AddListener(LevelSaveLoadManager.Instance.ClearData);
    }

    private void OnLevelsButton()
    {
        mainScreenUI.SetActive(false);
        levelSelectionScreenUI.SetActive(true);
    }

    private void CreateLevelUIs()
    {
        AllLevelsData data = LevelSaveLoadManager.Instance.LoadAllLevelsData();
        
        for (var i = 0; i < baseLevelData.levelsDataList.Count; i++)
        {
            LevelData levelData = data.allLevelsData[i];
            
            LevelUIEntity levelUIEntity = Instantiate(levelUIPrefab, levelUIContentParent).GetComponent<LevelUIEntity>();
            levelUIEntity.levelData = levelData;
            levelUIEntity.Initialize();
        }
    }

    private void InitializeLevelsData()
    {
        AllLevelsData data = LevelSaveLoadManager.Instance.LoadAllLevelsData();
        if (data == null)
        {
            Debug.Log("Data == null");
            data = new AllLevelsData();
            data.allLevelsData = new List<LevelData>();
                
            for (int i = 0; i < baseLevelData.levelsDataList.Count; i++)
            {
                TextAsset jsonData = baseLevelData.levelsDataList[i];
                LevelJsonData levelJsonData = JsonUtility.FromJson<LevelJsonData>(jsonData.ToString());
                LevelUIData levelUIData = new LevelUIData
                {
                    levelIndex = i,
                    isReadyToPlay = i == 0 ? true : false,
                    highScore = i == 0 ? 0 : -1,
                };

                LevelData levelData = new LevelData
                {
                    levelUIData = levelUIData,
                    levelJsonData = levelJsonData,
                };

                data.allLevelsData.Add(levelData);
            }
            
            LevelSaveLoadManager.Instance.SaveInitializedData(data);
        }
    }
}
