using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level2Manager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;

    public GameObject OpeningMinigame2;
    public GameObject Minigame2;
    public GameObject RewardMinigame2;

    public GameObject OpeningMinigame3;
    public GameObject Minigame3;

    public GameObject PetaLevel;

    [Header("Buttons di Peta Level")]
    public Button btnMinigame1;
    public Button btnMinigame2;
    public Button btnMinigame3;

    [Header("Progress Flags")]
    private bool minigame1Unlocked = false;
    private bool minigame2Unlocked = false;

    private void Start()
    {
        // Load progress dari PlayerPrefs
        minigame1Unlocked = PlayerPrefs.GetInt("Level2_MiniGame1Unlocked", 0) == 1;
        minigame2Unlocked = PlayerPrefs.GetInt("Level2_MiniGame2Unlocked", 0) == 1;

        UpdateButtonStates();
    }

    // ------------------ Flow MiniGame 1 ------------------
    public void OnSelectButtonOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);

        // Setelah Minigame1 dimulai unlock Minigame2
        minigame1Unlocked = true;
        PlayerPrefs.SetInt("Level2_MiniGame1Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    public void OnSelectRewardGame()
    {
        RewardMinigame1.SetActive(false);
        AudioManager.instance.StopAllSFX();
        AudioManager.instance.PlaySFX(4);
        OpeningMinigame2.SetActive(true);
    }

    // ------------------ Flow MiniGame 2 ------------------
    public void OnSelectOpening2()
    {
        OpeningMinigame2.SetActive(false);
        Minigame2.SetActive(true);

        // Setelah Minigame2 dimulai unlock Minigame3
        minigame2Unlocked = true;
        PlayerPrefs.SetInt("Level2_MiniGame2Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    public void OnSelectRewardGame2()
    {
        RewardMinigame2.SetActive(false);
        AudioManager.instance.StopAllSFX();
        AudioManager.instance.PlaySFX(4);
        OpeningMinigame3.SetActive(true);
    }

    // ------------------ Flow MiniGame 3 ------------------
    public void OnSelectOpening3()
    {
        OpeningMinigame3.SetActive(false);
        Minigame3.SetActive(true);
    }

    public void OnSelectReward3()
    {
        AudioManager.instance.StopAllSFX();
        SceneManager.LoadScene("Level 3 Mental Health");
    }

    // ------------------ Peta Level ------------------
    public void GoToMinigame1()
    {
        AudioManager.instance.PlaySFX(4);
        OpeningMinigame1.SetActive(true);
        PetaLevel.SetActive(false);
    }

    public void GoToMinigame2()
    {
        if (minigame1Unlocked)
        {
            AudioManager.instance.PlaySFX(4);
            OpeningMinigame2.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }

    public void GoToMinigame3()
    {
        if (minigame2Unlocked)
        {
            AudioManager.instance.PlaySFX(4);
            OpeningMinigame3.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }

    // ------------------ Reset Progress ------------------
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("Level2_MiniGame1Unlocked");
        PlayerPrefs.DeleteKey("Level2_MiniGame2Unlocked");
        PlayerPrefs.Save();

        minigame1Unlocked = false;
        minigame2Unlocked = false;

        UpdateButtonStates();

        Debug.Log("Progress Level 2 sudah di-reset. Semua minigame terkunci kembali.");
    }

    // ------------------ Update Button States ------------------
    private void UpdateButtonStates()
    {
        // Minigame 1 selalu terbuka
        SetButtonState(btnMinigame1, true);

        // Minigame 2 hanya aktif jika Minigame 1 terbuka
        SetButtonState(btnMinigame2, minigame1Unlocked);

        // Minigame 3 hanya aktif jika Minigame 2 terbuka
        SetButtonState(btnMinigame3, minigame2Unlocked);
    }

    private void SetButtonState(Button btn, bool isUnlocked)
    {
        btn.interactable = isUnlocked;

        ColorBlock cb = btn.colors;
        if (isUnlocked)
        {
            cb.normalColor = Color.white;
            cb.disabledColor = Color.white;
        }
        else
        {
            cb.normalColor = Color.gray;
            cb.disabledColor = Color.gray;
        }
        btn.colors = cb;
    }
    public void Home()
    {
        SceneManager.LoadScene("Home Screen");
    }
}
