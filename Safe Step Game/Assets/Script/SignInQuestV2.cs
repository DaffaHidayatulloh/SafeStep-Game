using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignInQuestV2 : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField nameInputField;
    public Button saveButton;
    public Text warningText;

    [Header("Panel SignIn (akan ditutup setelah save)")]
    public GameObject signInPanel;

    [Header("Teks Display Nama")]
    public Text profileNameText;   // Teks nama di Profile
    public Text homeNameText;      // Teks nama di Home Screen

    private string fileName = "playerData.json";

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }

    void Start()
    {
        warningText.gameObject.SetActive(false);

        saveButton.onClick.AddListener(SaveName);

        // Jika file ada, otomatis load nama
        if (File.Exists(GetFilePath()))
        {
            string json = File.ReadAllText(GetFilePath());
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            nameInputField.text = data.playerName;

            // Update kedua teks
            UpdateDisplayTexts(data.playerName);
        }
    }

    public void SaveName()
    {
        string enteredName = nameInputField.text;

        if (!string.IsNullOrEmpty(enteredName))
        {
            // Simpan ke file JSON
            PlayerData data = new PlayerData();
            data.playerName = enteredName;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(GetFilePath(), json);

            // Update teks di profile dan home
            UpdateDisplayTexts(enteredName);

            // Tutup panel SignIn
            if (signInPanel != null)
                signInPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowWarning("Name cannot be empty!"));
        }
    }

    void UpdateDisplayTexts(string newName)
    {
        if (profileNameText != null)
            profileNameText.text = newName;

        if (homeNameText != null)
            homeNameText.text = newName;
    }

    string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);

        // Shake effect
        Vector3 originalPos = warningText.transform.localPosition;
        float shakeDuration = 0.3f;
        float shakeMagnitude = 10f;
        float elapsedShake = 0f;

        while (elapsedShake < shakeDuration)
        {
            elapsedShake += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            warningText.transform.localPosition = originalPos + new Vector3(offsetX, 0, 0);
            yield return null;
        }
        warningText.transform.localPosition = originalPos;

        yield return new WaitForSeconds(2f);

        // Fade out
        Color c = warningText.color;
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            c.a = alpha;
            warningText.color = c;
            yield return null;
        }

        warningText.gameObject.SetActive(false);
    }
}


