using UnityEngine;

public class Level1Manager : MonoBehaviour
{
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
    public void OnSelectButtonOpening()
    {
        OpeningScreen.SetActive(false);
        Puzzlepria.SetActive(true);
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
    }
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
    public void OnSelectButtonLanjutMinigame3()
    {
        OpeningMinigame3.SetActive(false);
        MiniGame3.SetActive(true);

    }
    
}
