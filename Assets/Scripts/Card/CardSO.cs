using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CardView
{
    public CardType cardType;
    [JsonIgnore]
    public Color color; // can use sprite instead of color
}

[CreateAssetMenu(fileName = "CardSO", menuName = "CardSO")]
public class CardSO : ScriptableObject
{
    public CardView[] cardViewDatas;

    private int maxPairCount = 2;
    private int totalCards = 0, pairsCounter;

    private List<(CardType key, int value)> cardViewList = new List<(CardType, int)>();

    private CardInfo[,] cardInfos;

    public CardView GetCardView(CardType cardType)
    {
        return cardViewDatas.FirstOrDefault(data  => data.cardType == cardType);
    }

    public void Init(CardInfo[,] cardInfos, int totalCards)
    {
        ClearPrevData();

        this.totalCards = totalCards;
        pairsCounter = totalCards / 2;
        if (cardInfos != null && cardInfos.Length > 1)
        {
            this.cardInfos = cardInfos;
            foreach (CardInfo cardInfo in cardInfos)
            {
                if (cardInfo.isMatched) continue;

                cardViewList.Add((cardInfo.cardType, 0));
            }

            return;
        }

        Array cardTypes = Enum.GetValues(typeof(CardType));
        foreach (CardType cardType in cardTypes)
        {
            cardViewList.Add((cardType, 0));
        }
    }

    private void ClearPrevData()
    {
        cardViewList.Clear();
        cardInfos = null;
    }

    public CardType GetCardType(int x, int y)
    {
        return cardInfos[x, y].cardType;
    }

    public CardType GetCardType()
    {
        System.Random rand = new System.Random();
        int randomIndex = rand.Next(pairsCounter);
        (CardType, int) pair = cardViewList[randomIndex];

        int value = pair.Item2;
        value++;
        cardViewList[randomIndex] = (pair.Item1, value);

        if (cardViewList[randomIndex].value == maxPairCount)
        {       
            cardViewList.Remove((pair.Item1, value));
            pairsCounter--;
        }

        return pair.Item1;
    }
}
