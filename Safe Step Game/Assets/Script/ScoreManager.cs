using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Slider progress tiap level
    public Slider sliderLevel1;
    public Slider sliderLevel2;
    public Slider sliderLevel3;

    // Legacy Text untuk display score
    public Text textLevel1;
    public Text textLevel2;
    public Text textLevel3;

    // Maksimal score tiap level (atur sesuai game-mu)
    public int maxScoreLevel1 = 100;
    public int maxScoreLevel2 = 100;
    public int maxScoreLevel3 = 100;

    private int scoreLevel1;
    private int scoreLevel2;
    private int scoreLevel3;

    void Start()
    {
        LoadScores();
        UpdateUI();
    }

    // Fungsi untuk menyimpan score
    public void SaveScore(int level, int score)
    {
        PlayerPrefs.SetInt("Score_Level" + level, score);
        PlayerPrefs.Save();

        LoadScores();
        UpdateUI();
    }

    // Ambil ulang semua score dari PlayerPrefs
    private void LoadScores()
    {
        scoreLevel1 = PlayerPrefs.GetInt("Score_Level1", 0);
        scoreLevel2 = PlayerPrefs.GetInt("Score_Level2", 0);
        scoreLevel3 = PlayerPrefs.GetInt("Score_Level3", 0);
    }

    // Update UI (Slider + Text)
    private void UpdateUI()
    {
        // Slider progress (0–1 dari maxScore)
        if (sliderLevel1 != null)
            sliderLevel1.value = (float)scoreLevel1 / maxScoreLevel1;

        if (sliderLevel2 != null)
            sliderLevel2.value = (float)scoreLevel2 / maxScoreLevel2;

        if (sliderLevel3 != null)
            sliderLevel3.value = (float)scoreLevel3 / maxScoreLevel3;

        // Display angka score yang tersimpan
        if (textLevel1 != null)
            textLevel1.text = scoreLevel1.ToString();

        if (textLevel2 != null)
            textLevel2.text = scoreLevel2.ToString();

        if (textLevel3 != null)
            textLevel3.text = scoreLevel3.ToString();
    }

    // Fungsi untuk ambil score spesifik
    public int GetScore(int level)
    {
        return PlayerPrefs.GetInt("Score_Level" + level, 0);
    }
}



