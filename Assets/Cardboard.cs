using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Cardboard : MonoBehaviour
{
    public bool WasInHandLastFrame = false;
    public AnimationCurve AnimationToDiscardX;
    public AnimationCurve AnimationToDiscardY;
    public AnimationCurve AnimationFromDrawX;
    public AnimationCurve AnimationFromDrawY;
    private Vector3 discardPileUIPosition;
    private Vector3 drawPileUIPosition;
    public Vector3Spring LocalPosition;
    public Vector3 PlayCardTowardsDiscardVelocity;
    public Vector3 DrawCardTowardsHandVelocity;
    public Vector3Spring LocalRotation;
    public Vector3Spring LocalScale;
    public float ScaleInPile;
    public Vector3 DrawPileRotation;
    public Vector3 DiscardPileRotation;
    private bool IsInitialized;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsInitialized) {
            Initialize();
            IsInitialized = true;
            LocalPosition.Value = drawPileUIPosition;
            LocalPosition.SetValue(drawPileUIPosition);
        }
    }

    private void Initialize() {
        discardPileUIPosition = transform.parent.InverseTransformPoint(FindObjectOfType<DiscardPileUICounter>().transform.parent.position);
        drawPileUIPosition = transform.parent.InverseTransformPoint(FindObjectOfType<DrawPileUICounter>().transform.parent.position);
        LocalScale.Value = ScaleInPile * Vector3.one;
        LocalScale.SetValue(ScaleInPile * Vector3.one);
        LocalRotation.Value = DrawPileRotation;
        LocalRotation.SetValue(DrawPileRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = LocalPosition.Value;
        transform.localEulerAngles = LocalRotation.Value;
        transform.localScale = LocalScale.Value;
    }

    public void SetImage(Card card) {
        Image image_component = GetComponent<Image>();
        image_component.sprite = Sprite.Create(card.Details.Image, new Rect(0, 0, 620, 880), new Vector2());
    }

    public void SetSelectionKey(int number) {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = number.ToString();
    }

    public void AnimatePlayAndDiscard() {
        // Debug.Log("Started animating discard");
        // StartCoroutine(GoToDiscard(transform.localPosition));
        LocalPosition.Value = discardPileUIPosition;
        LocalPosition.SetVelocityInDirectionOfGoal(PlayCardTowardsDiscardVelocity);
        LocalScale.Value = ScaleInPile * Vector3.one;
        LocalRotation.Value = DiscardPileRotation;
    }

    private IEnumerator GoToDiscard(Vector3 old_position_in_hand) {
        float animation_progress = 0.0f;
        while (animation_progress < 1.0f) {
            animation_progress += Time.deltaTime;
            transform.localPosition = Vector3.right * Mathf.Lerp(old_position_in_hand.x, discardPileUIPosition.x, AnimationToDiscardX.Evaluate(animation_progress))
                                    + Vector3.up * Mathf.Lerp(old_position_in_hand.y, discardPileUIPosition.y, AnimationToDiscardY.Evaluate(animation_progress));
            yield return null;
        }
        if (!WasInHandLastFrame) {
            gameObject.SetActive(false);
        }
        yield break;
    }

    public void AnimateDraw() {
        // StartCoroutine(DrawFromDiscard(transform.localPosition));
        if (!IsInitialized) {
            Initialize();
            IsInitialized = true;
        }
        LocalPosition.SetValue(drawPileUIPosition);
        LocalPosition.SetVelocityInDirectionOfGoal(DrawCardTowardsHandVelocity);
        LocalScale.SetValue(ScaleInPile * Vector3.one);
        LocalScale.Value = Vector3.one;
        LocalRotation.SetValue(DrawPileRotation);
        LocalRotation.Value = Vector3.zero;
    }

    private IEnumerator DrawFromDiscard(Vector3 new_position_in_hand) {
        float animation_progress = 0.0f;
        while (animation_progress < 1.0f) {
            animation_progress += Time.deltaTime;
            transform.localPosition = Vector3.right * Mathf.Lerp(drawPileUIPosition.x, new_position_in_hand.x, AnimationFromDrawX.Evaluate(animation_progress))
                                    + Vector3.up * Mathf.Lerp(drawPileUIPosition.y, new_position_in_hand.y, AnimationFromDrawY.Evaluate(animation_progress));
            yield return null;
        }
        yield break;
    }
}
