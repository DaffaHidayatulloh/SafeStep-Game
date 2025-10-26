using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;

using UnityEngine.SceneManagement;

public class UI_Events : MonoBehaviour
{


    // [Header("Panels")]
    // public GameObject Home;
    // public GameObject Progres;
    // public GameObject Profile;

    // DataWrapper dataWrapper;

    public Camera CertificateCamera;
    
    private int scoreLevel1;
    private int scoreLevel2;
    private int scoreLevel3;


    [Header("GameCardsTextures")] // as array
    public Texture[] gameCardsTextures;
    

    private UIDocument _document;
    // private Button _button;
    private List<VisualElement> _navButtons;


    private Button _popButtonYes;
    private Button _popButtonNo;


    private Button _gameCardButton;

    private int _currentGameCardIndex = 0;
    private VisualElement _gameCardImage;

    private VisualElement _popup;

    private VisualElement _homeMenu;
    private VisualElement _progressMenu;
    private VisualElement _profileMenu;


    private Button _gameSelectLeftArrow;
    private Button _gameSelectRightArrow;


    private ProgressBar _level1Progression;
    private ProgressBar _level2Progression;
    private ProgressBar _level3Progression;

    private ProgressBar _totalProgression;

    private VisualElement _playerAvatar;


    private Button _profileEditNameButton;
    private Button _profileEditAvatarButton;

    private Button _profileExportDataButton;
    private Button _logoutButton;

    private Button _reseteDataButton;

    private Button _claimCertificateButton;


    private TextField _profileNameTextField;

    private Button _profileNameSaveButton;


    private Button _avatarMaleButton;
    private Button _avatarFemaleButton;

    private Button _avatarSaveButton;

    private VisualElement _avatarSelectionPopupMenu;

    private VisualElement _nameChangePopupMenu;



    private VisualElement _resetDataPopupMenu;
    private Button _resetDataConfirmButton;


    private int _selectedAvatarIndex = 0;

    private string fileName = "playerData.json";
    private Label _playerNameLabel;

    public Texture[] playerAvatars;
    private VisualElement _sertifStuff;

    private string savePath;




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

    void Start()
    {

        savePath = Application.persistentDataPath + "/character.json";
        // dataWrapper = GetComponent<DataWrapper>();

    }

    private void TempResetClass()
    {
        Debug.Log("Resetting nav button classes");
        for (int i = 0; i < _navButtons.Count; i++)
        {
            _navButtons[i].RemoveFromClassList("navisSelected");
        }
    }
    public void OnSelectHome(ClickEvent evt)
    {

        TempResetClass();
        _navButtons[0].AddToClassList("navisSelected");

        _homeMenu.AddToClassList("common-show");
        _progressMenu.RemoveFromClassList("common-show");
        _profileMenu.RemoveFromClassList("common-show");

    }

    public void OnSelectProgress(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[1].AddToClassList("navisSelected");


        LoadScores();


                
        _level1Progression.value = scoreLevel1;
        _level2Progression.value = scoreLevel2;
        _level3Progression.value = scoreLevel3;

        _totalProgression.value = scoreLevel1 + scoreLevel2 + scoreLevel3;

        Debug.Log("Level 1 Progression: " + _level1Progression.value);
        Debug.Log("Level 2 Progression: " + _level2Progression.value);
        Debug.Log("Level 3 Progression: " + _level3Progression.value);
        Debug.Log("Total Progression: " + _totalProgression.value);


        if (_totalProgression.value >= 1050)
        {
            // _totalProgression.value = 300;

            // set certificate camera to black and white
            
            _claimCertificateButton.AddToClassList("button-blue");
            _claimCertificateButton.SetEnabled(true);
            _sertifStuff.RemoveFromClassList("sertif-disabled");
        } else
        {
            _claimCertificateButton.RemoveFromClassList("button-blue");
            _claimCertificateButton.SetEnabled(false);
            

            // also set the certific
        }


        _progressMenu.AddToClassList("common-show");
        _homeMenu.RemoveFromClassList("common-show");
        _profileMenu.RemoveFromClassList("common-show");
    }

