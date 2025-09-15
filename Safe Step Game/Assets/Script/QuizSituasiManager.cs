using UnityEngine;
using UnityEngine.UI;

public class QuizSituasiManager : MonoBehaviour
{
    [Header("Soal")]
    public CheckBoxSituasi[] questions;
    private int currentQuestion = 0;

    [Header("Mini Game Control")]
    public GameObject miniGame3;   // drag Mini Game 3 di sini
    public GameObject reward;      // drag GameObject reward di sini

    [Header("Score System")]
    public int levelIndex = 1;       // level ke berapa
    public int miniGameIndex = 3;    // Mini game ke berapa
    public int rewardPerCorrect = 50; // skor per jawaban benar
    public Text scoreDisplayText;    // tampilkan total score dari quiz ini

    private int earnedThisQuiz = 0;  // total yang didapat di sesi quiz sekarang

    private void Start()
    {
        if (reward != null) reward.SetActive(false);

        foreach (var q in questions)
        {
            q.gameObject.SetActive(false);
            q.SetQuizManager(this);
        }

        LoadQuestion(currentQuestion);
    }

    public void Answered(bool isCorrect, CheckBoxSituasi question)
    {
        if (isCorrect)
        {
            // Tambah score kalau soal ini baru pertama kali benar
            int added = HandleScoreForQuestion(currentQuestion);
            earnedThisQuiz += added;
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

        // Tampilkan total earned dari quiz ini
        if (scoreDisplayText != null)
            scoreDisplayText.text = "+" + earnedThisQuiz;
    }

    private int HandleScoreForQuestion(int questionIndex)
    {
        string questionKey = $"Level{levelIndex}_MiniGame{miniGameIndex}_Q{questionIndex}_Completed";
        string scoreKey = $"Score_Level{levelIndex}";

        // Kalau soal ini sudah pernah benar, return 0
        if (PlayerPrefs.GetInt(questionKey, 0) == 1)
        {
            Debug.Log($"Soal {questionIndex + 1} sudah pernah benar. Tidak tambah score.");
            return 0;
        }

        // Kalau baru pertama benar, tambah score
        int currentScore = PlayerPrefs.GetInt(scoreKey, 0);
        currentScore += rewardPerCorrect;
        PlayerPrefs.SetInt(scoreKey, currentScore);

        PlayerPrefs.SetInt(questionKey, 1);
        PlayerPrefs.Save();

        Debug.Log($"Soal {questionIndex + 1} pertama kali benar. Tambah {rewardPerCorrect}. Total: {currentScore}");

        return rewardPerCorrect;
    }
}


