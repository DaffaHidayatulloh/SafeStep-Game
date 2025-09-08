using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
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

        foreach (var q in questions)
        {
            q.gameObject.SetActive(false);
            q.SetQuizManager(this);
        }

        LoadQuestion(currentQuestion);
    }

    public void Answered(bool isCorrect, CheckBoxController question)
    {
        if (!isCorrect)
        {
            wrongQuestions.Add(question);
        }

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
    }
}


