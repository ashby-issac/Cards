using UnityEngine;

public enum CardType
{
    Card1 = 0,
    Card2 = 1, 
    Card3 = 2, 
    Card4 = 3, 
    Card5 = 4, 
    Card6 = 5,
    Card7 = 6,
    Card8 = 7,
}

[System.Serializable]
public class CardInfo
{
    public int x, y;
    public bool isMatched;
    public CardType cardType;
    public CardView cardView;
}

public class Card : MonoBehaviour
{
    [SerializeField] private Transform front;
    [SerializeField] private Transform back;
    [SerializeField] private MeshRenderer cardFrontRenderer;

    public CardSO cardSO;
    public CardRotator CardRotator;
    public CardInput CardInput;

    private CardInfo cardInfo;

    public int X => cardInfo.x;
    public int Y => cardInfo.y;

    public CardType CardType => cardInfo.cardType;
    public CardInfo CardInfo => cardInfo;

    public void Init(int x, int y)
    {
        cardInfo = new CardInfo();

        cardInfo.x = x;
        cardInfo.y = y;

        cardInfo.cardType = Level.SavedCards ? cardSO.GetCardType(x, y) : cardSO.GetCardType();
        cardInfo.cardView = cardSO.GetCardView(cardInfo.cardType);

        cardFrontRenderer.material.color = cardInfo.cardView.color;
    }

    public void HideCard()
    {
        cardInfo.isMatched = true;
        gameObject.SetActive(false);
        CardInput.enabled = false;
    }
}
