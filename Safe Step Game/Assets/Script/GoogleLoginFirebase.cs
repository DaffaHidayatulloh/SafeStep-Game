using Firebase.Auth;
using Firebase;
using Google;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;         

public class GoogleLoginFirebase : MonoBehaviour
{
    [Header("Firebase & Google Config")]
    [Tooltip("Masukkan WebClient ID dari Firebase Console")]
    private string GoogleAPI = "940344831661-514ucene5kah26fv0tmf7087eg21k2fu.apps.googleusercontent.com"; // Ganti dengan Web Client ID kamu

    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;
    private FirebaseUser user;

    [Header("UI")]
    public Image UserProfilePic;
    public Text messageText; //  Tambahan: untuk menampilkan pesan error di UI (Legacy Text)

    private bool isGoogleSignInInitialized = false;
    private string fileName = "playerData.json";

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }

    void Start()
    {
        InitFirebase();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("Firebase initialized successfully!");
                ShowMessage("Firebase siap digunakan ");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                ShowMessage("Gagal inisialisasi Firebase ");
            }
        });
    }

    void InitFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login()
    {
        ShowMessage("Memulai login Google...");

        // Inisialisasi Google Sign-In hanya sekali
        if (!isGoogleSignInInitialized)
        {
            configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = GoogleAPI,
                RequestEmail = true,
                UseGameSignIn = false,
                ForceTokenRefresh = true
            };

            GoogleSignIn.Configuration = configuration;
            isGoogleSignInInitialized = true;
        }

        // Jalankan proses Sign-In
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleSignIn);
    }

    private void OnGoogleSignIn(Task<GoogleSignInUser> task)
    {
        if (task.IsCanceled)
        {
            Debug.Log("Login Google dibatalkan pengguna.");
            ShowMessage("Login dibatalkan oleh pengguna.");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("Terjadi error saat login Google: " + task.Exception);
            ShowMessage("Gagal login Google Pastikan koneksi internet aktif atau coba lagi.");
            return;
        }

        // Jika berhasil login Google
        GoogleSignInUser googleUser = task.Result;
        Debug.Log("Berhasil login Google sebagai: " + googleUser.DisplayName);
        ShowMessage("Login Google berhasil, menghubungkan ke Firebase...");

        Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(OnFirebaseAuthComplete);
    }

    private void OnFirebaseAuthComplete(Task<FirebaseUser> task)
    {
        if (task.IsCanceled)
        {
            Debug.Log("Autentikasi Firebase dibatalkan.");
            ShowMessage("Autentikasi dibatalkan ");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("Autentikasi Firebase gagal: " + task.Exception);
            ShowMessage("Gagal autentikasi Firebase Periksa koneksi internet atau konfigurasi Firebase.");
            return;
        }

        // Jika berhasil login Firebase
        user = auth.CurrentUser;
        Debug.Log("Login Firebase sukses! User: " + user.DisplayName);
        ShowMessage("Login sukses! Selamat datang, " + user.DisplayName);

        // Simpan nama ke JSON
        SavePlayerName(user.DisplayName);

        // (Opsional) Jika ingin load foto profil, bisa tetap dipakai
        if (UserProfilePic != null && user.PhotoUrl != null)
            StartCoroutine(LoadImage(user.PhotoUrl.ToString()));

        // Pindah ke scene EditAvatar
        SceneManager.LoadScene("EditAvatar");
    }

    private void SavePlayerName(string playerName)
    {
        PlayerData data = new PlayerData { playerName = playerName };

        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, json);

        Debug.Log($"Nama player '{playerName}' disimpan ke: {filePath}");
    }

    IEnumerator LoadImage(string imageUri)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            UserProfilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        else
        {
            Debug.LogWarning("Gagal memuat foto profil: " + www.error);
            ShowMessage("Gagal memuat foto profil.");
        }
    }

    // Fungsi tambahan untuk menampilkan teks di layar
    private void ShowMessage(string msg)
    {
        Debug.Log(msg);
        if (messageText != null)
        {
            messageText.text = msg;
        }
    }
}
