using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[System.Serializable]
public class CharacterData
{
    public string characterName; // "Male" atau "Female"
}
public class CheckBoxSituasi : MonoBehaviour
{
    [Header("Jawaban")]
    public Button[] optionButtons;
    public int[] correctIndexes; // bisa isi 1 atau lebih jawaban benar

    [Header("Feedback")]
    public GameObject[] feedbackObjects;
    // jumlah harus sama dengan jumlah optionButtons

    [Header("Character Display")]
    public Image characterImage;        // tempat menampilkan sprite karakter di UI
    public Sprite[] maleSprites;        // [0] normal, [1] happy, [2] sad
    public Sprite[] femaleSprites;      // [0] normal, [1] happy, [2] sad

    private Sprite currentNormalSprite; // sprite dasar (normal)
    private string savePath;
    private QuizSituasiManager quizManager;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/character.json";

        LoadCharacter(); // ambil karakter tersimpan

        // matikan semua feedback di awal
        foreach (var fb in feedbackObjects)
            fb.SetActive(false);

        // assign tombol
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    private void LoadCharacter()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save data tidak ditemukan: " + savePath);
            return;
        }

        string json = File.ReadAllText(savePath);
        CharacterData data = JsonUtility.FromJson<CharacterData>(json);

        if (data.characterName == "Male")
        {
            currentNormalSprite = maleSprites[0];
            characterImage.sprite = maleSprites[0];
        }
        else
        {
            currentNormalSprite = femaleSprites[0];
            characterImage.sprite = femaleSprites[0];
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

        // mainkan suara
        if (isCorrect)
            AudioManager.instance.PlaySFX(0); // suara benar
        else
            AudioManager.instance.PlaySFX(1); // suara salah

        // ubah ekspresi karakter
        StartCoroutine(ChangeCharacterExpression(isCorrect));

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

    IEnumerator ChangeCharacterExpression(bool isCorrect)
    {
        if (characterImage == null) yield break;

        Sprite[] spriteSet;
        if (currentNormalSprite == maleSprites[0])
            spriteSet = maleSprites;
        else
            spriteSet = femaleSprites;

        characterImage.sprite = isCorrect ? spriteSet[1] : spriteSet[2];
        yield return new WaitForSeconds(3f); // ekspresi berubah selama 1 detik
        characterImage.sprite = currentNormalSprite;
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


