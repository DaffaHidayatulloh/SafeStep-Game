using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SwipeImage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Vector2 startAnchoredPos;
    private CanvasGroup canvasGroup;

    [Header("Swipe settings")]
    public float swipeThreshold = 100f;
    public float offscreenDistance = 800f;
    public float maxDragDistance = 200f;

    [Header("Animation")]
    public float moveSpeed = 8f;
    public float fadeDuration = 0.3f;
    public float hiddenDelay = 0.5f;

    [Header("Objective Settings")]
    [Tooltip("Urutan arah swipe yang benar (contoh: Right, Left, Right, Left)")]
    public string[] correctDirections;

    [Tooltip("Instruksi teks yang muncul sesuai urutan swipe")]
    public string[] instructionTexts;

    [Tooltip("Referensi ke UI Text legacy (drag dari Inspector)")]
    public Text instructionTextUI;

    private int currentIndex = 0;
    private bool hasSwiped = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        startAnchoredPos = rect.anchoredPosition;
        ResetState();

        // Set instruksi awal kalau ada
        if (instructionTextUI != null && instructionTexts.Length > 0)
        {
            instructionTextUI.text = instructionTexts[0];
        }
    }

    void ResetState()
    {
        rect.anchoredPosition = startAnchoredPos;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        hasSwiped = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (hasSwiped) return;

        rect.anchoredPosition += new Vector2(eventData.delta.x, 0f);

        // batasi area drag
        float offsetX = rect.anchoredPosition.x - startAnchoredPos.x;
        offsetX = Mathf.Clamp(offsetX, -maxDragDistance, maxDragDistance);
        rect.anchoredPosition = startAnchoredPos + new Vector2(offsetX, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hasSwiped) return;

        float dragDistance = rect.anchoredPosition.x - startAnchoredPos.x;

        if (Mathf.Abs(dragDistance) >= swipeThreshold)
        {
            float dir = Mathf.Sign(dragDistance); // +1 kanan, -1 kiri
            string swipeDir = dir > 0 ? "Right" : "Left";

            string expectedDir = correctDirections.Length > 0 ? correctDirections[currentIndex] : "Right";

            if (swipeDir == expectedDir)
            {
                hasSwiped = true;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                StartCoroutine(SmoothHideAndReturn(dir));
            }
            else
            {
                //kasih animasi disini
                StartCoroutine(ShakeAndReset());
            }
        }
        else
        {
            StartCoroutine(SmoothMoveTo(startAnchoredPos));
        }
    }

    IEnumerator SmoothMoveTo(Vector2 target)
    {
        while (Vector2.Distance(rect.anchoredPosition, target) > 0.5f)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        rect.anchoredPosition = target;
    }

    IEnumerator SmoothHideAndReturn(float direction)
    {
        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startAnchoredPos + new Vector2(direction * offscreenDistance, 0f);

        // keluar + fade
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed * 0.5f);
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        rect.anchoredPosition = targetPos;
        canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(hiddenDelay);

        // reset ke posisi awal + fade in
        rect.anchoredPosition = startAnchoredPos;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        // pindah ke objective berikutnya
        currentIndex++;
        if (currentIndex >= correctDirections.Length)
        {
            currentIndex = 0; // loop ulang, bisa diubah kalau mau stop di akhir
        }

        // update instruksi text
        if (instructionTextUI != null && instructionTexts.Length > 0)
        {
            int textIndex = Mathf.Min(currentIndex, instructionTexts.Length - 1);
            instructionTextUI.text = instructionTexts[textIndex];
        }

        ResetState();
    }

    IEnumerator ShakeAndReset()
    {
        Vector2 originalPos = rect.anchoredPosition;

        float elapsed = 0f;
        float duration = 0.3f;
        float magnitude = 20f;

        while (elapsed < duration)
        {
            float x = Mathf.Sin(elapsed * 50f) * magnitude;
            rect.anchoredPosition = originalPos + new Vector2(x, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(SmoothMoveTo(startAnchoredPos));
    }
}

