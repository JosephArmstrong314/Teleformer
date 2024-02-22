using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Level
{
    public class Level1Tutorial : MonoBehaviour
    {
        public List<string> instructions;
        public TextMeshProUGUI tutorialText;
        public GameObject square1;
        public GameObject square2;
        public ButtonLevel1 button;

        private void Start()
        {
            StartCoroutine(ShowTutorial());
        }

        public IEnumerator ShowTutorial()
        {
            tutorialText.text = instructions[0];
            bool getKey = false;
            while (!getKey)
            {
                if (
                    (
                        Input.GetKey(KeyCode.A)
                        || Input.GetKey(KeyCode.S)
                        || Input.GetKey(KeyCode.D)
                        || Input.GetKey(KeyCode.W)
                    ) || Input.GetKey(KeyCode.Space)
                )
                    getKey = true;
                yield return null;
            }

            //Connect the 2 boxes
            tutorialText.text = instructions[2];
            yield return new WaitForSeconds(2);

            //Select the first box
            tutorialText.text = instructions[2];
            square1.SetActive(true);
            square2.SetActive(true);
            yield return new WaitForSeconds(5);
            // while (true /*not box selected*/)
            //     yield return null;

            //Select the second box
            tutorialText.text = instructions[3];
            yield return new WaitForSeconds(5);
            // while (true /*not box selected*/)
            //     yield return null;

            //Hold LEFT shift to aim
            tutorialText.text = instructions[4];
            yield return new WaitForSeconds(5);
            // while (true /*not box selected*/)
            //     yield return null;


            //Press the button
            tutorialText.text = instructions[5];
            while (!button.IsPressed())
                yield return null;
            square1.SetActive(false);
            square2.SetActive(false);

            tutorialText.text = instructions[6];
        }
    }
}
