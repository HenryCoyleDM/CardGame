using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CardboardDrawer : MonoBehaviour
{
    public bool WasInHandLastFrame = false;
    public AnimationCurve AnimationToDiscardX;
    public AnimationCurve AnimationToDiscardY;
    public AnimationCurve AnimationFromDrawX;
    public AnimationCurve AnimationFromDrawY;
    private Vector3 discard_pile_UI;
    private Vector3 draw_pile_UI;
    
    // Start is called before the first frame update
    void Start()
    {
        discard_pile_UI = transform.parent.InverseTransformPoint(FindObjectOfType<DiscardPileUICounter>().transform.parent.position);
        draw_pile_UI = transform.parent.InverseTransformPoint(FindObjectOfType<DrawPileUICounter>().transform.parent.position);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        StartCoroutine(GoToDiscard(transform.localPosition));
    }

    private IEnumerator GoToDiscard(Vector3 old_position_in_hand) {
        float animation_progress = 0.0f;
        while (animation_progress < 1.0f) {
            animation_progress += Time.deltaTime;
            transform.localPosition = Vector3.right * Mathf.Lerp(old_position_in_hand.x, discard_pile_UI.x, AnimationToDiscardX.Evaluate(animation_progress))
                                    + Vector3.up * Mathf.Lerp(old_position_in_hand.y, discard_pile_UI.y, AnimationToDiscardY.Evaluate(animation_progress));
            yield return null;
        }
        if (!WasInHandLastFrame) {
            gameObject.SetActive(false);
        }
        yield break;
    }

    public void AnimateDraw() {
        StartCoroutine(DrawFromDiscard(transform.localPosition));
    }

    private IEnumerator DrawFromDiscard(Vector3 new_position_in_hand) {
        float animation_progress = 0.0f;
        while (animation_progress < 1.0f) {
            animation_progress += Time.deltaTime;
            transform.localPosition = Vector3.right * Mathf.Lerp(draw_pile_UI.x, new_position_in_hand.x, AnimationFromDrawX.Evaluate(animation_progress))
                                    + Vector3.up * Mathf.Lerp(draw_pile_UI.y, new_position_in_hand.y, AnimationFromDrawY.Evaluate(animation_progress));
            yield return null;
        }
        yield break;
    }
}
