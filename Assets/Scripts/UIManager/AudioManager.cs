using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;


    [Header("Audio Clips")]
    public AudioClip[] musicTracks;
    // public AudioClip attack1Clip;
    // public AudioClip attack2Clip;
    // public AudioClip jumpEffectClip;
    // public AudioClip collectDiamondClip;
    // public AudioClip collectPotionClip;

    private bool isMusicOn = true;
    private bool isSfxOn = true;

    private const string MusicOnKey = "MusicOn";
    private const string SfxOnKey = "SfxOn";
    private const string VolumeKey = "Volume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        LoadAudioSettings();
        ApplyAudioSettings();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.buildIndex);
    }

    private void PlaySceneMusic(int sceneIndex)
    {
        if (musicTracks.Length > sceneIndex && musicTracks[sceneIndex] != null)
        {
            musicSource.Stop();
            musicSource.clip = musicTracks[sceneIndex];
            if (isMusicOn)
            {
                musicSource.Play();
            }
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void LoadAudioSettings()
    {
        isMusicOn = PlayerPrefs.GetInt(MusicOnKey, 1) == 1;
        isSfxOn = PlayerPrefs.GetInt(SfxOnKey, 1) == 1;
        float volume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        SetMusicVolume(volume * 0.2f);
        SetSfxVolume(volume);
    }

    public void SaveMusicSetting()
    {
        PlayerPrefs.SetInt(MusicOnKey, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveSfxSetting()
    {
        PlayerPrefs.SetInt(SfxOnKey, isSfxOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveVolumeSetting(float volume)
    {
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public bool IsMusicOn() => isMusicOn;
    public bool IsSfxOn() => isSfxOn;
    public float GetVolume() => musicSource != null ? musicSource.volume / 0.2f : 1f;
    public float GetVolumeRaw()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 1f);
    }

    private void ApplyAudioSettings()
    {
        if (isMusicOn)
            musicSource.Play();
        else
            musicSource.Stop();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        ApplyAudioSettings();
        SaveMusicSetting();
    }

    public void ToggleSfx()
    {
        isSfxOn = !isSfxOn;
        SaveSfxSetting();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;
        SaveVolumeSetting(volume / 0.2f);
    }

    public void SetSfxVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
    }

    public void PlaySfx(AudioClip clip)
    {
        if (isSfxOn && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}