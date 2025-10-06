using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckBoxController : MonoBehaviour
{
    [Header("Jawaban")]
    public Button[] optionButtons;
    public int correctIndex = 0;

    [Header("Feedback")]
    public GameObject wrongFeedback;
    public GameObject correctFeedback;

    [Header("Score System")]
    public int levelIndex = 1;          // Level ke berapa
    public int miniGameIndex = 1;       // Mini game ke berapa (1-3)
    public int rewardScore = 50;       // Score reward untuk mini game ini
    public Text scoreDisplayText;       // Tampilkan score hasil

    private QuizManager quizManager;
    private bool alreadyCompleted = false; // Cek apakah sudah pernah dikerjakan

    private void Start()
    {
        wrongFeedback.SetActive(false);
        correctFeedback.SetActive(false);

        // Cek apakah mini game ini sudah pernah diselesaikan
        string key = $"Level{levelIndex}_MiniGame{miniGameIndex}_Done";
        alreadyCompleted = PlayerPrefs.GetInt(key, 0) == 1;

        // Pasang listener ke semua tombol
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    public void SetQuizManager(QuizManager manager)
    {
        quizManager = manager;
    }

    void CheckAnswer(int index)
    {
        foreach (var btn in optionButtons)
            btn.interactable = false;

        if (index == correctIndex)
        {
            AudioManager.instance.PlaySFX(0);
            StartCoroutine(ShowFeedback(correctFeedback, true));
        }
        else
        {
            AudioManager.instance.PlaySFX(1);
            StartCoroutine(ShowFeedback(wrongFeedback, false));
        }
    }

    IEnumerator ShowFeedback(GameObject feedbackObj, bool isCorrect)
    {
        feedbackObj.SetActive(true);
        yield return new WaitForSeconds(3f); // feedback tampil 3 detik
        feedbackObj.SetActive(false);

        // setelah feedback selesai, matikan soal
        gameObject.SetActive(false);

        // kalau jawab benar, kasih reward
        if (isCorrect)
            HandleScoring();
        else
            scoreDisplayText.text = "+0"; // salah tidak dapat score

        quizManager.Answered(isCorrect, this);
    }

    void HandleScoring()
    {
        string doneKey = $"Level{levelIndex}_MiniGame{miniGameIndex}_Done";

        if (!alreadyCompleted) // baru pertama kali benar
        {
            int currentScore = PlayerPrefs.GetInt($"Score_Level{levelIndex}", 0);
            currentScore += rewardScore;

            PlayerPrefs.SetInt($"Score_Level{levelIndex}", currentScore);
            PlayerPrefs.SetInt(doneKey, 1); // tandai mini game ini sudah selesai
            PlayerPrefs.Save();

            scoreDisplayText.text = "+150"; // tampilkan reward
            alreadyCompleted = true;
        }
        else
        {
            scoreDisplayText.text = "+0"; // sudah pernah diselesaikan, tidak tambah score
        }
    }

    public void ResetButtons()
    {
        foreach (var btn in optionButtons)
            btn.interactable = true;
    }
}




