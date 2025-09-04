using UnityEngine;

public class HomeScreenManager : MonoBehaviour
{
    public GameObject Home;
    public GameObject Progres;
    public GameObject profile;
    
    public void OnSelectHome()
    {
        Home.SetActive(true);
        Progres.SetActive(false);
        profile.SetActive(false);
    }
    public void OnSelectProgress()
    {
        Progres.SetActive(true);
        Home.SetActive(false);
        profile.SetActive(false);
    }

    public void OnSelecProfile()
    {
        profile.SetActive(true);
        Home.SetActive(false);
        Progres.SetActive(false);
    }
}
