using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    public static bool SavedCards = false;
    private CardLocalData cardLocalData;

    private void Awake()
    {
        CardInfo[,] cardInfos = null;
        if (SaveSystem.HasKey(CardManager.CardsData_Constant))
        {
            cardLocalData = SaveSystem.LoadJson<CardLocalData>(CardManager.CardsData_Constant);
            Debug.Log($"cardLocalData: {cardLocalData.cardInfos.Count()}");
            Debug.Log($"cardLocalData: {JsonConvert.SerializeObject(cardLocalData.cardInfos)}");
            cardInfos = CardUtility.ConvertTo2D(cardLocalData.cardInfos);
            SavedCards = true;
        }

        gridManager.Init(cardLocalData != null && cardInfos != null ? cardInfos : new CardInfo[0, 0]);
    }

    public void Restart()
    {

    }
}
