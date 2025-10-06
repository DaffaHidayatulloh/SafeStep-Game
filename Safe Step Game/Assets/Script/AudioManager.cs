using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Daftar BGM")]
    public AudioClip[] bgmList;

    [Header("Daftar SFX")]
    public AudioClip[] sfxClips;

    private int currentBGMIndex = -1;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// Memainkan BGM dari index tertentu
    public void PlayBGM(int index)
    {
        if (currentBGMIndex == index && bgmSource.isPlaying)
            return;

        if (bgmList.Length > index && bgmList[index] != null)
        {
            bgmSource.clip = bgmList[index];
            bgmSource.loop = true;
            bgmSource.Play();
            currentBGMIndex = index;
        }
    }

    /// Menghentikan BGM
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    /// Pause / Unpause BGM
    public void ToggleBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Pause();
        else
            bgmSource.UnPause();
    }

    /// Memainkan SFX dari index tertentu
    public void PlaySFX(int index)
    {
        if (sfxClips.Length > index && sfxClips[index] != null)
            sfxSource.PlayOneShot(sfxClips[index]);
    }

    /// Menghentikan SFX jika sedang diputar
    public void StopSFX()
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();
    }

    /// Menghentikan semua SFX jika sedang diputar
    public void StopAllSFX()
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();
    }

    /// Menjeda semua audio (BGM dan SFX)
    public void PauseAllAudio()
    {
        if (bgmSource.isPlaying) bgmSource.Pause();
    }

    /// Melanjutkan semua audio yang dijeda
    public void ResumeAllAudio()
    {
        if (bgmSource.clip != null) bgmSource.UnPause();
    }


    /// Mendapatkan index BGM yang sedang dimainkan
    public int GetCurrentBGMIndex()
    {
        return currentBGMIndex;
    }
}
