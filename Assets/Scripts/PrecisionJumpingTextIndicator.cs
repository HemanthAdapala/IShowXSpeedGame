using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PrecisionJumpingTextIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textIndicator;
    private Coroutine textCoroutine;


    void Start()
    {
        if (textIndicator == null)
        {
            textIndicator = GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnJumpEarly()
    {
        Debug.Log("Jumped too early!");
        string text = "Jumped too early!";
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine(ShowTextAndFade(text));
    }

    private void OnJumpPerfect()
    {
        Debug.Log("Perfect jump!");
        string text = "Perfect jump!";
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine(ShowTextAndFade(text));
    }

    private void OnJumpLate()
    {
        Debug.Log("Jumped too late!");
        string text = "Jumped too late!";
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine(ShowTextAndFade(text));
    }

    public IEnumerator ShowTextAndFade(string text)
    {
        textIndicator.SetText(text);
        textIndicator.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.3f);
        yield return textIndicator.DOFade(0, 0.5f).WaitForCompletion();
        ResetTextIndicator();
    }

    private void ResetTextIndicator()
    {
        textIndicator.color = new Color(1, 1, 1, 1);
        textIndicator.SetText("");
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
