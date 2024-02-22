using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class IntegrationTest
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Test Scene");
    }
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerDieOnSpike()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        GameObject gameOverMenu = GameObject.FindWithTag("Canvas").transform.GetChild(2).gameObject;
        Assert.IsFalse(gameOverMenu.activeSelf);
        
        //Game over menu show up after player is died
        yield return new WaitUntil(()=>Time.timeScale==0);
        Assert.IsTrue(gameOverMenu.activeSelf);
    }
}
