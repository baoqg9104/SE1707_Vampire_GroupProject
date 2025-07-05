using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    public Button closeBtn;
    public Button toggleMusicBtn;
    public Image toggleMusicImage;
    public Button toggleSfxBtn;
    public Image toggleSfxImage;
    public Slider volumeSlider;

    public Sprite onImage;
    public Sprite offImage;

    private bool isMusicOn;
    private bool isSfxOn;
    private float volume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(transform.root.gameObject);
        }
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.LoadAudioSettings();
            isMusicOn = AudioManager.Instance.IsMusicOn();
            isSfxOn = AudioManager.Instance.IsSfxOn();
            volume = AudioManager.Instance.GetVolumeRaw();
        }
        else
        {
            isMusicOn = true;
            isSfxOn = true;
            volume = 1f;
        }

        UpdateMusicButtonUI();
        UpdateSfxButtonUI();
        UpdateVolumeSliderUI();

        closeBtn.onClick.AddListener(CloseSettingsPanel);
        toggleMusicBtn.onClick.AddListener(ToggleMusic);
        toggleSfxBtn.onClick.AddListener(ToggleSfx);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        gameObject.SetActive(false);
    }

    private void CloseSettingsPanel()
    {
        gameObject.SetActive(false);
    }

    private void ToggleMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
            isMusicOn = AudioManager.Instance.IsMusicOn();
        }
        UpdateMusicButtonUI();
    }

    private void ToggleSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSfx();
            isSfxOn = AudioManager.Instance.IsSfxOn();
        }
        UpdateSfxButtonUI();
    }

    private void SetVolume(float value)
    {
        volume = value;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume * 0.2f);
            AudioManager.Instance.SetSfxVolume(volume);
        }
    }

    private void UpdateMusicButtonUI()
    {
        if (isMusicOn)
        {
            toggleMusicImage.sprite = onImage;
        }
        else
        {
            toggleMusicImage.sprite = offImage;
        }
    }

    private void UpdateSfxButtonUI()
    {
        if (isSfxOn)
        {
            toggleSfxImage.sprite = onImage;
        }
        else
        {
            toggleSfxImage.sprite = offImage;
        }
    }

    private void UpdateVolumeSliderUI()
    {
        volumeSlider.value = volume;
    }
}