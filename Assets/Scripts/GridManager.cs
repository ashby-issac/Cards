using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float yDim, xDim;
    [SerializeField] private GameObject card;

    [SerializeField] private float xSpacing, ySpacing;

    private float xScale, yScale;

    private void Awake()
    {
        xScale = card.transform.localScale.x / 2;
        yScale = card.transform.localScale.y / 2;

        InitBoard();
    }

    private void InitBoard()
    {
        for (int y = 0; y < yDim * 2; y+=2)
        {
            for (int x = 0; x < xDim * 2; x+=2)
            {
                SpawnCard(x, y);
            }
        }
    }

    private void SpawnCard(int x, int y)
    {
        GameObject cardInstance = Instantiate(card, GetWorldPos(x, y), Quaternion.identity);
        cardInstance.transform.parent = transform;


    }

    private Vector2 GetWorldPos(int x, int y)
    {
        var xSpace = x == 0 ? 0 : xSpacing;
        var ySpace = y == 0 ? 0 : ySpacing;

        return new Vector2(xScale + (x * xScale) + (x * xSpace), yScale + (y * yScale) + (y * ySpace));
    }
}
