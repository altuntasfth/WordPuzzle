using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIEntity : MonoBehaviour
{
    public TextMeshProUGUI levelInfoTMP;
    public TextMeshProUGUI highScoreTMP;
    
    [Space(10)] [Header("PlayButton")]
    public Button playButton;
    public TextMeshProUGUI playButtonTMP;
    public Sprite enabledPlayButtonSprite;
    public Sprite disabledPlayButtonSprite;
}
