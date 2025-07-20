using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInput : MonoBehaviour
{
    public Card card;

    private void OnMouseDown()
    {
        Debug.Log($"OnMouseDown");
        if (!CardManager.Instance.BlockInput)
            CardManager.Instance.ShowSpecificCard(card.X, card.Y);
    }
}
