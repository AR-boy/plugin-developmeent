# plugin-development-unity
Unity project to develop project for Augmented Reality plugin used to develop Augmneted Reality Posters

# Documentation

# Demos folder
Demos folder inside Assets, contains demo levels that show how to use different features of the plugin for Windows and Android

# Prefabs
Prebfabs are stored insde Prefabs folder in Assets

## ARCamera
ARCamera is a Unity camera game object that allows the end-user of the application to observe what the camera can see. The prefab is set up with an initial z-axis position (500 Unity units away from the Background Pane). The ARCamera observes and displays any rendered GameObjects that exist between itself and the Background Pane. It provides the composite view that the device displays. The camera is translated at the start of the application to point at the centre of the background pane and it adjusts it’s FOV according to Background Pane’s size. If the 16:9 aspect ratio is preserved, the camera’s frustum will line up with the edges of the background plane, even when there is a change in z-value of the camera. Otherwise, the camera remains static in x and y axes.

## ARRoot
ARRoot is an empty game object which is used to transform all 3D AR-related game content. It serves as a hierarchical root and any game object that requires pose estimation should be placed inside it. Transformation in Unity work on hierarchies, therefore if a game object belongs to a hierarchy and the hierarchy’s ancestor is transformed, the transformation will also be applied to that game object. A collider was used to determine when the game object intersects with a background which can ruin perspective. To control this situation, control was implemented that checks if the object intersects the background and moves it forward in the z-direction while simultaneously moving the ARCamera forward by the same amount to preserve the scale. 

## BackgroundPane
The last prefab within the library is the BackgroundPane and this game object serves as the background which displays the input video from the camera and is positioned at a z value of 0. It is auto-sized according to camera resolution and has the same dimensions as the resolution in Unity’s world space units. Additionally, it has unlit texture material attached to the plane to ensure that light made by the game engine does not affect the texture’s colours.


# Scripts 

- Marshalling folder- contains all marshaller class scripts for different data types

- UnityAR folder - contains scripts that are attached to the aforementioned prefabs and provide the described functionality (The Utilities script provides methods for getting transformation values and saving camera calibration data)

- OpenCVInterop - files store all functionality that makes use of native plugin methods 


- DemoScripts folder - contains demo scripts which show the tool working and how to use its functionality 

- Python - contains two scripts to generate Aruco boards and Charuco boards
    - Aruco board generation script should be ran with the following arguments: number of wanted makers horizontally, number of wanted makers vertically, marker length in cm and marker seperation in cm
    - Charuco board generation script should be ran with the following arguments: number of wanted squares horizontally, number of wanted squares vertically, marker length in cm and sqaure length in cm (sqaures should be longer than markers)

