using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections;

public class CertificateDownloader : MonoBehaviour
{
    [Header("UI Certificate Elements")]
    public Image certificateBackground;   // Sertifikat background (UI)
    public Text playerNameText;           // Nama pemain (Legacy UI Text)

    [Header("Button")]
    public Button downloadButton;         // Tombol download

    [Header("Notification")]
    public Text notificationText;         // Popup notifikasi (Legacy Text)

    [Header("Save Settings")]
    public string fileName = "Certificate.png";  // Nama file hasil download
    private string playerDataFile = "playerData.json"; // File tempat nama disimpan

    [Header("Render Settings")]
    public Camera certificateCamera;      // Kamera khusus sertifikat
    public RenderTexture renderTexture;   // RenderTexture tempat menangkap hasil kamera

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFile(byte[] array, int byteLength, string fileName);
#endif

    void Start()
    {
        LoadPlayerName();

        if (notificationText != null)
            notificationText.gameObject.SetActive(false);

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
            playerNameText.text = "No Name"; // fallback kalau belum ada nama
        }
    }

    public void SaveCertificate()
    {
        StartCoroutine(CaptureCertificate());
    }

    private IEnumerator CaptureCertificate()
    {
        yield return new WaitForEndOfFrame();

        if (certificateCamera == null || renderTexture == null)
        {
            Debug.LogError("CertificateCamera atau RenderTexture belum diassign!");
            yield break;
        }

        // Render kamera ke texture
        certificateCamera.targetTexture = renderTexture;
        certificateCamera.Render();

        // Ambil hasil render dari RenderTexture
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        // Kembalikan target texture kamera dan aktif render
        certificateCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string savedPath = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        savedPath = Path.Combine("/storage/emulated/0/Download", fileName);
        File.WriteAllBytes(savedPath, bytes);

        // Supaya muncul di Gallery kita pakai MediaScanner
        using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection");
            mediaScanner.CallStatic("scanFile", context, new string[] { savedPath }, null, null);
        }

#elif UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(bytes, bytes.Length, fileName);
        savedPath = "Browser Download";
#else
        savedPath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(savedPath, bytes);
#endif

        Debug.Log("Certificate saved to: " + savedPath);
        ShowNotification("Certificate Saved!");
    }

    private void ShowNotification(string message)
    {
        if (notificationText == null) return;

        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(2f));
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.gameObject.SetActive(false);
    }
}

