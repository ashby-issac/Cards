using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matchesScoreText;
    [SerializeField] private TextMeshProUGUI turnsScoreText;

    public static UIManager Instance;

    private int matchesScore, turnsScore;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        matchesScore = 0;
        turnsScore = 0;
    }

    public void UpdateMatchesScore(int scoreToAdd)
    {
        matchesScore += scoreToAdd;
        matchesScoreText.text = $"{matchesScore}";
    }

    public void UpdateTurnsScore(int scoreToAdd)
    {
        turnsScore += scoreToAdd;
        turnsScoreText.text = $"{turnsScore}";
    }
}
