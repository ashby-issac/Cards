using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CardLocalData
{
    public CardInfo[][] cardInfos;
    public ChancesInfo chancesInfo;
}

[System.Serializable]
public class ChancesInfo
{
    public int matchesCount;
    public int turnsCount;
}

public class CardManager : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private float cardsFlipDelay = 1.0f;
    [SerializeField] private float inputDelay = 1.0f;

    private Card[,] cards;
    private CardInfo[,] cardInfos;
    private Card firstShownCard = null, secondShownCard = null;
    private Queue<Card> queue = new Queue<Card>();

    private bool cardInfosInitialized;
    private float xDim, yDim;

    public bool BlockInput = true;
    public static CardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitCards(int xDim, int yDim)
    {
        this.xDim = xDim;
        this.yDim = yDim;
        cards = new Card[xDim, yDim];
    }

    public void InitCardInfos(int xDim, int yDim, CardInfo[,] cardInfos)
    {
        if (cardInfos != null && cardInfos.Length > 1)
        {
            this.cardInfos = cardInfos;
            cardInfosInitialized = true; // Represents state where saved data is initialized or not
        }
        else
        {
            this.cardInfos = new CardInfo[xDim, yDim];
            Debug.Log($"Init card infos");
        }
    }

    public void AddCard(GameObject cardInstance, int x, int y)
    {
        cards[x, y] = cardInstance.GetComponent<Card>();
        cards[x, y].Init(x, y);

        if (!cardInfosInitialized)
            cardInfos[x, y] = cards[x, y].CardInfo;
    }

    public bool IsCardMatched(int x, int y)
    {
        if (cardInfosInitialized)
        {
            return cardInfos[x, y].isMatched;
        }

        return false;
    }

    public void OnCardsInitialized()
    {
        StartCoroutine(ShowAllCards());
    }

    public IEnumerator ShowAllCards()
    {
        yield return new WaitForSeconds(cardsFlipDelay);
        TriggerCardsRotation(true);

        yield return new WaitForSeconds(cardsFlipDelay);
        TriggerCardsRotation(false);

        Invoke(nameof(EnableInput), inputDelay);
    }

    private void TriggerCardsRotation(bool showCard)
    {
        foreach (var card in cards)
        {
            if (card == null) continue;

            card.CardRotator.TriggerCardsRotation(showCard);
        }
    }

    private void EnableInput()
    {
        BlockInput = false;
    }

    public void ShowSpecificCard(int x, int y)
    {
        if (CheckGameOverCondition())
            return;

        level.SFXManager.PlaySound(AudioType.CardFlip);
        Debug.Log($"ShowSpecificCard: {x}, {y}");
        cards[x, y].CardInput.isClickable = false;
        cards[x, y].CardRotator.TriggerCardsRotation(showCard: true);

        if (!queue.Contains(cards[x, y]))
            queue.Enqueue(cards[x, y]);

        if (firstShownCard == null)
        {
            Debug.Log($"Init first show card: ({x}, {y})");
            firstShownCard = cards[x, y];
        }
        else if (secondShownCard == null)
        {
            secondShownCard = cards[x, y];
            Debug.Log($"Init second show card: ({x}, {y})");
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        UIManager.Instance.UpdateTurnsScore(1);

        bool isMatch = firstShownCard.CardType == secondShownCard.CardType;
        firstShownCard = secondShownCard = null;

        yield return new WaitForSeconds(0.5f);
        if (isMatch)
        {
            UIManager.Instance.UpdateMatchesScore(1);
            HideMatchedCards();
        }
        else
            FlipUnmatchedCards();

        level.SFXManager.PlaySound(isMatch ? AudioType.CardMatch : AudioType.CardMismatch);
    }

    public bool CheckGameOverCondition()
    {
        ChancesInfo chancesInfo = UIManager.Instance.ChancesInfo;
        if (chancesInfo.turnsCount >= (xDim * yDim) * 2)
        {
            level.SFXManager.PlaySound(AudioType.GameOver);
            BlockInput = true;
            UIManager.Instance.SetGameOverText(true);
            TriggerCardsRotation(true);
            return true;
        }

        return false;
    }

    public void FlipUnmatchedCards()
    {
        var card1 = queue.Dequeue();
        var card2 = queue.Dequeue();

        card1.CardRotator.TriggerCardsRotation(showCard: false, () => card1.CardInput.isClickable = true);
        card2.CardRotator.TriggerCardsRotation(showCard: false, () => card2.CardInput.isClickable = true);
    }

    public void HideMatchedCards()
    {
        var card1 = queue.Dequeue();
        var card2 = queue.Dequeue();

        card1.HideCard();
        card2.HideCard();

        cardInfos[card1.X, card1.Y] = card1.CardInfo;
        cardInfos[card2.X, card2.Y] = card2.CardInfo;

        card1.CardInput.isClickable = true;
        card2.CardInput.isClickable = true;
    }

    private void OnDestroy()
    {
        UserData.SaveCardInfos(cardInfos, unmatchedCardLeft: FindUnmatchedCards());
    }

    private bool FindUnmatchedCards()
    {
        foreach (var data in cardInfos)
            if (!data.isMatched)
                return true;

        return false;
    }
}
