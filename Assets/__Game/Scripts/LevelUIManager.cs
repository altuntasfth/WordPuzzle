using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainScreenUI;
    [SerializeField] private GameObject levelSelectionScreenUI;
    [SerializeField] private Transform levelUIContentParent;
    [SerializeField] private GameObject levelUIPrefab;
    [SerializeField] private Button levelsButton;

    private void Awake()
    {
        levelsButton.onClick.AddListener(OnLevelsButton);
        CreateLevelUIs();
    }

    private void OnLevelsButton()
    {
        mainScreenUI.SetActive(false);
        levelSelectionScreenUI.SetActive(true);
    }
    
    private void CreateLevelUIs()
    {
        for (var i = 0; i < 20; i++)
        {
                
            LevelUIEntity levelUIEntity = Instantiate(levelUIPrefab, levelUIContentParent).GetComponent<LevelUIEntity>();
            
        }
    }
}
