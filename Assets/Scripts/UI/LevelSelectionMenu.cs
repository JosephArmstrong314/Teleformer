using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionMenu : MonoBehaviour
    {
        public Transform buttonRoot;

        private void Awake()
        {
            int levelUnlocked = PlayerPrefs.GetInt("Level");
            for (int i = 0; i <= levelUnlocked; i++)
            {
                buttonRoot.GetChild(i).GetComponent<Button>().interactable = true;
            }
            for (int i = levelUnlocked+1; i < buttonRoot.childCount; i++)
            {
                buttonRoot.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        public void OnLevelButton(Transform buttonTransform)
        {
            int index = buttonTransform.GetSiblingIndex();
            LevelManager.Instance.SetLevel(index);
            SceneManager.LoadScene(LevelManager.Instance.scenePaths[index]);
        }
    }
}