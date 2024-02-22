# Deployment


## Play the game locally
1. Download the .zip file for your operating system.
    - Download latest Mac build: https://drive.google.com/file/d/1FNuQyAT0VIy7lBe6yxjobjg2RrKu7jcY/view?usp=sharing
    - Download latest Windows build: https://drive.google.com/file/d/1HHcKcFoJDC6XD1czCoZn9B7nd3dB7jbe/view?usp=sharing
2. Unzip
3. Run the game:
   - For windows run the file Teleformer.exe
      - You may be see an "unrecognized app" prompt - click on "More info" then "Run anyway"
   - For Mac run the .app
      - If you cannot open the game, rightclick the game and select open.
4. Play!
 
## Play the game online (no download needed!)
1. Open this link in a browser: https://qiruhu.itch.io/teleformer
2. Click run game.
3. Play!

## Download and Setup the Code
1. Download GIT using the instructions on this page: https://git-scm.com/
2. Download GIT LFS using the instructions on this page: https://git-lfs.com/
3. Download the Unity Hub: https://unity.com/ (personal and student plans are free)
4. From the Unity Hub installs page download the Unity Editor version 2021.3.16f1
5. Clone our reposition onto your local machine: git clone https://github.com/ucsb-cs148-w23/project-t05-telekineticgame.git
6. From the Unity Hub click the arrow next to Open and select "Add project from disk" and add the git repo directory you just downloaded
7. Set the Unity Project's editor version to 2021.3.16f1 in the Hub, then click to open the project and wait for it to load
8. You should now have access to the Unity Project and Code
9. To make a build of the game, in the Unity Editor, click File -> Build Settings
10. Make sure that all Scenes from the directories: "Scenes/UI" & "Scenes/Levels" are in the Scenes In Build tab with Main Menu at the top (if not then add them using the Add Open Scenes button after opening the missing scenes)
11. Click Build
