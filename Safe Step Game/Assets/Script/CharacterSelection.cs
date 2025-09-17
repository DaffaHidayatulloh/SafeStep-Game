using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
    }

    [Header("UI References")]
    public Image maleImage;
    public Image femaleImage;
    public Button saveButton;

    [Header("Highlight Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    private string selectedCharacter = "";
    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/character.json";

        // Reset warna awal
        maleImage.color = normalColor;
        femaleImage.color = normalColor;

        // Event klik gambar
        maleImage.GetComponent<Button>().onClick.AddListener(() => SelectCharacter("Male"));
        femaleImage.GetComponent<Button>().onClick.AddListener(() => SelectCharacter("Female"));

        // Event klik tombol save
        saveButton.onClick.AddListener(SaveSelection);
    }

    private void SelectCharacter(string name)
    {
        selectedCharacter = name;

        // Update warna highlight
        if (name == "Male")
        {
            maleImage.color = selectedColor;
            femaleImage.color = normalColor;
        }
        else if (name == "Female")
        {
            femaleImage.color = selectedColor;
            maleImage.color = normalColor;
        }

        Debug.Log("Selected: " + name);
    }

    private void SaveSelection()
    {
        if (string.IsNullOrEmpty(selectedCharacter))
        {
            Debug.LogWarning("No character selected!");
            return;
        }

        CharacterData data = new CharacterData();
        data.characterName = selectedCharacter;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Character saved: " + selectedCharacter);

        // Pindah ke Home Screen
        SceneManager.LoadScene("Home Screen");
    }
}


