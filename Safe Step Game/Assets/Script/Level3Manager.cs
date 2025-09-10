using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public GameObject OpeningMinigame1;
    public GameObject Minigame1;
    ///public GameObject RewardMinigame1;
   
    public void OnSelectOpening()
    {
        OpeningMinigame1.SetActive(false);
        Minigame1.SetActive(true);
    }
   
}
