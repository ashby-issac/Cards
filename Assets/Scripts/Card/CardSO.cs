using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CardView
{
    public CardType cardType;
    public Color color; // can use sprite instead of color
}

[CreateAssetMenu(fileName = "CardSO", menuName = "CardSO")]
public class CardSO : ScriptableObject
{
    public CardView[] cardViewDatas;

    private Dictionary<CardType, int> cardViewDic = new Dictionary<CardType, int>();
    private int maxPairCount = 2;

    public void InitCardsDict()
    {
        foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            cardViewDic.Add(cardType, 0);
    }

    public CardView GetCardView(CardType cardType)
    {
        return cardViewDatas.FirstOrDefault(data  => data.cardType == cardType);
    }

    public CardType GetCardType()
    {
        System.Random rand = new System.Random();
        var keyValuePair = cardViewDic.ElementAt(rand.Next(cardViewDic.Count));

        if (cardViewDic.ContainsKey(keyValuePair.Key))
        {
            cardViewDic[keyValuePair.Key]++;
            if (cardViewDic[keyValuePair.Key] == maxPairCount)
                cardViewDic.Remove(keyValuePair.Key);
        }

        return keyValuePair.Key;
    }
}
