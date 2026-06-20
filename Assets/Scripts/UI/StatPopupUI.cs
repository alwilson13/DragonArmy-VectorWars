using System.Collections;
using TMPro;
using UnityEngine;

/// Handles a temporary HUD popup message.
/// 
/// The popup fades in, moves slightly upward, then fades out.
/// Used for health, damage, fire rate, and speed upgrade feedback.
public class StatPopupUI : MonoBehaviour
{
    [Header("Popup Settings")]

    [Tooltip("How long the popup stays visible.")]
    [SerializeField] private float lifetime = 1.2f;

    [Tooltip("How far the popup moves upward.")]
    [SerializeField] private float moveUpDistance = 40f;

    private TMP_Text popupText;
    private RectTransform rectTransform;

    private void Awake()
    {
        popupText = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    /// Initializes the popup text and color.
    public void Setup(string message, Color textColor)
    {
        if (popupText != null)
        {
            popupText.text = message;
            popupText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
        }

        StartCoroutine(PopupRoutine(textColor));
    }

    /// Fades the popup in, moves it up, then fades it out.
    private IEnumerator PopupRoutine(Color textColor)
    {
        float timer = 0f;

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(0f, moveUpDistance);

        while (timer < lifetime)
        {
            timer += Time.deltaTime;

            float progress = timer / lifetime;

            // Fade in during the first part, fade out near the end.
            float alpha;

            if (progress < 0.25f)
            {
                alpha = Mathf.Lerp(0f, 1f, progress / 0.25f);
            }
            else
            {
                alpha = Mathf.Lerp(1f, 0f, (progress - 0.25f) / 0.75f);
            }

            if (popupText != null)
            {
                popupText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            }

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, progress);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}