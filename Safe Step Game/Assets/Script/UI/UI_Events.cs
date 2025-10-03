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
    

    private string fileName = "playerData.json";
    private Label _playerNameLabel;

    public Texture[] playerAvatars;

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

    }

    private void TempResetClass()
    {
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
        
        _level1Progression.value = (float)scoreLevel1 / 100;
        _level2Progression.value = (float)scoreLevel2 / 100;
        _level3Progression.value = (float)scoreLevel3 / 100;

        _totalProgression.value = scoreLevel1 + scoreLevel2 + scoreLevel3;


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

        _level1Progression.highValue = 100;
        _level1Progression.lowValue = 0;
        _level1Progression.value = 0;

        _level2Progression.highValue = 100;
        _level2Progression.lowValue = 0;
        _level2Progression.value = 0;

        _level3Progression.highValue = 100;
        _level3Progression.lowValue = 0;
        _level3Progression.value = 100;


        _totalProgression.highValue = 300;
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
        

    }



        private void LoadCharacter()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);

            // Sprite selectedSprite = null;

            // _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[0]);

            if (data.characterName == "Male")
            {
                _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[0]);
            }
            else if (data.characterName == "Female")
            {
                _playerAvatar.style.backgroundImage = new StyleBackground((Background)playerAvatars[1]);
            }

            // Set ke dua tempat (main menu dan profil)
            // if (characterImageMainMenu != null)
            //     characterImageMainMenu.sprite = selectedSprite;

            // if (characterImageProfile != null)
            //     characterImageProfile.sprite = selectedSprite;
        }
        else
        {
            Debug.Log("No character selected yet.");
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

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            if (data != null && !string.IsNullOrEmpty(data.playerName))
            {
                if (_playerNameLabel != null)
                    _playerNameLabel.text = data.playerName;
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
