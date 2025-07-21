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
    private Dictionary<CardType, int> cardViewDic = new Dictionary<CardType, int>();
    private CardInfo[,] cardInfos;

    public CardView GetCardView(CardType cardType)
    {
        return cardViewDatas.FirstOrDefault(data  => data.cardType == cardType);
    }

    public void InitCardsDict(CardInfo[,] cardInfos)
    {
        ClearPrevData();
        if (cardInfos != null && cardInfos.Length > 1)
        {
            this.cardInfos = cardInfos;
            foreach (CardInfo cardInfo in cardInfos)
            {
                if (cardInfo.isMatched) continue;

                if (cardViewDic.ContainsKey(cardInfo.cardType))
                    cardViewDic[cardInfo.cardType] = 0;
                else
                    cardViewDic.Add(cardInfo.cardType, 0);
            }

            return;
        }

        Array cardTypes = Enum.GetValues(typeof(CardType));
        foreach (CardType cardType in cardTypes)
            cardViewDic.Add(cardType, 0);
    }

    private void ClearPrevData()
    {
        cardViewDic.Clear();
        cardInfos = null;
    }

    public CardType GetCardType(int x, int y)
    {
        return cardInfos[x, y].cardType;
    }

    public CardType GetCardType()
    {
        System.Random rand = new System.Random();
        var keyValuePair = cardViewDic.ElementAt(rand.Next(cardViewDic.Count));

        Debug.Log($"KeyValuePair: {keyValuePair.Key}");

        if (cardViewDic.ContainsKey(keyValuePair.Key))
        {
            cardViewDic[keyValuePair.Key]++;
            if (cardViewDic[keyValuePair.Key] == maxPairCount)
                cardViewDic.Remove(keyValuePair.Key);
        }

        return keyValuePair.Key;
    }
}
