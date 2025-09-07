using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckBoxController : MonoBehaviour

{
    [Header("Buttons")]
    public Button[] optionButtons; // drag 4 button ke sini lewat inspector
    public int correctIndex = 0;   // tentukan index jawaban benar (0 = button pertama)

    [Header("Feedback Objects")]
    public GameObject wrongFeedback;   // drag GameObject untuk feedback salah
    public GameObject correctFeedback; // drag GameObject untuk feedback benar

    private void Start()
    {
        // pastikan feedback tidak aktif di awal
        wrongFeedback.SetActive(false);
        correctFeedback.SetActive(false);

        // assign listener ke semua button
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // penting untuk closure
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void CheckAnswer(int index)
    {
        if (index == correctIndex)
        {
            Debug.Log("Jawaban benar!");
            StartCoroutine(ShowFeedback(correctFeedback, 3f));
            // bisa tambahkan reward/pindah scene di sini
        }
        else
        {
            Debug.Log("Jawaban salah!");
            StartCoroutine(ShowFeedback(wrongFeedback, 3f));
        }
    }

    IEnumerator ShowFeedback(GameObject feedbackObj, float duration)
    {
        feedbackObj.SetActive(true);
        yield return new WaitForSeconds(duration);
        feedbackObj.SetActive(false);
    }
}

