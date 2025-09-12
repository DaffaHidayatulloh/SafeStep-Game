using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;

    public GameObject OpeningMinigame2;
    public GameObject Minigame2;
    public GameObject RewardMinigame2;

    public GameObject OpeningMinigame3;
    public GameObject Minigame3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSelectButtonOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);
    }

    public void OnSelectRewardGame()
    {
        RewardMinigame1.SetActive(false);
        OpeningMinigame2.SetActive(true);
    }
    public void OnSelectOpening2()
    {
        OpeningMinigame2.SetActive(false);
        Minigame2.SetActive(true);
    }
    public void OnSelectRewardGame2() 
    {
        RewardMinigame2.SetActive(false);
        OpeningMinigame3.SetActive(true);
    }
    public void OnSelectOpening3() 
    {
        OpeningMinigame3.SetActive(false);
        Minigame3.SetActive(true);
    }
    public void OnSelectReward3()
    {
        SceneManager.LoadScene("Level 3 Mental Health");
    }
}
