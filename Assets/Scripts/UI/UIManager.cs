using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Level level;

    [SerializeField] private TextMeshProUGUI matchesScoreText;
    [SerializeField] private TextMeshProUGUI turnsScoreText;
    [SerializeField] private Button restartBtn;

    public static UIManager Instance;

    private ChancesInfo chancesInfo;
    private int matchesScore, turnsScore;

    private void Awake()
    {
        Instance = this;

        if (SaveSystem.HasKey(UserData.ChancesData_Constant))
        {
            chancesInfo = SaveSystem.LoadJson<ChancesInfo>(UserData.ChancesData_Constant);
            UpdateTexts();
        }
        else
        {
            chancesInfo = new ChancesInfo();
        }
    }

    private void OnEnable()
    {
        matchesScore = 0;
        turnsScore = 0;
        restartBtn.onClick.AddListener(() => level.Restart());
    }

    private void OnDisable()
    {
        restartBtn.onClick.RemoveListener(() => level.Restart());
    }

    private void OnDestroy()
    {
        UserData.SaveChancesInfo(chancesInfo);
    }

    public void UpdateMatchesScore(int scoreToAdd)
    {
        chancesInfo.matchesScore += scoreToAdd;
        matchesScoreText.text = $"{chancesInfo.matchesScore}";
    }

    public void UpdateTurnsScore(int scoreToAdd)
    {
        chancesInfo.turnsScore += scoreToAdd;
        turnsScoreText.text = $"{chancesInfo.turnsScore}";
    }

    public void UpdateTexts()
    {
        matchesScoreText.text = $"{chancesInfo.matchesScore}";
        turnsScoreText.text = $"{chancesInfo.turnsScore}";
    }
}
