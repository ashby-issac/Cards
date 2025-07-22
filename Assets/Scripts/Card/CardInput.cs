using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInput : MonoBehaviour
{
    public Card card;
    public bool isClickable = true;

    private void OnMouseDown()
    {
        Debug.Log($"OnMouseDown");
        if (!CardManager.Instance.BlockInput && isClickable)
        {
            Debug.Log($"isClickable: {card.X}, {card.Y}");
            CardManager.Instance.ShowSpecificCard(card.X, card.Y);
        }
    }
}
