using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public GameObject Puzzlepria;
    public GameObject OpeningScreen;

    public GameObject PuzzlePriaSelesai;
    public GameObject Puzzlewanita;

    public GameObject PuzzleWanitaSelesai;
    public GameObject Reward;

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

}