    public void OnSelectProfile(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[2].AddToClassList("navisSelected");

        _profileMenu.AddToClassList("common-show");
        _homeMenu.RemoveFromClassList("common-show");
        _progressMenu.RemoveFromClassList("common-show");
    }

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        // _button = _document.rootVisualElement.Q("nav-home") as Button;
        // _button.RegisterCallback<ClickEvent>(OnSetHome);

        _navButtons = _document.rootVisualElement.Query(className: "nav-button").ToList();
        _navButtons[0].RegisterCallback<ClickEvent>(OnSelectHome);
        _navButtons[1].RegisterCallback<ClickEvent>(OnSelectProgress);
        _navButtons[2].RegisterCallback<ClickEvent>(OnSelectProfile);


        _popup = _document.rootVisualElement.Q("pop-up-overlay");

        _popButtonYes = _document.rootVisualElement.Q<Button>("pop-yes") as Button;
        _popButtonNo = _document.rootVisualElement.Q<Button>("pop-no") as Button;

        //change popup button no string text to "no"
        _popButtonNo.text = "No";

        _popButtonYes.clicked += () =>
        {
            Debug.Log("YES");
            // _popup.style.display = DisplayStyle.None;
        };
        _popButtonNo.clicked += () =>
        {
            Debug.Log("NO");
            // _popup.style.display = DisplayStyle.None;

            HidePopup();
        };



        _gameCardButton = _document.rootVisualElement.Q<Button>("gamecard-button") as Button;

        _gameCardButton.RegisterCallback<ClickEvent>(GameCardStart);

        _homeMenu = _document.rootVisualElement.Q("home-menu");
        _progressMenu = _document.rootVisualElement.Q("progress-menu");
        _profileMenu = _document.rootVisualElement.Q("profile-menu");

        _gameSelectLeftArrow = _document.rootVisualElement.Q<Button>("gamepick-left") as Button;
        _gameSelectRightArrow = _document.rootVisualElement.Q<Button>("gamepick-right") as Button;


        _gameCardImage = _document.rootVisualElement.Q("gamecard") as VisualElement;

        _resetDataPopupMenu = _document.rootVisualElement.Q("reset-progress") as VisualElement;
        _resetDataConfirmButton = _document.rootVisualElement.Q<Button>("reset-button") as Button;

        _resetDataConfirmButton.clicked += () =>
        {
            Debug.Log("RESET DATA CONFIRMED");
            ResetProgressionData();
        };

        _gameSelectLeftArrow.clicked += () =>
        {
            Debug.Log("LEFT ARROW CLICKED");
            _currentGameCardIndex--;
            Debug.Log(_currentGameCardIndex);
            if (_currentGameCardIndex < 0)
            {
                _currentGameCardIndex = gameCardsTextures.Length - 1;
            }
            _gameCardImage.style.backgroundImage = new StyleBackground((Background)gameCardsTextures[_currentGameCardIndex]);

        };

        _gameSelectRightArrow.clicked += () =>
        {
            Debug.Log("RIGHT ARROW CLICKED");
            _currentGameCardIndex++;
            Debug.Log(_currentGameCardIndex);
            if (_currentGameCardIndex >= gameCardsTextures.Length)
            {
                _currentGameCardIndex = 0;
            }
            _gameCardImage.style.backgroundImage = new StyleBackground((Background)gameCardsTextures[_currentGameCardIndex]);
        };

        _gameCardButton.clicked += () =>
        {

        };



        _level1Progression = _document.rootVisualElement.Q<ProgressBar>("progress-lv1") as ProgressBar;
        _level2Progression = _document.rootVisualElement.Q<ProgressBar>("progress-lv2") as ProgressBar;
        _level3Progression = _document.rootVisualElement.Q<ProgressBar>("progress-lv3") as ProgressBar;
        _totalProgression = _document.rootVisualElement.Q<ProgressBar>("progress-total") as ProgressBar;

        _level1Progression.highValue = 350;
        _level1Progression.lowValue = 0;
        _level1Progression.value = 0;

        _level2Progression.highValue = 450;
        _level2Progression.lowValue = 0;
        _level2Progression.value = 0;

        _level3Progression.highValue = 300;
        _level3Progression.lowValue = 0;
        _level3Progression.value = 100;


