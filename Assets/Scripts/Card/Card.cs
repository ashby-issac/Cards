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

public class Card : MonoBehaviour
{
    [SerializeField] private Transform front;
    [SerializeField] private Transform back;
    [SerializeField] private MeshRenderer cardFrontRenderer;

    public CardSO cardSO;

    private int x, y;
    private bool isFlipped;
    private CardType cardType;
    private CardView cardView;

    public int X => x;
    public int Y => y;

    public bool IsFlipped => isFlipped;
    public CardType CardType => cardType;

    public CardRotator CardRotator;
    public CardInput CardInput;

    public void Init(int x, int y, bool isFlipped)
    {
        this.x = x;
        this.y = y;
        this.isFlipped = isFlipped;

        // Move this code and call when the game starts
        cardType = cardSO.GetCardType();
        cardView = cardSO.GetCardView(cardType);

        cardFrontRenderer.material.color = cardView.color;
    }

    public void HideCard()
    {
        gameObject.SetActive(false);
        CardInput.enabled = false;
    }
}
