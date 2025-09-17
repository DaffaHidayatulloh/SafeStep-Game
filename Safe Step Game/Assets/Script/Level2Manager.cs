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
        // Default: hanya MiniGame1 yang aktif
        btnMinigame1.interactable = true;
        btnMinigame2.interactable = false;
        btnMinigame3.interactable = false;
    }

    // ------------------ Flow MiniGame 1 ------------------
    public void OnSelectButtonOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);

        // Setelah Minigame1 dimulai unlock Minigame2
        minigame1Unlocked = true;
        btnMinigame2.interactable = true;
    }

    public void OnSelectRewardGame()
    {
        RewardMinigame1.SetActive(false);
        OpeningMinigame2.SetActive(true);
    }

    // ------------------ Flow MiniGame 2 ------------------
    public void OnSelectOpening2()
    {
        OpeningMinigame2.SetActive(false);
        Minigame2.SetActive(true);

        // Setelah Minigame2 dimulai  unlock Minigame3
        minigame2Unlocked = true;
        btnMinigame3.interactable = true;
    }

    public void OnSelectRewardGame2()
    {
        RewardMinigame2.SetActive(false);
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
        SceneManager.LoadScene("Level 3 Mental Health");
    }

    // ------------------ Peta Level ------------------
    public void GoToMinigame1()
    {
        OpeningMinigame1.SetActive(true);
        PetaLevel.SetActive(false);
    }

    public void GoToMinigame2()
    {
        if (minigame1Unlocked) // hanya bisa kalau minigame1 sudah pernah dibuka
        {
            OpeningMinigame2.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }

    public void GoToMinigame3()
    {
        if (minigame2Unlocked) // hanya bisa kalau minigame2 sudah pernah dibuka
        {
            OpeningMinigame3.SetActive(true);
            PetaLevel.SetActive(false);
        }
    }
}

