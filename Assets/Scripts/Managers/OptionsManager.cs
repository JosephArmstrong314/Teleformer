using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour {
    
    // Singleton Instance
    public static OptionsManager Instance;

    // References to all relevant game objects in options menu scene
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [SerializeField] private Image _toggleMainMute;
    [SerializeField] private Image _toggleMusicMute;
    [SerializeField] private Image _toggleSFXMute;

    [SerializeField] private Sprite _unmuted, _muted;

    [SerializeField] private TextMeshProUGUI _mainVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeText;

    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    [SerializeField] private Button _fullScreenButton;
    [SerializeField] private TextMeshProUGUI _fullScreenButtonText;

    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    [SerializeField] private Button _up;
    [SerializeField] private Button _down;
    [SerializeField] private Button _jump;
    [SerializeField] private Button _lock;
    [SerializeField] private Button _shoot;
    [SerializeField] private Button _energize;
    [SerializeField] private Button _pause;
    [SerializeField] private Button _restart;

    [SerializeField] private TextMeshProUGUI _leftText;
    [SerializeField] private TextMeshProUGUI _rightText;
    [SerializeField] private TextMeshProUGUI _upText;
    [SerializeField] private TextMeshProUGUI _downText;
    [SerializeField] private TextMeshProUGUI _jumpText;
    [SerializeField] private TextMeshProUGUI _lockText;
    [SerializeField] private TextMeshProUGUI _shootText;
    [SerializeField] private TextMeshProUGUI _energizeText;
    [SerializeField] private TextMeshProUGUI _pauseText;
    [SerializeField] private TextMeshProUGUI _restartText;

    Event keyEvent;
    KeyCode newKey;
    bool waitingForKey;

    // private variables to store player prefs in
    private float _mainVolume;
    private float _musicVolume;
    private float _sfxVolume;

    private float _savedMainVolume;
    private float _savedMusicVolume;
    private float _savedSFXVolume;

    private int _mainMute;
    private int _musicMute;
    private int _sfxMute;

    private int _quality;

    private int _resolutionWidth;
    private int _resolutionHeight;

    private int _fullScreen;

    // more private variables
    private Resolution[] _resolutions;

    void Awake() {
        Instance = this;
    }

    void Start() {
        UpdateValuesFromPlayerPrefs();

        // Can comment out this line
        OptionalDebugMessages();

        SetValuesForSlidersAndText();

        SetValuesForButtons();

        SetQualityDropdown();

        SetResolutionDropdown();

        setKeyBindButtons();

        waitingForKey = false;
    }

    void OnGUI() {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey) {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName) {
        if (!waitingForKey) {
            StartCoroutine(AssignKey(keyName));
        }
    }

    IEnumerator WaitForKey() {
        while(!keyEvent.isKey) {
            yield return null;
        }
    }

    public IEnumerator AssignKey(string keyName) {
        waitingForKey = true;

        yield return WaitForKey();

        switch(keyName) {
            case "left":
                GameManager.Instance.left = newKey;
                _leftText.text = GameManager.Instance.left.ToString();
                PlayerPrefs.SetString("Left", GameManager.Instance.left.ToString());
                break;
            case "right":
                GameManager.Instance.right = newKey;
                _rightText.text = GameManager.Instance.right.ToString();
                PlayerPrefs.SetString("Right", GameManager.Instance.right.ToString());
                break;
            case "up":
                GameManager.Instance.up = newKey;
                _upText.text = GameManager.Instance.up.ToString();
                PlayerPrefs.SetString("Up", GameManager.Instance.up.ToString());
                break;
            case "down":
                GameManager.Instance.down = newKey;
                _downText.text = GameManager.Instance.down.ToString();
                PlayerPrefs.SetString("Down", GameManager.Instance.down.ToString());
                break;
            case "jump":
                GameManager.Instance.jump = newKey;
                _jumpText.text = GameManager.Instance.jump.ToString();
                PlayerPrefs.SetString("Jump", GameManager.Instance.jump.ToString());
                break;
            case "lock":
                GameManager.Instance.Lock = newKey;
                _lockText.text = GameManager.Instance.Lock.ToString();
                PlayerPrefs.SetString("Lock", GameManager.Instance.Lock.ToString());
                break;
            case "shoot":
                GameManager.Instance.shoot = newKey;
                _shootText.text = GameManager.Instance.shoot.ToString();
                PlayerPrefs.SetString("Shoot", GameManager.Instance.shoot.ToString());
                break;
            case "energize":
                GameManager.Instance.energize = newKey;
                _energizeText.text = GameManager.Instance.energize.ToString();
                PlayerPrefs.SetString("Energize", GameManager.Instance.energize.ToString());
                break;
            case "pause":
                GameManager.Instance.pause = newKey;
                _pauseText.text = GameManager.Instance.pause.ToString();
                PlayerPrefs.SetString("Pause", GameManager.Instance.pause.ToString());
                break;
            case "restart":
                GameManager.Instance.restart = newKey;
                _restartText.text = GameManager.Instance.restart.ToString();
                PlayerPrefs.SetString("Restart", GameManager.Instance.restart.ToString());
                break;
        }

        yield return null;
    }

    void setKeyBindButtons() {
        _leftText.text = GameManager.Instance.left.ToString();
        _rightText.text = GameManager.Instance.right.ToString();
        _upText.text = GameManager.Instance.up.ToString();
        _downText.text = GameManager.Instance.down.ToString();
        _jumpText.text = GameManager.Instance.jump.ToString();
        _lockText.text = GameManager.Instance.Lock.ToString();
        _shootText.text = GameManager.Instance.shoot.ToString();
        _energizeText.text = GameManager.Instance.energize.ToString();
        _pauseText.text = GameManager.Instance.pause.ToString();
        _restartText.text = GameManager.Instance.restart.ToString();
    }

    void UpdateValuesFromPlayerPrefs() {
        _mainVolume = PlayerPrefs.GetFloat("MainVolume");
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        _savedMainVolume = PlayerPrefs.GetFloat("SavedMainVolume");
        _savedMusicVolume = PlayerPrefs.GetFloat("SavedMusicVolume");
        _savedSFXVolume = PlayerPrefs.GetFloat("SavedSFXVolume");

        _mainMute = PlayerPrefs.GetInt("MainMute");
        _musicMute = PlayerPrefs.GetInt("MusicMute");
        _sfxMute = PlayerPrefs.GetInt("SFXMute");

        _quality = PlayerPrefs.GetInt("Quality");

        _resolutionWidth = PlayerPrefs.GetInt("ResolutionWidth");
        _resolutionHeight = PlayerPrefs.GetInt("ResolutionHeight");

        _fullScreen = PlayerPrefs.GetInt("FullScreen");
    }

    void OptionalDebugMessages() {
        // For debugging purposes, can comment out select lines
        /*Debug.Log("OptionsManager::Start::_mainVolume = " + (_mainVolume).ToString());
        Debug.Log("OptionsManager::Start::_musicVolume = " + (_musicVolume).ToString());
        Debug.Log("OptionsManager::Start::_sfxVolume = " + (_sfxVolume).ToString());
        Debug.Log("OptionsManager::Start::_savedMainVolume = " + (_savedMainVolume).ToString());
        Debug.Log("OptionsManager::Start::_savedMusicVolume = " + (_savedMusicVolume).ToString());
        Debug.Log("OptionsManager::Start::_savedSFXVolume = " + (_savedSFXVolume).ToString());
        Debug.Log("OptionsManager::Start::_mainMute = " + (_mainMute).ToString());
        Debug.Log("OptionsManager::Start::_musicMute = " + (_musicMute).ToString());
        Debug.Log("OptionsManager::Start::_sfxMute = " + (_sfxMute).ToString());
        Debug.Log("OptionsManager::Start::_quality = " + (_quality).ToString());
        Debug.Log("OptionsManager::Start::_resolutionWidth = " + (_resolutionWidth).ToString());
        Debug.Log("OptionsManager::Start::_resolutionHeight = " + (_resolutionHeight).ToString());
        Debug.Log("OptionsManager::Start::_fullScreen = " + (_fullScreen).ToString());*/

        Debug.Log("OptionsManager::Start::_left = " + PlayerPrefs.GetString("Left"));
        Debug.Log("OptionsManager::Start::_right = " + PlayerPrefs.GetString("Right"));
        Debug.Log("OptionsManager::Start::_up = " + PlayerPrefs.GetString("Up"));
        Debug.Log("OptionsManager::Start::_down = " + PlayerPrefs.GetString("Down"));
        Debug.Log("OptionsManager::Start::_jump = " + PlayerPrefs.GetString("Jump"));
        Debug.Log("OptionsManager::Start::_lock = " + PlayerPrefs.GetString("Lock"));
        Debug.Log("OptionsManager::Start::_shoot = " + PlayerPrefs.GetString("Shoot"));
        Debug.Log("OptionsManager::Start::_energize = " + PlayerPrefs.GetString("Energize"));
        Debug.Log("OptionsManager::Start::_pause = " + PlayerPrefs.GetString("Pause"));
        Debug.Log("OptionsManager::Start::_restart = " + PlayerPrefs.GetString("Restart"));
    }

    void SetValuesForSlidersAndText() {
        _mainVolumeSlider.value = _mainVolume;
        _musicVolumeSlider.value = _musicVolume;
        _sfxVolumeSlider.value = _sfxVolume;

        _mainVolumeText.text = ((int)(_mainVolume * 100)).ToString();
        _musicVolumeText.text = ((int)(_musicVolume * 100)).ToString();
        _sfxVolumeText.text = ((int)(_sfxVolume * 100)).ToString();

        _mainVolumeSlider.onValueChanged.AddListener(val => ChangeMainVolume(val));
        _musicVolumeSlider.onValueChanged.AddListener(val => ChangeMusicVolume(val));
        _sfxVolumeSlider.onValueChanged.AddListener(val => ChangeSFXVolume(val));
    }

    void SetValuesForButtons() {
        if (_mainMute == 0) {
            _toggleMainMute.sprite = _unmuted;
        } else {
            _toggleMainMute.sprite = _muted;
        }

        if (_musicMute == 0) {
            _toggleMusicMute.sprite = _unmuted;
        } else {
            _toggleMusicMute.sprite = _muted;
        }

        if (_sfxMute == 0) {
            _toggleSFXMute.sprite = _unmuted;
        } else {
            _toggleSFXMute.sprite = _muted;
        }

        if (_fullScreen == 0) {
            _fullScreenButtonText.text = "OFF";
        } else {
            _fullScreenButtonText.text = "ON";
        }

        _leftText.text = PlayerPrefs.GetString("Left");
        _rightText.text = PlayerPrefs.GetString("Right");
        _upText.text = PlayerPrefs.GetString("Up");
        _downText.text = PlayerPrefs.GetString("Down");
        _jumpText.text = PlayerPrefs.GetString("Jump");
        _lockText.text = PlayerPrefs.GetString("Lock");
        _shootText.text = PlayerPrefs.GetString("Shoot");
        _energizeText.text = PlayerPrefs.GetString("Energize");
        _pauseText.text = PlayerPrefs.GetString("Pause");
        _restartText.text = PlayerPrefs.GetString("Restart");
    }

    void SetQualityDropdown() {
        _qualityDropdown.value = QualitySettings.GetQualityLevel();
        _qualityDropdown.RefreshShownValue();
    }

    void SetResolutionDropdown() {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        List<string> resolutionStringList = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++) {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            resolutionStringList.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(resolutionStringList);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    void ChangeMainVolume(float value) {
        if (value == 0.0001f) {
            _toggleMainMute.sprite = _muted;
            _mainMute = 1;
        } else {
            _toggleMainMute.sprite = _unmuted;
            _mainMute = 0;
        }

        _mainVolume = value;
        AudioListener.volume = _mainVolume;
        _mainVolumeText.text = ((int)(_mainVolume * 100)).ToString();

        PlayerPrefs.SetFloat("MainVolume", _mainVolume);
        PlayerPrefs.SetInt("MainMute", _mainMute);
    }

    void ChangeMusicVolume(float value) {
        if (value == 0.0001f) {
            _toggleMusicMute.sprite = _muted;
            _musicMute = 1;
        } else {
            _toggleMusicMute.sprite = _unmuted;
            _musicMute = 0;
        }

        _musicVolume = value;
        SoundManager.Instance.UpdateMusicAudioMixerVolume(_musicVolume);
        _musicVolumeText.text = ((int)(_musicVolume * 100)).ToString();

        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.SetInt("MusicMute", _musicMute);
    }

    void ChangeSFXVolume(float value) {
        if (value == 0.0001f) {
            _toggleSFXMute.sprite = _muted;
            _sfxMute = 1;
        } else {
            _toggleSFXMute.sprite = _unmuted;
            _sfxMute = 0;
        }

        _sfxVolume = value;
        SoundManager.Instance.UpdateSFXAudioMixerVolume(_sfxVolume);
        _sfxVolumeText.text = ((int)(_sfxVolume * 100)).ToString();

        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetInt("SFXMute", _sfxMute);
    }

    public void OnMainMuteButtonClicked() {
        if (_mainMute == 0) {
            _savedMainVolume = _mainVolume;
            PlayerPrefs.SetFloat("SavedMainVolume", _savedMainVolume);
            _mainVolumeSlider.value = 0.0001f;
            ChangeMainVolume(0.0001f);
        } else {
            _mainVolume = _savedMainVolume;
            _mainVolumeSlider.value = _mainVolume;
            ChangeMainVolume(_mainVolume);
        }
    }

    public void OnMusicMuteButtonClicked() {
        if (_musicMute == 0) {
            _savedMusicVolume = _musicVolume;
            PlayerPrefs.SetFloat("SavedMusicVolume", _savedMusicVolume);
            _musicVolumeSlider.value = 0.0001f;
            ChangeMusicVolume(0.0001f);
        } else {
            _musicVolume = _savedMusicVolume;
            _musicVolumeSlider.value = _musicVolume;
            ChangeMusicVolume(_musicVolume);
        }
    }

    public void OnSFXMuteButtonClicked() {
        if (_sfxMute == 0) {
            _savedSFXVolume = _sfxVolume;
            PlayerPrefs.SetFloat("SavedSFXVolume", _savedSFXVolume);
            _sfxVolumeSlider.value = 0.0001f;
            ChangeSFXVolume(0.0001f);
        } else {
            _sfxVolume = _savedSFXVolume;
            _sfxVolumeSlider.value = _sfxVolume;
            ChangeSFXVolume(_sfxVolume);
        }
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        _quality = qualityIndex;
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = _resolutions[resolutionIndex];
        _resolutionWidth = resolution.width;
        _resolutionHeight = resolution.height;
        Screen.SetResolution(_resolutionWidth, _resolutionHeight, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionWidth", _resolutionWidth);
        PlayerPrefs.SetInt("ResolutionHeight", _resolutionHeight);
    }

    public void OnFullScreenButtonClicked () {
        if (_fullScreen == 0) {
            _fullScreen = 1;
            Screen.fullScreen = true;
            _fullScreenButtonText.text = "ON";
            PlayerPrefs.SetInt("FullScreen", 1);
        } else {
            _fullScreen = 0;
            Screen.fullScreen = false;
            _fullScreenButtonText.text = "OFF";
            PlayerPrefs.SetInt("FullScreen", 0);
        }
    }
}