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

    [Header("Buttons di Peta Level")]
    public Button btnMinigame1;
    public Button btnMinigame2;
    public Button btnMinigame3;

    [Header("Progress Flags")]
    private bool minigame1Unlocked = false;
    private bool minigame2Unlocked = false;

    private void Start()
    {
        // Default: hanya Minigame1 yang bisa diakses
        btnMinigame1.interactable = true;
        btnMinigame2.interactable = false;
        btnMinigame3.interactable = false;
    }

    // ------------------ Flow MiniGame 1 ------------------
    public void OnSelectButtonOpening()
    {
        OpeningScreen.SetActive(false);
        Puzzlepria.SetActive(true);

        // Minigame 1 terbuka
        minigame1Unlocked = true;
        btnMinigame2.interactable = true; // aktifkan tombol Minigame2 di peta
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

        // Minigame 2 terbuka
        minigame2Unlocked = true;
        btnMinigame3.interactable = true; // aktifkan tombol Minigame3 di peta
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
}


