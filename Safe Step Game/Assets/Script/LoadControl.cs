using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadControl : MonoBehaviour
{
    private string playerDataFile = "playerData.json";
    private string characterDataFile = "character.json";

    void Start()
    {
        string playerPath = Path.Combine(Application.persistentDataPath, playerDataFile);
        string characterPath = Path.Combine(Application.persistentDataPath, characterDataFile);

        // Cek apakah kedua file ada
        if (File.Exists(playerPath) && File.Exists(characterPath))
        {
            // Kalau ada keduanya, langsung ke Home Screen
            SceneManager.LoadScene("Home Screen");
        }
        else
        {
            Debug.Log("Data belum lengkap. Tetap di scene ini.");
        }
    }
}
