using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject Puzzlepria;
    public GameObject OpeningScreen;

    public GameObject PuzzlePriaSelesai;
    public GameObject Puzzlewanita;

    public GameObject PuzzleWanitaSelesai;
    public GameObject Reward;

    public GameObject OpeningMinigame2;
    public GameObject MiniGame2;
    public GameObject RewardMinigame2;

    public GameObject OpeningMinigame3;
    public GameObject MiniGame3;

    public GameObject PetaLevel;

    [Header("UI Buttons")]
    public Button buttonMinigame1;
    public Button buttonMinigame2;
    public Button buttonMinigame3;

    [Header("Progress Flags")]
    private bool minigame1Unlocked = false;
    private bool minigame2Unlocked = false;

    private void Start()
    {
        // Load progress dari PlayerPrefs
        minigame1Unlocked = PlayerPrefs.GetInt("MiniGame1Unlocked", 0) == 1;
        minigame2Unlocked = PlayerPrefs.GetInt("MiniGame2Unlocked", 0) == 1;

        UpdateButtonStates();
    }

    // ------------------ Flow MiniGame 1 ------------------
    public void OnSelectButtonOpening()
    {
        OpeningScreen.SetActive(false);
        Puzzlepria.SetActive(true);

        // Menandai bahwa minigame 1 sudah dibuka
        minigame1Unlocked = true;
        PlayerPrefs.SetInt("MiniGame1Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    public void OnSelectButtonPriaLanjut()
    {
        PuzzlePriaSelesai.SetActive(false);
        Puzzlewanita.SetActive(true);
    }

    public void OnSelectButtonWanitaLanjut()
    {
        PuzzleWanitaSelesai.SetActive(false);
        Reward.SetActive(true);
    }

    public void OnSelectButtonLanjutOpeningMinigame2()
    {
        Reward.SetActive(false);
        OpeningMinigame2.SetActive(true);

        // Menandai bahwa minigame 2 sudah bisa dibuka
        minigame2Unlocked = true;
        PlayerPrefs.SetInt("MiniGame2Unlocked", 1);
        PlayerPrefs.Save();

        UpdateButtonStates();
    }

    // ------------------ Flow MiniGame 2 ------------------
    public void OnSelectButtonLanjutMinigame2()
    {
        OpeningMinigame2.SetActive(false);
        MiniGame2.SetActive(true);
    }

    public void OnSelectButtonOpening3()
    {
        RewardMinigame2.SetActive(false);
        OpeningMinigame3.SetActive(true);
    }

    // ------------------ Flow MiniGame 3 ------------------
    public void OnSelectButtonLanjutMinigame3()
    {
        OpeningMinigame3.SetActive(false);
        MiniGame3.SetActive(true);
    }

    public void OnSelectReward3()
    {
        SceneManager.LoadScene("Level 2 Anti Kekerasan");
    }

    // ------------------ Peta Level ------------------
    public void GoToMinigame1()
    {
        OpeningScreen.SetActive(true);
        PetaLevel.SetActive(false);
    }

    public void GoToMinigame2()
    {
        if (minigame1Unlocked)
        {
            OpeningMinigame2.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }

    public void GoToMinigame3()
    {
        if (minigame2Unlocked)
        {
            OpeningMinigame3.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }

    // ------------------ Reset Progress ------------------
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset flag ke false
        minigame1Unlocked = false;
        minigame2Unlocked = false;

        UpdateButtonStates();

        Debug.Log("Progress sudah di-reset. Semua minigame terkunci kembali.");
    }

    // ------------------ Update Button States ------------------
    private void UpdateButtonStates()
    {
        // Minigame 1 selalu terbuka
        SetButtonState(buttonMinigame1, true);

        // Minigame 2 hanya aktif jika Minigame 1 terbuka
        SetButtonState(buttonMinigame2, minigame1Unlocked);

        // Minigame 3 hanya aktif jika Minigame 2 terbuka
        SetButtonState(buttonMinigame3, minigame2Unlocked);
    }

    private void SetButtonState(Button btn, bool isUnlocked)
    {
        btn.interactable = isUnlocked;

        ColorBlock cb = btn.colors;
        if (isUnlocked)
        {
            cb.normalColor = Color.white;   // warna normal
            cb.disabledColor = Color.white; // biar tidak gelap kalau sudah terbuka
        }
        else
        {
            cb.normalColor = Color.gray;    // terkunci = abu-abu
            cb.disabledColor = Color.gray;  // saat disabled tetap abu-abu
        }
        btn.colors = cb;
    }
}
