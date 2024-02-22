# TESTING

## Unit Test in Unity
Unity provides a unit test feature called Test Runner which utilizes the NUnit framework. Here are a summary of the steps for setting up and running unit test using Test Runner. For detailed instruction, please refer to https://www.kodeco.com/9454-introduction-to-unity-unit-testing.

1. Open Test Runner window. Select PlayMode. Create PlayMode Test Assembly Folder.
2. Set Up the Test Assembly. Create the test script in the test folder by clicking the button in Test Runner window.
3. Write Unit Test. There are mainly 2 types of test: tests behave as ordinary methods and tests behave like a coroutine in Play Mode.
4. Run Unit Test. In Test Runner window, click run all or run selected tests.

### Existing Unit Tests
In the script Assets/Test/Tests/TestSuite.cs:
- DoorLocked: Ensure the prefab "Door Locked" is locked when instantiated. Test instantiates a “Door Locked” prefab and asserts that it begins in the locked state.

### Unit Test Plans Going Forward:
The game development industry is built on testing, but the requirements for a single player game differ in kind from those of a web application and so the testing is likewise different. Games do not need to be bug free, they instead need to have the experience of common users be as smooth and fun as possible. The state of a game at any time is likewise far more variable with more degrees of freedom than a standard app so uncovering consequential bugs is challenging to do with simple unit tests. This means that development time is best focused on addressing the results of end-to-end play testing rather than on ensuring there are no edge cases, as play testing checks that the core path works. Because of this and because of the rapid concept iteration inherent to revising based on what players find fun, there is less value in focusing on unit tests for this project over other forms of testing.

## Component Test in Unity
Component test can be similarly done using Unity Built-in Test Runner. The changes in the game scene can be detected by Unity Test that behaves like a coroutine in Play Mode. 

In the script Assets/Test/Tests/IntegrationTest.cs:
- PlayerDieOnSpike: In this test, a test scene is loaded in which the player will fall onto a spike and die. As a result, the Game Over Menu will be triggered. This test tests that the Game Over Menu is inactive at the beginning and is activated after the player dies.

## Play Testing Plans Going Forward:
Our teams testing direction going forward is primarily to put the game in the hands of players and condition our development and bug fixing on the response we receive from these end-to-end play tests. This will come in the form of developer end-to-end play throughs of the game with the intent of discovering bugs, player full game testing to see if the puzzle design is comfortable and bug free for players outside the dev team, and player A/B testing on feature concepts to orient the development toward player enjoyment. This is the most valuable testing process for our circumstances because it can find difficult to automatically test for errors that would hurt the game experience and orient our design decisions on player fun.

## A screenshot of the Test Runner and Test Scene
![test](../Assets/Image/TestRunnerScreenshot.png)
