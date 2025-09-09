using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSelectButtonOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);
    }
}
