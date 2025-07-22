using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotator : MonoBehaviour
{
    [SerializeField] private float flipDuration = 0.5f;
    [SerializeField] private bool startFlipping = false;

    private Quaternion startRotation, endRotation, targetRotation;
    private float flipTime = 0f;

    private Card[] cards;
    private Action onComplete;

    public float FlipDuration => flipDuration;

    private void Start()
    {
        SetRotation();
    }

    private void SetRotation()
    {
        startRotation = transform.rotation;
        endRotation = startRotation * Quaternion.Euler(0, 180f, 0);
    }

    public void TriggerCardsRotation(bool showCard, Action onComplete = null)
    {
        var card = GetComponent<Card>();
        Debug.Log($"Triggering rotation for card({card.X}, {card.Y}), showCard: {showCard}");

        targetRotation = showCard == false ? startRotation : endRotation;
        startRotation = transform.rotation;

        if (this.onComplete != null)
            this.onComplete = null;

        this.onComplete = onComplete;

        startFlipping = true;
        enabled = startFlipping;
    }

    void Update()
    {
        if (!startFlipping) return;

        flipTime += Time.deltaTime;
        float t = Mathf.Clamp01(flipTime / flipDuration);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            startFlipping = false;
            enabled = startFlipping;
            flipTime = 0;
            transform.rotation = targetRotation;
            onComplete?.Invoke();
        }
    }
}
