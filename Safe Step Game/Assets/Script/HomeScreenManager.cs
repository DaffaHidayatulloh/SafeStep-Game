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

    public GameObject panelEditnama;
    public GameObject panelEditavatar;

    private string fileName = "playerData.json";

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
    }

    public Image characterImageMainMenu;   // untuk main menu
    public Image characterImageProfile;    // untuk profil
    public Sprite maleSprite;
    public Sprite femaleSprite;

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/character.json";
        LoadCharacter();
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

    private void LoadCharacter()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);

            Sprite selectedSprite = null;

            if (data.characterName == "Male")
            {
                selectedSprite = maleSprite;
            }
            else if (data.characterName == "Female")
            {
                selectedSprite = femaleSprite;
            }

            // Set ke dua tempat (main menu dan profil)
            if (characterImageMainMenu != null)
                characterImageMainMenu.sprite = selectedSprite;

            if (characterImageProfile != null)
                characterImageProfile.sprite = selectedSprite;
        }
        else
        {
            Debug.Log("No character selected yet.");
        }
    }
    public void OnSeleceditnama()
    {
       panelEditnama.SetActive(true);
    }
    public void OnCloseEditnama()
    {
        panelEditnama.SetActive(false);
    }
    public void OnSeleceditavatar()
    {
        panelEditavatar.SetActive(true);
    }
    public void OnCloseEditavatar()
    {
        panelEditavatar.SetActive(false);
    }

}
