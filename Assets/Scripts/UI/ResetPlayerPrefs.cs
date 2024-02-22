using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ResetPlayerPrefs : MonoBehaviour
    {
        private const string levelKey = "Level";
        
        public void ResetPlayerPrefsButton()
        {
            PlayerPrefs.SetInt(levelKey, 0);
            for (int i = 0; i < LevelManager.Instance.scenePaths.Count; i++)
            {
                PlayerPrefs.SetInt("Ruby" + i.ToString(), 0);
                PlayerPrefs.SetInt("Square" + i.ToString(), 0);
            }
            PlayerPrefs.SetInt("Ruby", 0);
            PlayerPrefs.SetInt("Square", 0);


        }

        public void ResetPlayerPrefsOptions() {
            PlayerPrefs.SetInt("MainMute", 0);
            PlayerPrefs.SetInt("MusicMute", 0);
            PlayerPrefs.SetInt("SFXMute", 0);

            PlayerPrefs.SetFloat("MainVolume", 0.5f);
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);

            PlayerPrefs.SetInt("ResolutionWidth", 1920);
            PlayerPrefs.SetInt("ResolutionHeight", 1080);
            PlayerPrefs.SetInt("FullScreen", 1);
            PlayerPrefs.SetInt("Quality", 2);

            PlayerPrefs.SetString("Left", "A");
            PlayerPrefs.SetString("Right", "D");
            PlayerPrefs.SetString("Up", "W");
            PlayerPrefs.SetString("Down", "S");
            PlayerPrefs.SetString("Jump", "Space");
            PlayerPrefs.SetString("Lock", "LeftShift");
            PlayerPrefs.SetString("Shoot", "Slash");
            PlayerPrefs.SetString("Energize", "RightShift");
            PlayerPrefs.SetString("Pause", "Escape");
            PlayerPrefs.SetString("Restart", "R");

            GameManager.Instance.left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
            GameManager.Instance.right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
            GameManager.Instance.up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W"));
            GameManager.Instance.down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S"));
            GameManager.Instance.jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
            GameManager.Instance.Lock = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Lock", "LeftShift"));
            GameManager.Instance.shoot = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot", "Slash"));
            GameManager.Instance.energize = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Energize", "RightShift"));
            GameManager.Instance.pause = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", "Escape"));
            GameManager.Instance.restart = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Restart", "R"));

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}