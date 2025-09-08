using UnityEngine;

public class QuizSituasiManager : MonoBehaviour
{
    [Header("Soal")]
    public CheckBoxSituasi[] questions;
    private int currentQuestion = 0;

    [Header("Mini Game Control")]
    public GameObject miniGame3;   // drag Mini Game 3 di sini
    public GameObject reward;      // drag GameObject reward di sini

    private void Start()
    {
        // pastikan reward belum muncul
        if (reward != null) reward.SetActive(false);

        // matikan semua soal di awal
        foreach (var q in questions)
        {
            q.gameObject.SetActive(false);
            q.SetQuizManager(this);
        }

        // tampilkan soal pertama
        LoadQuestion(currentQuestion);
    }

    public void Answered(bool isCorrect, CheckBoxSituasi question)
    {
        // tidak perlu simpan wrongQuestions lagi
        NextQuestion();
    }

    private void LoadQuestion(int index)
    {
        if (index < questions.Length)
        {
            questions[index].gameObject.SetActive(true);
            questions[index].ResetButtons();
        }
    }

    private void NextQuestion()
    {
        currentQuestion++;

        if (currentQuestion >= questions.Length)
        {
            // semua soal sudah dijawab (benar/salah)
            Debug.Log("Quiz selesai!");
            EndQuiz();
        }
        else
        {
            LoadQuestion(currentQuestion);
        }
    }

    private void EndQuiz()
    {
        if (miniGame3 != null) miniGame3.SetActive(false);
        if (reward != null) reward.SetActive(true);
    }
}

