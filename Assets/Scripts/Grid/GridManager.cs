using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private CardSO cardsSO;
    [SerializeField] private GameObject card;
    [SerializeField] private CardManager cardManager;

    [Header("XY Coordinates")]
    [SerializeField] private int xDim;
    [SerializeField] private int yDim;
    [SerializeField] private float xSpacing, ySpacing;

    private float xScale, yScale;

    public void Init(CardInfo[,] cardInfos)
    {
        cardsSO.InitCardsDict(cardInfos);
        cardManager.InitCards(xDim, yDim);
        cardManager.InitCardInfos(xDim, yDim, cardInfos);

        var cardTransform = card.transform.GetChild(0);
        xScale = cardTransform.localScale.x / 2;
        yScale = cardTransform.localScale.y / 2;

        Debug.Log($"xScale: {xScale}, yScale: {yScale}");

        InitBoard();
        cardManager.OnCardsInitialized();
    }

    private void InitBoard()
    {
        for (int y = 0; y < yDim * 2; y+=2)
        {
            for (int x = 0; x < xDim * 2; x+=2)
            {
                if (!cardManager.IsCardMatched(x/2,  y/2))
                    SpawnCard(x, y);
            }
        }
    }

    private void SpawnCard(int x, int y)
    {
        GameObject cardInstance = Instantiate(card, GetWorldPos(x, y), Quaternion.identity);
        cardInstance.transform.parent = transform;

        cardManager.AddCard(cardInstance, x/2, y/2);
    }

    private Vector2 GetWorldPos(int x, int y)
    {
        var xSpace = x == 0 ? 0 : xSpacing;
        var ySpace = y == 0 ? 0 : ySpacing;

        return new Vector2(xScale + (x * xScale) + (x * xSpace), yScale + (y * yScale) + (y * ySpace));
    }
}
