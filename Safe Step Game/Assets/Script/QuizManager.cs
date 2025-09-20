using System.Collections.Generic;
using UnityEngine;
using System;

public class QuizManager : MonoBehaviour
{
    // Event yang bisa didengar oleh script lain (mis. QuizButtonManager)
    public event Action OnQuizCompleted;

    [Header("Soal")]
    public CheckBoxController[] questions;
    private int currentQuestion = 0;

    private List<CheckBoxController> wrongQuestions = new List<CheckBoxController>();
    private bool reviewPhase = false;

    [Header("Mini Game Control")]
    public GameObject miniGame3;   // drag Mini Game 3 di sini
    public GameObject reward;      // drag GameObject reward di sini

    private void Start()
    {
        // pastikan reward belum muncul
        if (reward != null) reward.SetActive(false);

        // nonaktifkan semua soal dulu dan set quiz manager pada tiap soal
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i] != null)
            {
                questions[i].gameObject.SetActive(false);
                questions[i].SetQuizManager(this);
            }
        }

        LoadQuestion(currentQuestion);
    }

    // Dipanggil oleh CheckBoxController ketika user menjawab
    public void Answered(bool isCorrect, CheckBoxController question)
    {
        if (!isCorrect && question != null)
        {
            // jangan tambahkan duplicate
            if (!wrongQuestions.Contains(question))
                wrongQuestions.Add(question);
        }

        NextQuestion();
    }

    private void LoadQuestion(int index)
    {
        // pastikan semua soal dinonaktifkan sebelum menampilkan yang index
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i] != null)
                questions[i].gameObject.SetActive(false);
        }

        if (index < questions.Length && questions[index] != null)
        {
            questions[index].gameObject.SetActive(true);
            questions[index].ResetButtons();
        }
    }

    private void NextQuestion()
    {
        // pastikan current question yang tampil dimatikan (jika ada)
        if (currentQuestion < questions.Length && currentQuestion >= 0 && questions[currentQuestion] != null)
        {
            questions[currentQuestion].gameObject.SetActive(false);
        }

        currentQuestion++;

        if (currentQuestion >= questions.Length)
        {
            if (!reviewPhase && wrongQuestions.Count > 0)
            {
                reviewPhase = true;
                questions = wrongQuestions.ToArray();
                wrongQuestions.Clear();
                currentQuestion = 0;
                LoadQuestion(currentQuestion);
            }
            else if (reviewPhase && wrongQuestions.Count > 0)
            {
                questions = wrongQuestions.ToArray();
                wrongQuestions.Clear();
                currentQuestion = 0;
                LoadQuestion(currentQuestion);
            }
            else
            {
                Debug.Log("Semua soal sudah benar!");
                EndQuiz();
            }
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

        // panggil event agar listener (mis. QuizButtonManager) tahu quiz selesai
        OnQuizCompleted?.Invoke();
    }
}


