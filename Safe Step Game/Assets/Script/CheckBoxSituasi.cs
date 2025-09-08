using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckBoxSituasi : MonoBehaviour
{
    [Header("Jawaban")]
    public Button[] optionButtons;
    public int[] correctIndexes; // bisa isi 1 atau lebih jawaban benar

    [Header("Feedback")]
    public GameObject[] feedbackObjects;
    // jumlah harus sama dengan jumlah optionButtons
    // misalnya Button[0]  FeedbackObjects[0], dst.

    private QuizSituasiManager quizManager;

    private void Start()
    {
        // matikan semua feedback di awal
        foreach (var fb in feedbackObjects)
        {
            fb.SetActive(false);
        }

        // assign tombol
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    public void SetQuizManager(QuizSituasiManager manager)
    {
        quizManager = manager;
    }

    void CheckAnswer(int index)
    {
        // Nonaktifkan tombol setelah klik
        foreach (var btn in optionButtons)
            btn.interactable = false;

        bool isCorrect = IsCorrect(index);

        // tampilkan feedback sesuai tombol yang dipilih
        if (index < feedbackObjects.Length)
        {
            StartCoroutine(ShowFeedback(feedbackObjects[index], isCorrect));
        }
        else
        {
            Debug.LogWarning("Feedback tidak diset untuk button index " + index);
            quizManager.Answered(isCorrect, this);
        }
    }

    IEnumerator ShowFeedback(GameObject feedbackObj, bool isCorrect)
    {
        feedbackObj.SetActive(true);
        yield return new WaitForSeconds(3f); // tampil 3 detik
        feedbackObj.SetActive(false);

        // matikan soal setelah feedback selesai
        gameObject.SetActive(false);

        // lanjut ke QuizManager
        quizManager.Answered(isCorrect, this);
    }

    public void ResetButtons()
    {
        foreach (var btn in optionButtons)
            btn.interactable = true;
    }

    private bool IsCorrect(int index)
    {
        foreach (int correct in correctIndexes)
        {
            if (index == correct) return true;
        }
        return false;
    }
}


