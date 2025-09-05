using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector3 startPosition; // posisi awal piece

    public Transform targetSlot; // slot tujuan yang benar untuk piece ini
    private bool placed = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (placed) return; // kalau sudah ditempatkan, tidak bisa di-drag lagi

        originalParent = transform.parent;
        startPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(canvas.transform); // supaya piece di atas UI lain saat drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (placed) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // cek apakah sudah dekat dengan slot target
        if (Vector2.Distance(rectTransform.position, targetSlot.position) < 50f)
        {
            // snap ke slot
            rectTransform.position = targetSlot.position;
            transform.SetParent(targetSlot);
            placed = true;
        }
        else
        {
            // balikin ke posisi awal
            transform.SetParent(originalParent);
            rectTransform.position = startPosition;
        }

    }
    public bool IsPlaced()
    {
        return placed;
    }
}