        _totalProgression.highValue = 1050;
        _totalProgression.lowValue = 0;
        _totalProgression.value = 0;


        _playerNameLabel = _document.rootVisualElement.Q<Label>("player-name") as Label;

        LoadPlayerName();

        _playerAvatar = _document.rootVisualElement.Q("player-avatar") as VisualElement;

        LoadCharacter();


        _profileEditNameButton = _document.rootVisualElement.Q<Button>("profile-change-name") as Button;
        _profileEditAvatarButton = _document.rootVisualElement.Q<Button>("profile-change-avatar") as Button;
        _profileExportDataButton = _document.rootVisualElement.Q<Button>("profile-export-data") as Button;
        _logoutButton = _document.rootVisualElement.Q<Button>("profile-logout") as Button;
        _reseteDataButton = _document.rootVisualElement.Q<Button>("profile-reset-data") as Button;

        _claimCertificateButton = _document.rootVisualElement.Q<Button>("SertifClaimButton") as Button;

        _sertifStuff = _document.rootVisualElement.Q("SertifStuff") as VisualElement;

        _logoutButton.clicked += () =>
        {
            Debug.Log("LOGOUT BUTTON CLICKED");
            // clear all player prefs
            // _resetDataPopupMenu = _document.rootVisualElement.Q("reset-data-confirmation") as VisualElement;
            _resetDataPopupMenu.style.display = DisplayStyle.Flex;
            _nameChangePopupMenu.style.display = DisplayStyle.None;
            _avatarSelectionPopupMenu.style.display = DisplayStyle.None;
            _popup.RemoveFromClassList("hide-popup");


        };


        _claimCertificateButton.clicked += () =>
        {
            Debug.Log("CLAIM CERTIFICATE BUTTON CLICKED");
            ShowPopup();
            // GoToSertif();
            Home_CertificateDownloader certDownloader = FindFirstObjectByType<Home_CertificateDownloader>();
            if (certDownloader != null)
            {
                certDownloader.SaveCertificate();
            }
            else
            {
                Debug.LogError("CertificateDownloader component not found in the scene.");
            }
        };


        _profileNameTextField = _document.rootVisualElement.Q<TextField>("name-field") as TextField;
        _profileNameSaveButton = _document.rootVisualElement.Q<Button>("change-name-confirm") as Button;

        _avatarFemaleButton = _document.rootVisualElement.Q<Button>("avatar-button-female") as Button;
        _avatarMaleButton = _document.rootVisualElement.Q<Button>("avatar-button-male") as Button;
        _avatarSaveButton = _document.rootVisualElement.Q<Button>("avatar-change-confirm") as Button;


        // _profileEditAvatarButton.clicked += () =>
        // {
        //     Debug.Log("EDIT AVATAR BUTTON CLICKED");
        //     // show avatar selection popup
        //     _document.rootVisualElement.Q("pop-up-overlay").RemoveFromClassList("hide-popup");
        //     // var changename = _document.rootVisualElement.Q("change-name");
        //     // changename.SetEnabled(true);

        //     var changeavatar = _document.rootVisualElement.Q("change-avatar");
        //     changeavatar.SetEnabled(true);
        //     _popup.RemoveFromClassList("hide-popup");
        // };

        _avatarSelectionPopupMenu = _document.rootVisualElement.Q("avatar-change") as VisualElement;
        _nameChangePopupMenu = _document.rootVisualElement.Q("change-name") as VisualElement;

        _profileEditNameButton.clicked += () =>
        {
            Debug.Log("EDIT NAME BUTTON CLICKED");
            // show name change popup
            _document.rootVisualElement.Q("pop-up-overlay").RemoveFromClassList("hide-popup");
            // var changeavatar = _document.rootVisualElement.Q("change-avatar");
            // changeavatar.SetEnabled(true);

            // var changename = _document.rootVisualElement.Q("change-name");
            // changename.SetEnabled(true);

            // _avatarSelectionPopupMenu.AddToClassList("hide-popup");
            // _nameChangePopupMenu.RemoveFromClassList("hide-popup");

            _profileNameTextField.value = _playerNameLabel.text;
            _avatarSelectionPopupMenu.style.display = DisplayStyle.None;
            _nameChangePopupMenu.style.display = DisplayStyle.Flex;
            _popup.RemoveFromClassList("hide-popup");
        };

