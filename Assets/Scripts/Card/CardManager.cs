using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int matchesScore;
    public int turnsScore;
}

public class CardManager : MonoBehaviour
{
    private Card[,] cards;
    private CardInfo[,] cardInfos;
    private Card firstShownCard = null, secondShowCard = null;

    private bool cardInfosInitialized;

    public bool BlockInput = false;
    public static CardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitCards(int xDim, int yDim)
    {
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
        BlockInput = true;
        yield return new WaitForSeconds(2f);

        foreach (var card in cards)
        {
            if (card == null) continue;

            card.CardRotator.TriggerCardsRotation(showCard: true);
        }

        yield return new WaitForSeconds(1f);

        foreach (var card in cards)
        {
            if (card == null) continue;

            card.CardRotator.TriggerCardsRotation(showCard: false, () =>
            {
                BlockInput = false;
            });
        }
    }

    public void ShowSpecificCard(int x, int y)
    {
        cards[x, y].CardRotator.TriggerCardsRotation(showCard: true);

        if (firstShownCard == null)
            firstShownCard = cards[x, y];
        else if (secondShowCard == null)
        {
            secondShowCard = cards[x, y];
            BlockInput = true;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        UIManager.Instance.UpdateTurnsScore(1);

        yield return new WaitForSeconds(0.5f);
        if (firstShownCard.CardType == secondShowCard.CardType)
        {
            UIManager.Instance.UpdateMatchesScore(1);
            HideMatchedCards();
        }
        else
            FlipUnmatchedCards();
    }

    public void FlipUnmatchedCards()
    {
        firstShownCard.CardRotator.TriggerCardsRotation(showCard: false);
        secondShowCard.CardRotator.TriggerCardsRotation(showCard: false, () =>
        {
        });

        ResetCards();
    }

    private void ResetCards()
    {
        firstShownCard = secondShowCard = null;
        BlockInput = false;
    }

    public void HideMatchedCards()
    {
        firstShownCard.HideCard();
        secondShowCard.HideCard();

        cardInfos[firstShownCard.X, firstShownCard.Y] = firstShownCard.CardInfo;
        cardInfos[secondShowCard.X, secondShowCard.Y] = secondShowCard.CardInfo;

        ResetCards();
    }

    private void OnDestroy()
    {
        UserData.SaveCardInfos(cardInfos, unmatchedCardLeft: FindUnmatchedCards());
    }

    private bool FindUnmatchedCards()
    {
        foreach (var data in cardInfos)
            if (!data.isMatched)
            {
                return true;
            }

        return false;
    }
}
