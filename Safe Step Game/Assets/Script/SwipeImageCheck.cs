using UnityEngine;
using UnityEngine.UI;

public class SwipeImageCheck : MonoBehaviour
{
    [Header("Setup MiniGame")]
    public int levelIndex = 1;        // Level ke berapa
    public int miniGameIndex = 1;     // Mini game ke berapa

    [Header("UI Button")]
    public Button targetButton;       // Button yang akan diganti image-nya
    public Sprite lockedSprite;       // Gambar awal (locked)
    public Sprite unlockedSprite;     // Gambar setelah selesai (unlocked)

    private string miniGameKey;       // Key untuk cek status mini game
    private string unlockKey;         // Key khusus untuk menyimpan status tombol

    void Start()
    {
        miniGameKey = $"MiniGameCompleted_Level{levelIndex}_{miniGameIndex}";
        unlockKey = $"MiniGameButton_Level{levelIndex}_{miniGameIndex}";

        CheckAndUpdateButton();
    }

    /// <summary>
    /// Mengecek apakah mini game sudah selesai, lalu update button.
    /// </summary>
    public void CheckAndUpdateButton()
    {
        bool completed = PlayerPrefs.GetInt(miniGameKey, 0) == 1;

        if (completed)
        {
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();
            UpdateButtonState(true);
        }
        else
        {
            if (PlayerPrefs.GetInt(unlockKey, 0) == 1)
                UpdateButtonState(true);
            else
                UpdateButtonState(false);
        }
    }

    /// <summary>
    /// Update sprite dan interaksi button.
    /// </summary>
    private void UpdateButtonState(bool unlocked)
    {
        if (targetButton != null)
        {
            Image btnImage = targetButton.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = unlocked ? unlockedSprite : lockedSprite;
            }

            targetButton.interactable = unlocked;
        }
    }

    /// <summary>
    /// Reset semua progress (dipanggil setelah DeleteAll).
    /// </summary>
    public void ResetButtonState()
    {
        UpdateButtonState(false);
    }
}

