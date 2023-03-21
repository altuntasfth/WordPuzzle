using System.Collections;
using System.Collections.Generic;
using __Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUIEntity : MonoBehaviour
{
    public LevelData levelData;
    
    [Space(10)] [Header("UI Texts")]
    public TextMeshProUGUI levelInfoTMP;
    public TextMeshProUGUI highScoreTMP;
    
    [Space(10)] [Header("Play Button Info")]
    public Button playButton;
    public TextMeshProUGUI playButtonTMP;
    public Sprite enabledPlayButtonSprite;
    public Sprite disabledPlayButtonSprite;

    public void Initialize()
    {
        levelInfoTMP.text = "Level " + levelData.levelUIData.levelIndex.ToString() + " - " +
                            levelData.levelJsonData.title;
        
        if (levelData.levelUIData.highScore == 0)
        {
            highScoreTMP.text = "No Score";
        }
        else if (levelData.levelUIData.highScore == -1)
        {
            highScoreTMP.text = "Locked Level";
        }
        else
        {
            highScoreTMP.text = "High Score: " + levelData.levelUIData.highScore;
        }
        
        playButton.GetComponent<Image>().sprite =
            levelData.levelUIData.isReadyToPlay == true ? enabledPlayButtonSprite : disabledPlayButtonSprite;
        playButtonTMP.text = levelData.levelUIData.isReadyToPlay == true ? "PLAY" : "LOCKED";
    }
}
