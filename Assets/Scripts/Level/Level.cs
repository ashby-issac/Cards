using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zOffset = 1f;

    public static bool SavedCards = false;
    private CardLocalData cardLocalData;

    private void Awake()
    {
        CardInfo[,] cardInfos = null;
        if (SaveSystem.HasKey(CardManager.CardsData_Constant))
        {
            cardLocalData = SaveSystem.LoadJson<CardLocalData>(CardManager.CardsData_Constant);

            cardInfos = CardUtility.ConvertTo2D(cardLocalData.cardInfos);
            if (cardInfos != null &&
                cardInfos.GetLength(0) * cardInfos.GetLength(1) == gridManager.XDim * gridManager.YDim)
            {
                SavedCards = true;
            }
            else
            {
                SaveSystem.DeleteSavedFile(CardManager.CardsData_Constant);
            }
        }

        gridManager.Init(cardLocalData != null && cardInfos != null ? cardInfos : new CardInfo[0, 0]);
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        float value = (gridManager.YDim * gridManager.CardScaleY) + (gridManager.YDim * gridManager.YSpacing);
        Debug.Log($"value: {value}");
        mainCamera.transform.position = new Vector3(0, value/2, - (value + zOffset));
    }

    public void Restart()
    {

    }
}
