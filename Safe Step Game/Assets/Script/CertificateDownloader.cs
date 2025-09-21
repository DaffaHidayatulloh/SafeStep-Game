using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CertificateDownloader : MonoBehaviour
{
    [Header("UI Certificate Elements")]
    public Image certificateBackground;   // Sertifikat background
    public Text playerNameText;           // Nama pemain (Legacy UI Text)

    [Header("Button")]
    public Button downloadButton;         // Tombol download

    [Header("Save Settings")]
    public string fileName = "Certificate.png";  // Nama file hasil download
    private string playerDataFile = "playerData.json"; // File tempat nama disimpan

    void Start()
    {
        // Ambil nama dari JSON dan tampilkan di teks
        LoadPlayerName();

        if (downloadButton != null)
        {
            downloadButton.onClick.AddListener(SaveCertificate);
        }
    }

    void LoadPlayerName()
    {
        string path = Path.Combine(Application.persistentDataPath, playerDataFile);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SignInQuest.PlayerData data = JsonUtility.FromJson<SignInQuest.PlayerData>(json);
            playerNameText.text = data.playerName;
        }
        else
        {
            playerNameText.text = "No Name"; // fallback
        }
    }

    public void SaveCertificate()
    {
        StartCoroutine(CaptureCertificate());
    }

    private System.Collections.IEnumerator CaptureCertificate()
    {
        yield return new WaitForEndOfFrame();

        // Ambil posisi sertifikat
        RectTransform rt = certificateBackground.GetComponent<RectTransform>();
        Vector2 size = rt.rect.size;
        Vector2 pos = rt.position;

        int width = Mathf.RoundToInt(size.x);
        int height = Mathf.RoundToInt(size.y);

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(pos.x - width / 2, pos.y - height / 2, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();

#if UNITY_ANDROID && !UNITY_EDITOR
        // Simpan ke folder Download agar mudah diakses
        string path = Path.Combine("/storage/emulated/0/Download", fileName);
        File.WriteAllBytes(path, bytes);

        // Supaya muncul di Gallery kita pakai MediaScanner
        using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection");
            mediaScanner.CallStatic("scanFile", context, new string[] { path }, null, null);
        }

        Debug.Log("Certificate saved to: " + path);
#else
        // Versi Editor / PC fallback
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Certificate saved to (Editor): " + path);
#endif
    }
}

