# README.md

Project Name: Teleformer

Technologies: Unity, C#

Teleformer is a 2D platformer puzzle game that explores the mechanic of telekinetically linked objects. There will be a player object that can jump, move laterally, push movable objects, and shoot projectiles that link objects together telekinetically. The player movement will be loosely based on Celeste. Objects will be linked via velocity. Also, since this is a single-player game, there is no need to place restrictions of any kind on the player.

Since this is a game, there is really only one "user role", namely the player.

To create the game, we will be using Unity, C#, and various free assets. 


## Installation
### Prerequisites
None
### Dependencies
Operating system: Win 10/11 64 bit, Mac 64 bit
### Installation Steps 
1. Go to the [Latest Github Release](https://github.com/ucsb-cs148-w23/project-t05-telekineticgame/releases/latest)
2. Download the .zip file for your operating system e.g. Teleformer-Executable-Windows-64bit.zip for Windows 64 bit users.
3. Unzip
4. Run the game:
   - For windows run the file Teleformer.exe
      - You may be see an "unrecognized app" prompt - click on "More info" then "Run anyway"
   - For Mac run the .app
### Functionality
On starting of the game, you can press "Play" to start playing the game, or press "Level select" to select the level you want to play.
### Known Problems
The player would be able to continuously jump when linking and standing on a box with current physics, causing the player would be able to fly in the air. We would want the player to apply an opposite force to the box when linking and standing on a box so that the player wouldn't just fly through the map.

### Contributing
| Team Member Name | GitHub ID     | UCSB Email              |
|------------------|---------------|-------------------------|
| Joseph Armstrong | jarmstrong845 | jarmstrong845@ucsb.edu  |
| Andrew Kwon      | andrew-kwon1  | andrew_kwon@ucsb.edu    |
| Qiru Hu          | amarantini    | qiru@ucsb.edu           |
| Matt Reddick     | mattreddick   | matthewreddick@ucsb.edu |
| Hao Wu           | haostevewu    | hao_wu@ucsb.edu         |
| Sheldin Lau      | shethon123    | sheldinlau@ucsb.edu     |
| Connor Gorsuch   | ConnorGit     | connoregorsuch@ucsb.edu |

# Deployment
Download latest Mac build: https://drive.google.com/file/d/1FNuQyAT0VIy7lBe6yxjobjg2RrKu7jcY/view?usp=sharing

Download latest Windows build: https://drive.google.com/file/d/1HHcKcFoJDC6XD1czCoZn9B7nd3dB7jbe/view?usp=sharing

To play the game in a browser (no download needed!), go to this link: https://qiruhu.itch.io/teleformer

Link to full deployment instruction: https://github.com/ucsb-cs148-w23/project-t05-telekineticgame/blob/main/team/docs/DEPLOY.md
