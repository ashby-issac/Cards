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
    [SerializeField] private TextMeshProUGUI gameOverText;

    public static UIManager Instance;

    private ChancesInfo chancesInfo;

    public ChancesInfo ChancesInfo => chancesInfo;

    private void Awake()
    {
        Instance = this;

        if (SaveSystem.HasKey(UserData.ChancesData_Constant))
            chancesInfo = SaveSystem.LoadJson<ChancesInfo>(UserData.ChancesData_Constant);
        else
            chancesInfo = new ChancesInfo();

        UpdateTexts();
    }

    private void OnEnable()
    {
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
        chancesInfo.matchesCount += scoreToAdd;
        matchesScoreText.text = $"{chancesInfo.matchesCount}";
    }

    public void UpdateTurnsScore(int scoreToAdd)
    {
        chancesInfo.turnsCount += scoreToAdd;
        turnsScoreText.text = $"{chancesInfo.turnsCount}";
    }

    public void UpdateTexts()
    {
        if (chancesInfo == null) chancesInfo = new ChancesInfo();

        SetGameOverText(false);
        matchesScoreText.text = $"{chancesInfo.matchesCount}";
        turnsScoreText.text = $"{chancesInfo.turnsCount}";
    }

    public void SetGameOverText(bool state)
    {
        gameOverText.gameObject.SetActive(state);
    }
}
