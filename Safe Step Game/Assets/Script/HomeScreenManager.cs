using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class HomeScreenManager : MonoBehaviour
{ 
    [Header("Panels")]
    public GameObject Home;
    public GameObject Progres;
    public GameObject profile;

    [Header("UI Texts")]
    public Text welcomeText;      // teks sambutan
    public Text profileNameText;  // teks nama di profil

    private string fileName = "playerData.json";

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }

    void Start()
    {
        LoadPlayerName();
    }

    void LoadPlayerName()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            if (data != null && !string.IsNullOrEmpty(data.playerName))
            {
                if (welcomeText != null)
                    welcomeText.text = "Halo, " + data.playerName + "!";

                if (profileNameText != null)
                    profileNameText.text = "Halo, " + data.playerName;
            }
        }
        else
        {
            Debug.LogWarning("Player data file not found: " + filePath);
        }
    }

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

    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level 1 Reproduksi");
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level 2 Anti Kekerasan");
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene("Level 3 Mental Health");
    }

    public void GoToSertif()
    {
        SceneManager.LoadScene("Badge & Sertifikat");
    }

}
