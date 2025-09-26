using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class UI_Events : MonoBehaviour
{
    

    [Header("Panels")]
    public GameObject Home;
    public GameObject Progres;
    public GameObject Profile;

    private UIDocument _document;
    // private Button _button;
    private List<VisualElement> _navButtons;




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

        Home.SetActive(true);
        Progres.SetActive(false);
        Profile.SetActive(false);
    }

    public void OnSelectProgress(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[1].AddToClassList("navisSelected");
        Progres.SetActive(true);
        Home.SetActive(false);
        Profile.SetActive(false);
    }

    public void OnSelectProfile(ClickEvent evt)
    {
        TempResetClass();
        _navButtons[2].AddToClassList("navisSelected");
        Profile.SetActive(true);
        Home.SetActive(false);
        Progres.SetActive(false);
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