        _profileEditAvatarButton.clicked += () =>
        {
            Debug.Log("EDIT AVATAR BUTTON CLICKED");
            // show avatar selection popup
            _document.rootVisualElement.Q("pop-up-overlay").RemoveFromClassList("hide-popup");
            // var changename = _document.rootVisualElement.Q("change-name");
            // changename.SetEnabled(true);

            // var changeavatar = _document.rootVisualElement.Q("avatar-change");
            // changeavatar.SetEnabled(true);
            _nameChangePopupMenu.style.display = DisplayStyle.None;
            _avatarSelectionPopupMenu.style.display = DisplayStyle.Flex;

            _popup.RemoveFromClassList("hide-popup");


            if (_playerAvatar.style.backgroundImage.value.texture == playerAvatars[0])
            {
                _avatarMaleButton.AddToClassList("avatar-select-selected");
                _avatarFemaleButton.RemoveFromClassList("avatar-select-selected");
                _selectedAvatarIndex = 0;
            }
            else if (_playerAvatar.style.backgroundImage.value.texture == playerAvatars[1])
            {
                _avatarFemaleButton.AddToClassList("avatar-select-selected");
                _avatarMaleButton.RemoveFromClassList("avatar-select-selected");
                _selectedAvatarIndex = 1;
            }
        };



        _avatarFemaleButton.clicked += () =>
        {
            Debug.Log("FEMALE AVATAR SELECTED");
            // _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[1]);
            _avatarFemaleButton.AddToClassList("avatar-select-selected");
            _avatarMaleButton.RemoveFromClassList("avatar-select-selected");
            _selectedAvatarIndex = 1;
        };

        _avatarMaleButton.clicked += () =>
        {
            Debug.Log("MALE AVATAR SELECTED");
            // _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[0]);
            _avatarMaleButton.AddToClassList("avatar-select-selected");
            _avatarFemaleButton.RemoveFromClassList("avatar-select-selected");

            _selectedAvatarIndex = 0;
        };


        _avatarSaveButton.clicked += () =>
        {
            Debug.Log("SAVE AVATAR BUTTON CLICKED");
            SaveAvatar();
            _popup.AddToClassList("hide-popup");
        };

