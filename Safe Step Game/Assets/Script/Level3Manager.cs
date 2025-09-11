using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;

    public GameObject OpeningMinigame2;
    public GameObject Minigame2;
    public GameObject RewardMinigame2;
   
    public void OnSelectOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);
    }

    public void OnSelectReward()
    {
        RewardMinigame1.SetActive(false);
        OpeningMinigame2.SetActive(true);
    }
    public void OnSelectOpening2()
    {
        OpeningMinigame2.SetActive(false);
        Minigame2.SetActive(true);
    }

}
