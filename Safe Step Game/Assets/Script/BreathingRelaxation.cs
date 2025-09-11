using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class BreathingRelaxation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    [Header("UI Elements")]
    public RectTransform circle;   // UI Lingkaran
    public Image circleImage;      // Komponen Image lingkaran
    public Text instructionText;   // Teks instruksi
    public Slider progressBar;     // Progress total 3 siklus

    [Header("Durasi (detik)")]
    public float inhaleDuration = 4f;
    public float holdDuration = 2f;
    public float exhaleDuration = 4f;

    [Header("Skala Lingkaran")]
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("Jumlah Siklus")]
    public int totalCycles = 3; // total 3 siklus
    private int currentCycle = 0;

    private bool isHoldingTap = false;

    private void Start()
    {
        // lingkaran mulai dari kecil
        circle.localScale = minScale;
        circleImage.color = Color.white;

        progressBar.minValue = 0f;
        progressBar.maxValue = totalCycles; // total 3
        progressBar.value = 0f;

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
                if (currentCycle == 0)
                {
                    progressBar.value = 0; // gagal di siklus 1  reset
                }
                continue; // ulangi siklus yang sama
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
                if (currentCycle == 0)
                {
                    progressBar.value = 0; // gagal di siklus 1  reset
                }
                continue;
            }

            // EXHALE
            instructionText.text = "Exhale... Lepaskan";
            yield return StartCoroutine(WaitForReleaseAndScale(circle, maxScale, minScale, exhaleDuration));
            circleImage.color = Color.white;

            // siklus selesai dengan sukses  naikkan progress
            currentCycle++;
            progressBar.value = currentCycle;
        }

        instructionText.text = "Selesai!";
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