        _profileNameSaveButton.clicked += () =>
        {
            Debug.Log("SAVE NAME BUTTON CLICKED");
            ChangeName();
            _popup.AddToClassList("hide-popup");
        };


    }
    

    private void ResetProgressionData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All player progress data has been reset.");
        PlayerPrefs.DeleteAll();
        // delete character.json file
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Character data file deleted: " + savePath);
        }
        else
        {
            Debug.LogWarning("Character data file not found for deletion: " + savePath);
        }
        // go to main menu scene
        SceneManager.LoadScene("LoginRegister");
    }


    private void SaveAvatar()
    {
        Debug.Log("Saving selected avatar index: " + _selectedAvatarIndex);

        string characterName = _selectedAvatarIndex == 0 ? "Male" : "Female";
        // Save to JSON file
        CharacterData data = new CharacterData();
        data.characterName = characterName;
        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, "character.json");
        File.WriteAllText(filePath, json);
        Debug.Log("Character data saved to: " + filePath);

        _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[_selectedAvatarIndex]);
    }
    

    private void ChangeName()
    {
        string newName = _profileNameTextField.value;
        Debug.Log("New name to save: " + newName);

        if (!string.IsNullOrEmpty(newName))
        {
            // Save to JSON file
            PlayerData data = new PlayerData();
            data.playerName = newName;

            string json = JsonUtility.ToJson(data);
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(filePath, json);

            Debug.Log("Player name saved to: " + filePath);

            // Update UI
            _playerNameLabel.text = newName;
        }
        else
        {
            Debug.LogWarning("New name is empty. Name not changed.");
        }
    }



        private void LoadCharacter()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "character.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);

            if (data != null && !string.IsNullOrEmpty(data.characterName))
            {
                Debug.Log("Character loaded: " + data.characterName);

                if (_playerAvatar != null && playerAvatars != null && playerAvatars.Length >= 2)
                {
                    // Pilih avatar berdasarkan nama karakter
                    if (data.characterName == "Male")
                    {
                        _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[0]);
                    }
                    else if (data.characterName == "Female")
                    {
                        _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[1]);
                    }
                    else
                    {
                        Debug.LogWarning("Unknown character name. Defaulting to Male avatar.");
                        _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[0]);
                    }
                }
                else
                {
                    Debug.LogWarning("Player avatar or avatar list not properly assigned.");
                }
            }
            else
            {
                Debug.LogWarning("Character data invalid or missing characterName.");
            }
        }
        else
        {
            Debug.LogWarning("Character data file not found: " + filePath);
        }
    }


    private void LoadScores()
    {
        scoreLevel1 = PlayerPrefs.GetInt("Score_Level1", 0);
        scoreLevel2 = PlayerPrefs.GetInt("Score_Level2", 0);
        scoreLevel3 = PlayerPrefs.GetInt("Score_Level3", 0);
    }


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
        void LoadPlayerName()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        // string playerName = DataWrapper.ReadLocalStorage("playerName");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            if (data != null && !string.IsNullOrEmpty(data.playerName))
            {

                if (_playerNameLabel != null)
                    _playerNameLabel.text = data.playerName;
                    // _playerNameLabel.text = playerName;
                else
                {
                    Debug.LogWarning("Player Name Label is null, using fallback as androgynous name");
                    _playerNameLabel.text = "Player";
                }

                // if (profileNameText != null)
                //     profileNameText.text = "Halo, " + data.playerName;
            }
        }
        else
        {
            Debug.LogWarning("Player data file not found: " + filePath);
        }
    }


    private void GameCardStart(ClickEvent evt)
    {
        Debug.Log("GAME CARD CLICKED");
        // _popup.style.display = DisplayStyle.Flex;

        // remove hide class from popup
        _popup.RemoveFromClassList("hide-popup");

        // _popup.transform.scale = Vector3.one; 

        // _popup.style.display = DisplayStyle.Flex;

        // do show map later below


        // for a debug reason let's add when game card is clicked, it wil add score to lv 1 or just according to the current game card index
        if (_currentGameCardIndex == 0)
        { GoToLevel1(); }
        else if (_currentGameCardIndex == 1)
        { GoToLevel2(); }
        else if (_currentGameCardIndex == 2)
        { GoToLevel3(); }




        Debug.Log("Scores: " + scoreLevel1 + ", " + scoreLevel2 + ", " + scoreLevel3);


    }




    private void HidePopup()
    {
        _popup.style.display = DisplayStyle.None;

        // _popup.transform.scale = Vector3.zero; 

        // assign hide class to popup
        _popup.AddToClassList("hide-popup");

    }
    
    private void ShowPopup()
    {
        // _popup.style.display = DisplayStyle.Flex;

        // remove hide class from popup
        _popup.RemoveFromClassList("hide-popup");

        // _popup.transform.scale = Vector3.one; 

    }

    private void OnDisable()
    {
        // _button.UnregisterCallback<ClickEvent>(OnSetHome);
        _navButtons[0].UnregisterCallback<ClickEvent>(OnSelectHome);
        _navButtons[1].UnregisterCallback<ClickEvent>(OnSelectProgress);
        _navButtons[2].UnregisterCallback<ClickEvent>(OnSelectProfile);
    }

    // private void OnSetHome(ClickEvent evt)
    // {
    //     Debug.Log("PRESSED HOME");
    //     Debug.Log(_document.rootVisualElement.Query("Button"));

    //     // OnSelectHome();

    //     if (_button.ClassListContains("navisSelected"))
    //     {
    //         _button.RemoveFromClassList("navisSelected");
    //         _navButtons[1].RemoveFromClassList("navisSelected");
    //         Debug.Log(_button.name);
    //     }
    //     else
    //     {
    //         _button.AddToClassList("navisSelected");
    //         _navButtons[1].AddToClassList("navisSelected");
    //     }
    // }


}
