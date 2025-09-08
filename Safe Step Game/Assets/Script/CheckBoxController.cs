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

    private QuizManager quizManager;

    private void Start()
    {
        wrongFeedback.SetActive(false);
        correctFeedback.SetActive(false);

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
            StartCoroutine(ShowFeedback(correctFeedback, true));
        }
        else
        {
            StartCoroutine(ShowFeedback(wrongFeedback, false));
        }
    }

    IEnumerator ShowFeedback(GameObject feedbackObj, bool isCorrect)
    {
        feedbackObj.SetActive(true);
        yield return new WaitForSeconds(3f); // feedback tampil 3 detik
        feedbackObj.SetActive(false);

        // setelah feedback selesai, soal dimatikan
        gameObject.SetActive(false);

        quizManager.Answered(isCorrect, this);
    }

    public void ResetButtons()
    {
        foreach (var btn in optionButtons)
            btn.interactable = true;
    }
}



