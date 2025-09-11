using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class BreathingRelaxation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI Elements")]
    public RectTransform circle;
    public Image circleImage;
    public Text instructionText;
    public Slider progressBar;

    [Header("Durasi (detik)")]
    public float inhaleDuration = 4f;
    public float holdDuration = 2f;
    public float exhaleDuration = 4f;

    [Header("Skala Lingkaran")]
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("Jumlah Siklus")]
    public int totalCycles = 3;
    private int currentCycle = 0;

    [Header("Game Flow")]
    public GameObject miniGameObject;  // assign panel minigame 2
    public GameObject rewardObject;    // assign panel reward minigame 2

    private bool isHoldingTap = false;

    private void Start()
    {
        circle.localScale = minScale;
        circleImage.color = Color.white;

        progressBar.minValue = 0f;
        progressBar.maxValue = totalCycles;
        progressBar.value = 0f;

        if (rewardObject != null)
            rewardObject.SetActive(false);

        StartCoroutine(BreathingCycle());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHoldingTap = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHoldingTap = false;
    }

    IEnumerator BreathingCycle()
    {
        currentCycle = 0;

        while (currentCycle < totalCycles)
        {
            // INHALE
            instructionText.text = "Inhale... Tap & Tahan";
            bool inhaleSuccess = false;
            yield return StartCoroutine(ControlledInhale(result => inhaleSuccess = result));
            circleImage.color = inhaleSuccess ? Color.green : Color.red;

            if (!inhaleSuccess)
            {
                circle.localScale = minScale;
                yield return new WaitForSeconds(1f);
                if (currentCycle == 0) progressBar.value = 0;
                continue;
            }

            // HOLD
            instructionText.text = "Hold... Tahan Sebentar";
            bool holdSuccess = false;
            yield return StartCoroutine(ControlledHold(result => holdSuccess = result));
            circleImage.color = holdSuccess ? Color.green : Color.red;

            if (!holdSuccess)
            {
                circle.localScale = minScale;
                yield return new WaitForSeconds(1f);
                if (currentCycle == 0) progressBar.value = 0;
                continue;
            }

            // EXHALE
            instructionText.text = "Exhale... Lepaskan";
            yield return StartCoroutine(WaitForReleaseAndScale(circle, maxScale, minScale, exhaleDuration));
            circleImage.color = Color.white;

            // Sukses siklus
            currentCycle++;
            progressBar.value = currentCycle;
        }

        instructionText.text = "Selesai!";

        // Tunggu 1 detik lalu matikan & aktifkan reward
        yield return new WaitForSeconds(1f);

        if (miniGameObject != null) miniGameObject.SetActive(false);
        if (rewardObject != null) rewardObject.SetActive(true);
    }

    IEnumerator ControlledInhale(System.Action<bool> callback)
    {
        float elapsed = 0f;
        while (!isHoldingTap)
            yield return null;

        while (circle.localScale.x < maxScale.x - 0.01f)
        {
            if (isHoldingTap)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / inhaleDuration);
                circle.localScale = Vector3.Lerp(minScale, maxScale, t);
            }
            else
            {
                callback(false);
                yield break;
            }
            yield return null;
        }

        circle.localScale = maxScale;
        callback(true);
    }

    IEnumerator ControlledHold(System.Action<bool> callback)
    {
        float elapsed = 0f;
        while (elapsed < holdDuration)
        {
            if (!isHoldingTap)
            {
                callback(false);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        callback(true);
    }

    IEnumerator WaitForReleaseAndScale(RectTransform target, Vector3 from, Vector3 to, float duration)
    {
        while (isHoldingTap)
            yield return null;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = to;
    }
}









