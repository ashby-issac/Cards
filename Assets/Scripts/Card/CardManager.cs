using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private Card[,] cards;

    private Card firstShownCard = null, secondShowCard = null;
    public bool BlockInput = false;

    public static CardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitCards(int xDim, int yDim)
    {
        cards = new Card[xDim, yDim];
    }

    public void AddCard(GameObject cardInstance, int x, int y)
    {
        cards[x, y] = new Card();

        cards[x, y] = cardInstance.GetComponent<Card>();
        cards[x, y].Init(x, y, isFlipped: false);
    }

    public void OnCardsInitialized()
    {
        StartCoroutine(ShowAllCards());
    }

    public IEnumerator ShowAllCards()
    {
        BlockInput = true;
        yield return new WaitForSeconds(2f);

        foreach (var card in cards)
        {
            card.CardRotator.TriggerCardsRotation(showCard: true);
        }

        yield return new WaitForSeconds(1f);

        foreach (var card in cards)
        {
            card.CardRotator.TriggerCardsRotation(showCard: false, () => BlockInput = false);
        }
    }

    public void ShowSpecificCard(int x, int y)
    {
        cards[x, y].CardRotator.TriggerCardsRotation(showCard: true);

        if (firstShownCard == null)
            firstShownCard = cards[x, y];
        else if (secondShowCard == null)
        {
            secondShowCard = cards[x, y];
            BlockInput = true;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);
        if (firstShownCard.CardType == secondShowCard.CardType)
            HideMatchedCards();
        else
            FlipUnmatchedCards();
    }

    // should be called when there's no match
    public void FlipUnmatchedCards()
    {
        firstShownCard.CardRotator.TriggerCardsRotation(showCard: false);
        secondShowCard.CardRotator.TriggerCardsRotation(showCard: false, () =>
        {
            ResetCards();
        });
    }

    private void ResetCards()
    {
        firstShownCard = secondShowCard = null;
        BlockInput = false;
    }

    public void HideMatchedCards()
    {
        firstShownCard.HideCard();
        secondShowCard.HideCard();
        Invoke(nameof(ResetCards), 0.5f);
    }
}
