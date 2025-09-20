using UnityEngine;
using UnityEngine.UI;

public class QuizSituasiButtonManager : MonoBehaviour
{
    [Header("Target Button")]
    public Image buttonImage;              // image di button
    public Sprite defaultSprite;           // sprite default
    public Sprite completedSprite;         // sprite kalau quiz selesai semua benar

    [Header("Quiz Reference")]
    public QuizSituasiManager quizManager; // drag script QuizSituasiManager di sini

    [Header("Save System")]
    public string saveKey = "QuizSituasi_AllCorrect"; // key untuk PlayerPrefs

    private void Start()
    {
        // Load status saat game mulai
        int status = PlayerPrefs.GetInt(saveKey, 0);
        if (status == 1)
            SetCompletedState();
        else
            SetDefaultState();
    }

    private void OnEnable()
    {
        if (quizManager != null)
            quizManager.OnAllCorrectCompleted += HandleAllCorrectCompleted;
    }

    private void OnDisable()
    {
        if (quizManager != null)
            quizManager.OnAllCorrectCompleted -= HandleAllCorrectCompleted;
    }

    private void HandleAllCorrectCompleted()
    {
        SetCompletedState();
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();
    }

    private void SetCompletedState()
    {
        if (buttonImage != null && completedSprite != null)
            buttonImage.sprite = completedSprite;
    }

    private void SetDefaultState()
    {
        if (buttonImage != null && defaultSprite != null)
            buttonImage.sprite = defaultSprite;
    }
}
