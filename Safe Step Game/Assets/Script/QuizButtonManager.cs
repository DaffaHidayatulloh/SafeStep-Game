using UnityEngine;
using UnityEngine.UI;

public class QuizButtonManager : MonoBehaviour
{
    [Header("Quiz Manager")]
    public QuizManager quizManager; // drag QuizManager ke sini

    [Header("Button UI")]
    public Button quizButton;          // tombol quiz
    public Sprite defaultSprite;       // sprite sebelum berhasil
    public Sprite completedSprite;     // sprite setelah berhasil

    private string saveKey = "QuizCompleted";

    private void Start()
    {
        // Load status dari PlayerPrefs
        if (PlayerPrefs.GetInt(saveKey, 0) == 1)
            SetCompletedState();
        else
            SetDefaultState();

        // subscribe ke event quiz selesai
        if (quizManager != null)
            quizManager.OnQuizCompleted += HandleQuizCompleted;
    }

    private void OnDestroy()
    {
        if (quizManager != null)
            quizManager.OnQuizCompleted -= HandleQuizCompleted;
    }

    private void HandleQuizCompleted()
    {
        SetCompletedState();
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();
    }

    private void SetCompletedState()
    {
        if (quizButton != null && completedSprite != null)
            quizButton.image.sprite = completedSprite;

        if (quizButton != null)
            quizButton.interactable = true;
    }

    private void SetDefaultState()
    {
        if (quizButton != null && defaultSprite != null)
            quizButton.image.sprite = defaultSprite;

        if (quizButton != null)
            quizButton.interactable = true;
    }

    // Reset manual kalau PlayerPrefs dihapus
    public void ResetQuizButton()
    {
        PlayerPrefs.DeleteKey(saveKey);
        SetDefaultState();
    }
}

