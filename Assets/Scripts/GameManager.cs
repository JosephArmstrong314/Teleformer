using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    // Singleton Instance
    public static GameManager Instance;

    // Default Player Pref values
    // Mute Settings (0 means unmuted, 1 means muted)
    private const int DefaultMainMute = 0;
    private const int DefaultMusicMute = 0;
    private const int DefaultSFXMute = 0;

    // Volume Settings
    private const float DefaultMainVolume = 0.5f;
    private const float DefaultMusicVolume = 0.5f;
    private const float DefaultSFXVolume = 0.5f;

    // Graphics Settings (default resolution is 1920 x 1080)
    private const int DefaultResolutionWidth = 1920;
    private const int DefaultResolutionHeight = 1080;
    private const int DefaultFullScreen = 1; // 1 means fullscreen
    private const int DefaultQuality = 2; // Medium

    // Key Binding Default Values (as strings)

    private const string DefaultLeft = "A";
    private const string DefaultRight = "D";
    private const string DefaultUp = "W";
    private const string DefaultDown = "S";

    private const string DefaultJump = "Space";

    private const string DefaultLock = "LeftShift";

    private const string DefaultShoot = "Slash";
    private const string DefaultEnergize = "RightShift";

    private const string DefaultPause = "Escape";
    private const string DefaultRestart = "R";


    // keycodes

    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode Lock { get; set; }
    public KeyCode shoot { get; set; }
    public KeyCode energize { get; set; }
    public KeyCode pause { get; set; }
    public KeyCode restart { get; set; }

    void setKeyCodes() {
        left = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", DefaultLeft));
        right = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", DefaultRight));
        up = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", DefaultUp));
        down = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", DefaultDown));
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", DefaultJump));
        Lock = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Lock", DefaultLock));
        shoot = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot", DefaultShoot));
        energize = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Energize", DefaultEnergize));
        pause = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", DefaultPause));
        restart = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Restart", DefaultRestart));
    }

    // Key Binding Dictionary

    private Dictionary<string, string> playerPrefsKeyBinds = new Dictionary<string, string>() {
        {"Left", DefaultLeft},
        {"Right", DefaultRight},
        {"Up", DefaultUp},
        {"Down", DefaultDown},
        {"Jump", DefaultJump},
        {"Lock", DefaultLock},
        {"Shoot", DefaultShoot},
        {"Energize", DefaultEnergize},
        {"Pause", DefaultPause},
        {"Restart", DefaultRestart},
    };

    // Function to set default key binds

    void setPlayerPrefsKeyBindDefaults() {
        foreach (var (key, value) in playerPrefsKeyBinds) {
            if (!(PlayerPrefs.HasKey(key))) {
                PlayerPrefs.SetString(key, value);
            }
        }
    }  

    // Player Prefs Dictionaries
    private Dictionary<string, int> playerPrefsMutes = new Dictionary<string, int>() {
        {"MainMute", DefaultMainMute},
        {"MusicMute", DefaultMusicMute},
        {"SFXMute", DefaultSFXMute}
    };
    private Dictionary<string, float> playerPrefsVolumes = new Dictionary<string, float>() {
        {"MainVolume", DefaultMainVolume},
        {"MusicVolume", DefaultMusicVolume},
        {"SFXVolume", DefaultSFXVolume}
    };
    private Dictionary<string, float> playerPrefsSavedVolumes = new Dictionary<string, float>() {
        {"SavedMainVolume", DefaultMainVolume},
        {"SavedMusicVolume", DefaultMusicVolume},
        {"SavedSFXVolume", DefaultSFXVolume}
    };
    private Dictionary<string, int> playerPrefsGraphics = new Dictionary<string, int>() {
        {"ResolutionWidth", DefaultResolutionWidth},
        {"ResolutionHeight", DefaultResolutionHeight},
        {"FullScreen", DefaultFullScreen},
        {"Quality", DefaultQuality}
    };

    // Singeton Awake() pattern
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // Set default values on awake so values are available for all other game objects' start()'s
        setPlayerPrefsDefaults();
        setPlayerPrefsKeyBindDefaults();
        setKeyCodes();
        setGraphics();
    }

    // Sets Player Prefs to default values if they aren't already set
    void setPlayerPrefsDefaults() {

        // Loops through each (key,value) pair in the dictionary of integer values
        foreach (var (key, value) in playerPrefsMutes) {

            // If the key doesn't exist, set it to the default value
            if (!(PlayerPrefs.HasKey(key))) {
                PlayerPrefs.SetInt(key, value);

                // If the key exists, and the value is 1 (meaning mute), set the value of the corresponding volume to 0
            } else {
                if (PlayerPrefs.GetInt(key) == 1) {
                    PlayerPrefs.SetFloat(key[..^4] + "Volume", 0.0001f);
                }
            }
        }

        // Loops through each (key,value) pair in the dictionary of float values
        foreach (var (key, value) in playerPrefsVolumes) {

            // If the key doesn't exist, set it to the default value
            if (!(PlayerPrefs.HasKey(key))) {
                PlayerPrefs.SetFloat(key, value);

                // If the key exists, and the value is 0.0f (meaning mute), set the value of the corresponding mute to 1
            } else {
                if (PlayerPrefs.GetFloat(key) == 0.0001f) {
                    PlayerPrefs.SetInt(key[..^6] + "Mute", 1);
                }
            }
        }

        // Loops through each (key,value) pair in the dictionary of float values
        foreach (var (key, value) in playerPrefsSavedVolumes) {

            // If the key doesn't exist, set it to the default value
            if (!(PlayerPrefs.HasKey(key))) {
                PlayerPrefs.SetFloat(key, value);
            }
        }

        foreach (var (key, value) in playerPrefsGraphics) {
            if (!(PlayerPrefs.HasKey(key))) {
                PlayerPrefs.SetInt(key, value);
            }
        }
    }

    void setGraphics() {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth"), PlayerPrefs.GetInt("ResolutionHeight"), Screen.fullScreen);
        if (PlayerPrefs.GetInt("FullScreen") == 1) {
            Screen.fullScreen = true;
        } else {
            Screen.fullScreen = false;
        }
    }
}