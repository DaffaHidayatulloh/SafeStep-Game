using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3Manager : MonoBehaviour
{
    [Header("MiniGame 1")]
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;

    [Header("MiniGame 2")]
    public GameObject OpeningMinigame2;
    public GameObject Minigame2;
    public GameObject RewardMinigame2;

    [Header("MiniGame 3")]
    public GameObject OpeningMinigame3;
    public GameObject Minigame3;
    public GameObject RewardMinigame3;

    [Header("Peta Level")]
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
        minigame1Unlocked = PlayerPrefs.GetInt("L3_MiniGame1Unlocked", 0) == 1;
        minigame2Unlocked = PlayerPrefs.GetInt("L3_MiniGame2Unlocked", 0) == 1;

        UpdateButtonStates();
    }

    // ------------------ Flow MiniGame 1 ------------------
    public void OnSelectOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);

        // Unlocked MiniGame1
        minigame1Unlocked = true;
        PlayerPrefs.SetInt("L3_MiniGame1Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    public void OnSelectReward()
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

        // Unlocked MiniGame2
        minigame2Unlocked = true;
        PlayerPrefs.SetInt("L3_MiniGame2Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    public void OnSelectReward2()
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
        SceneManager.LoadScene("Home Screen");
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
        PlayerPrefs.DeleteKey("L3_MiniGame1Unlocked");
        PlayerPrefs.DeleteKey("L3_MiniGame2Unlocked");
        PlayerPrefs.Save();

        minigame1Unlocked = false;
        minigame2Unlocked = false;

        UpdateButtonStates();

        Debug.Log("Progress Level 3 sudah di-reset.");
    }

    // ------------------ Update Button States ------------------
    private void UpdateButtonStates()
    {
        // MiniGame1 selalu aktif
        SetButtonState(btnMinigame1, true);

        // MiniGame2 aktif kalau MiniGame1 unlocked
        SetButtonState(btnMinigame2, minigame1Unlocked);

        // MiniGame3 aktif kalau MiniGame2 unlocked
        SetButtonState(btnMinigame3, minigame2Unlocked);
    }

    private void SetButtonState(Button btn, bool isUnlocked)
    {
        btn.interactable = isUnlocked;

        ColorBlock cb = btn.colors;
        if (isUnlocked)
        {
            cb.normalColor = Color.white;   // warna normal kalau kebuka
            cb.disabledColor = Color.white;
        }
        else
        {
            cb.normalColor = Color.gray;    // abu-abu kalau terkunci
            cb.disabledColor = Color.gray;
        }
        btn.colors = cb;
    }
    public void Home()
    {
        SceneManager.LoadScene("Home Screen");
    }
    public void ButtonBack()
    {
        SceneManager.LoadScene("Level 3 Mental Health");
     
    }
}
