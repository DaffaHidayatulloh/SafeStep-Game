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
    [Tooltip("Minimal horizontal movement (pixels) dianggap swipe")]
    public float swipeThreshold = 100f;

    [Tooltip("Seberapa jauh image bergerak keluar layar ketika di-swipe")]
    public float offscreenDistance = 800f;

    [Header("Animation")]
    [Tooltip("Kecepatan gerakan (semakin besar = semakin cepat)")]
    public float moveSpeed = 8f;

    [Tooltip("Durasi fade ketika muncul kembali (detik)")]
    public float fadeDuration = 0.3f;

    [Tooltip("Delay ketika image hilang sebelum muncul lagi (detik)")]
    public float hiddenDelay = 0.5f;

    bool hasSwiped = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        startAnchoredPos = rect.anchoredPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (hasSwiped) return;
        // geser hanya pada sumbu X
        rect.anchoredPosition += new Vector2(eventData.delta.x, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hasSwiped) return;

        float dragDistance = rect.anchoredPosition.x - startAnchoredPos.x;

        if (Mathf.Abs(dragDistance) >= swipeThreshold)
        {
            float dir = Mathf.Sign(dragDistance); // +1 kanan, -1 kiri
            hasSwiped = true;

            // matikan interaksi sementara
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            StartCoroutine(SmoothHideAndReturn(dir));
        }
        else
        {
            // balik ke tengah dengan smooth
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

        // slide out + fade out
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

        // tunggu sebentar ketika hidden
        yield return new WaitForSeconds(hiddenDelay);

        // pindahkan langsung kembali ke posisi awal (tetap alpha = 0) lalu fade in
        rect.anchoredPosition = startAnchoredPos;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        // restore interaksi
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        hasSwiped = false;
    }
}