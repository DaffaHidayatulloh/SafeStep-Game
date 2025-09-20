using UnityEngine;
using UnityEngine.UI;

public class BreathingButtonManager : MonoBehaviour
{
    [Header("Target Button")]
    public Image buttonImage;              // image di button
    public Sprite defaultSprite;           // sprite default
    public Sprite completedSprite;         // sprite kalau minigame selesai

    [Header("Minigame Reference")]
    public BreathingRelaxation breathingManager; // drag BreathingRelaxation di sini

    [Header("Save System")]
    public string saveKey = "BreathingCompleted"; // key untuk PlayerPrefs

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
        if (breathingManager != null)
            breathingManager.OnBreathingCompleted += HandleBreathingCompleted;
    }

    private void OnDisable()
    {
        if (breathingManager != null)
            breathingManager.OnBreathingCompleted -= HandleBreathingCompleted;
    }

    private void HandleBreathingCompleted()
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

