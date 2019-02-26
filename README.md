# Assignment 1
> Kaiten Zushi

# Submission Details
**Author**: Matthew Chan (mac2474)
**Submission Date**: 02/26/2019
**Computer Platform**: macOS High Sierra 10.13.6
**Mobile Platform**: Android 9 (Pie) on Google Pixel
**Project Title**: Kaiten Zushi

# Directory Structure
```
.
├── Imported        # assets imported from the Unity Asset Store
├── Materials       # materials for prefabs 
├── Prefabs         # prefabs used in the scene (i.e. Sushi Plate, Chef, etc.)
├── Scenes          # contains only the main scene
├── Scripts         # contains all of the C# scripts
├── Sounds          # contains sound effects
```

# Usage
The game starts with the Settings menu opened up and the game paused. In this menu, you can modify the difficulty of the game (either Easy or Hard) and also manipulate the camera (if in Player Mode).

Once you close the Settings panel, the game starts.

The chef spawns 3 different types of dishes on the conveyor belt:
1. Sushi plates
2. Dessert plates
3. Special plates

There are 4 tables, each that can hold a max of 4 dishes on them. When dishes are served to the table, they slowly disappear and are eventually consumed by the customers.

If a dish falls to the ground, completes a full round around the conveyor belt, crashes into another plate, or stays on the belt too long it breaks (and disappears in a poof of white smoke).

## Video Demo
Video demonstration can be found [here](https://youtu.be/gY21lXMKP2k).

# Missing features
All of the required features for this assignment have been implemented.

# Bugs
Previously had a bug with UI elements not blocking raycasts from touch input on my Android device, but this has since been fixed. No other bugs found at the moment.

# Asset Sources
- [FREE Food Pack](https://www.assetstore.unity3d.com/en/?stay#!/content/85884)
- [White Smoke Particle System](https://www.assetstore.unity3d.com/en/?stay#!/content/20404)
- [Kitchen Props Free](https://www.assetstore.unity3d.com/en/?stay#!/content/80208)
