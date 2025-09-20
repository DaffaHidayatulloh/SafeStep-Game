using UnityEngine;
using UnityEngine.UI;

public class PuzzleCheck : MonoBehaviour

{
    [Header("Setup")]
    public int levelIndex = 1;                // Level ke berapa
    public int totalMiniGames = 2;            // Jumlah mini game (misalnya Puzzle Pria + Puzzle Wanita)

    [Header("UI Button")]
    public Button miniGameButton;             // Button yang akan diganti gambarnya
    public Sprite lockedSprite;               // Gambar button terkunci
    public Sprite unlockedSprite;             // Gambar button terbuka

    private string unlockKey;                 // Key untuk PlayerPrefs

    void Start()
    {
        unlockKey = $"Level{levelIndex}_MiniGamesUnlocked";

        CheckMiniGameProgress();
    }

    public void CheckMiniGameProgress()
    {
        bool allCompleted = true;

        // cek semua mini game sudah selesai?
        for (int i = 1; i <= totalMiniGames; i++)
        {
            string completionKey = $"Puzzle_Level{levelIndex}_Mini{i}_Completed";
            if (PlayerPrefs.GetInt(completionKey, 0) == 0)
            {
                allCompleted = false;
                break;
            }
        }

        // kalau semua sudah selesai
        if (allCompleted)
        {
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();
            UpdateButtonState(true);
        }
        else
        {
            // kalau belum, cek apakah pernah tersimpan
            if (PlayerPrefs.GetInt(unlockKey, 0) == 1)
                UpdateButtonState(true);
            else
                UpdateButtonState(false);
        }
    }

    private void UpdateButtonState(bool unlocked)
    {
        if (miniGameButton != null)
        {
            Image btnImage = miniGameButton.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = unlocked ? unlockedSprite : lockedSprite;
            }

            miniGameButton.interactable = unlocked;
        }
    }

    // dipanggil saat delete all prefs reset button
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        UpdateButtonState(false);
    }
}
