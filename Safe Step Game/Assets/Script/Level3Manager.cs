using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    public GameObject RewardMinigame1;

    public GameObject OpeningMinigame2;
    public GameObject Minigame2;
    public GameObject RewardMinigame2;

    public GameObject OpeningMinigame3;
    public GameObject Minigame3;
    public GameObject RewardMinigame3;
   
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
    public void OnSelectReward2()
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
        SceneManager.LoadScene("Home Screen");
    }

}
