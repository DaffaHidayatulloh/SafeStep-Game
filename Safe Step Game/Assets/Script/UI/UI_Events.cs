using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Events : MonoBehaviour
{


    // [Header("Panels")]
    // public GameObject Home;
    // public GameObject Progres;
    // public GameObject Profile;

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

        // Home.SetActive(true);
        // Progres.SetActive(false);
        // Profile.SetActive(false);

        _homeMenu.AddToClassList("common-show");
        _progressMenu.RemoveFromClassList("common-show");
        _progressMenu.RemoveFromClassList("common-show");

    }

    public void OnSelectProgress(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[1].AddToClassList("navisSelected");
        // Progres.SetActive(true);
        // Home.SetActive(false);
        // Profile.SetActive(false);

        _progressMenu.AddToClassList("common-show");
        _homeMenu.RemoveFromClassList("common-show");
        _profileMenu.RemoveFromClassList("common-show");
    }

    public void OnSelectProfile(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[2].AddToClassList("navisSelected");
        // Profile.SetActive(true);
        // Home.SetActive(false);
        // Progres.SetActive(false);

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
