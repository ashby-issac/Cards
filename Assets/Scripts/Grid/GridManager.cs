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

    private float xHalfScale, yHalfScale;

    public float CardScaleY => yHalfScale * 2;
    public float YSpacing => ySpacing;

    public int XDim => xDim;
    public int YDim => yDim;

    public void Init(CardInfo[,] cardInfos)
    {
        cardsSO.Init(cardInfos, xDim * yDim);
        cardManager.InitCards(xDim, yDim);
        cardManager.InitCardInfos(xDim, yDim, cardInfos);

        var cardTransform = card.transform.GetChild(0);
        xHalfScale = cardTransform.localScale.x / 2;
        yHalfScale = cardTransform.localScale.y / 2;

        Debug.Log($"xScale: {xHalfScale}, yScale: {yHalfScale}");

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

        return new Vector2(xHalfScale + (x * xHalfScale) + (x * xSpace), yHalfScale + (y * yHalfScale) + (y * ySpace));
    }
}
