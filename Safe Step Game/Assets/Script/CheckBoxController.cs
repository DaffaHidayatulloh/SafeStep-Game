using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CheckBoxController : MonoBehaviour
{
    [Header("Jawaban")]
    public Button[] optionButtons;
    public int correctIndex = 0;

    [Header("Feedback")]
    public GameObject wrongFeedback;
    public GameObject correctFeedback;

    [Header("Score System")]
    public int levelIndex = 1;
    public int miniGameIndex = 1;
    public int rewardScore = 50;
    public Text scoreDisplayText;

    [Header("Character Display")]
    public Image characterImage;
    public Sprite[] maleSprites;   // [0]=normal, [1]=happy, [2]=sad
    public Sprite[] femaleSprites; // [0]=normal, [1]=happy, [2]=sad

    private Sprite currentNormalSprite;
    private Sprite currentHappySprite;
    private Sprite currentSadSprite;

    private QuizManager quizManager;
    private bool alreadyCompleted = false;
    private string characterGender; // "Male" atau "Female"

    private void Start()
    {
        wrongFeedback.SetActive(false);
        correctFeedback.SetActive(false);

        LoadCharacterData();

        SetCharacterSprites();

        string key = $"Level{levelIndex}_MiniGame{miniGameIndex}_Done";
        alreadyCompleted = PlayerPrefs.GetInt(key, 0) == 1;

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


        if (isCorrect)
            StartCoroutine(ChangeCharacterExpression(currentHappySprite));
        else
            StartCoroutine(ChangeCharacterExpression(currentSadSprite));

        yield return new WaitForSeconds(3f); // feedback tampil 3 detik
        feedbackObj.SetActive(false);

        // setelah feedback selesai, matikan soal
        gameObject.SetActive(false);

        // kalau jawab benar, kasih reward
        if (isCorrect)
            HandleScoring();
        else
            scoreDisplayText.text = "+0";

        quizManager.Answered(isCorrect, this);
    }

    IEnumerator ChangeCharacterExpression(Sprite expressionSprite)
    {
        // Simpan sprite normal
        characterImage.sprite = expressionSprite;
        yield return new WaitForSeconds(3f); // tampil 1 detik
        characterImage.sprite = currentNormalSprite; // kembali ke normal
    }

    void HandleScoring()
    {
        string doneKey = $"Level{levelIndex}_MiniGame{miniGameIndex}_Done";

        if (!alreadyCompleted)
        {
            int currentScore = PlayerPrefs.GetInt($"Score_Level{levelIndex}", 0);
            currentScore += rewardScore;

            PlayerPrefs.SetInt($"Score_Level{levelIndex}", currentScore);
            PlayerPrefs.SetInt(doneKey, 1);
            PlayerPrefs.Save();

            scoreDisplayText.text = "+150";
            alreadyCompleted = true;
        }
        else
        {
            scoreDisplayText.text = "+0";
        }
    }

    public void ResetButtons()
    {
        foreach (var btn in optionButtons)
            btn.interactable = true;
    }

    void LoadCharacterData()
    {
        string savePath = Application.persistentDataPath + "/character.json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);
            characterGender = data.characterName; // "Male" atau "Female"
        }
        else
        {
            // Default ke Male kalau file belum ada
            characterGender = "Male";
        }
    }

    void SetCharacterSprites()
    {
        if (characterGender == "Male")
        {
            currentNormalSprite = maleSprites[0];
            currentHappySprite = maleSprites[1];
            currentSadSprite = maleSprites[2];
        }
        else
        {
            currentNormalSprite = femaleSprites[0];
            currentHappySprite = femaleSprites[1];
            currentSadSprite = femaleSprites[2];
        }

        // Tampilkan sprite awal
        characterImage.sprite = currentNormalSprite;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterName; // "Male" atau "Female"
    }
}





