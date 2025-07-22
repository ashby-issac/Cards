using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInput : MonoBehaviour
{
    public Card card;
    public bool isClickable = true;

    private void OnMouseDown()
    {
        if (!CardManager.Instance.BlockInput && isClickable)
        {
            CardManager.Instance.ShowSpecificCard(card.X, card.Y);
        }
    }
}
