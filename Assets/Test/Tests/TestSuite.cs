using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    // A Test behaves as an ordinary method
    [Test]
    public void DoorLocked()
    {
        // Use the Assert class to test conditions
        GameObject door =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Props/Door Locked"));
        Assert.True(door.GetComponent<Door>().isLocked);
        Object.Destroy(door.gameObject);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSuiteWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
