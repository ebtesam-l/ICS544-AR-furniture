# AR Furniture Placement App

A mobile AR app built with Unity that lets you place and arrange 3D furniture in your real room using your phone camera.

---

## What the App Does

- Detects your floor using AR
- Place virtual sofas in your real room
- Switch between 3 different sofa styles
- Scale, rotate, and move furniture after placing it

---

## Requirements

- Unity 6000.0.74f1
- iPhone with iOS 13 or later (requires ARKit)
- Xcode (to build and install on iPhone)
- Mac computer (required for iOS builds)

---

## How to Open the Project

1. Open **Unity Hub**
2. Click **Add** and select the project folder `ar-project-v2`
3. Make sure Unity version **6000.0.74f1** is installed
4. Click the project to open it

---

## How to Build and Run on iPhone

1. In Unity, go to **File → Build Settings**
2. Select **iOS** as the platform
3. Click **Switch Platform**
4. Click **Build** and choose a folder to save the Xcode project
5. Open the generated `.xcodeproj` file in **Xcode**
6. Connect your iPhone
7. Select your device in Xcode and click the **Play** button to install

> Make sure you have an Apple Developer account set up in Xcode under Signing & Capabilities.

---

## How to Use the App

### Step 1 — Splash Screen
When the app opens you will see the splash screen. Wait 2 seconds for the camera to initialize, then tap **Start Scanning**.

### Step 2 — Scan the Floor
Point your camera at the floor and slowly move it around. A black outline will appear showing the detected surface.

### Step 3 — Choose a Sofa
Tap one of the 3 sofa buttons at the bottom of the screen to select which furniture you want to place.

### Step 4 — Place the Furniture
Tap anywhere on the detected floor to place the sofa there.

### Step 5 — Edit the Furniture
After placing, a control panel appears with 3 options:

| Button | What it does |
|--------|--------------|
| - / +  | Make the sofa smaller or bigger |
| < / >  | Rotate the sofa left or right |
| Move   | Tap the floor again to move the sofa to a new spot |

### Step 6 — Switch Furniture
Tap any sofa button at the bottom to remove the current sofa and place a different one.

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── FurniturePlacementManager.cs   — handles AR placement, buttons, scale/rotate/move
│   └── SplashScreenManager.cs         — controls splash screen and camera startup
├── Assets/
│   └── Furnitures/
│       ├── White Sofa/
│       ├── Light Brown Sofa/
│       └── Brown Sofa/
└── Editor/
    ├── BuildTransformPanel.cs         — editor tool to build the control panel UI
    └── BuildSplashScreen.cs           — editor tool to build the splash screen UI
```

---

## Scripts Overview

### FurniturePlacementManager.cs
Attached to the **XR Origin** GameObject. Handles everything:
- Detecting floor taps using AR raycasting
- Spawning the selected furniture prefab
- Scaling, rotating, and moving the placed object
- Showing/hiding the transform control panel

### SplashScreenManager.cs
Attached to the **Canvas** GameObject. Controls the app flow:
- Shows the splash screen on launch
- Waits 2 seconds for AR to initialize
- When Start Scanning is tapped, hides the splash and shows the furniture buttons

---

## Common Issues

**Black screen on launch** — This is normal. The app waits 2 seconds for the AR camera to initialize.

**Floor not detected** — Make sure you are in a well-lit room and slowly move the camera across the floor.

**Furniture looks pink** — Materials are using the wrong shader. All materials should use Universal Render Pipeline/Lit.

**Buttons not working** — Make sure the FurniturePlacementManager on XR Origin has all prefab fields assigned in the Inspector.

---

## Built With

- Unity — Game engine
- AR Foundation — Cross-platform AR framework
- ARKit — Apple AR SDK for iPhone
- Universal Render Pipeline (URP) — Rendering pipeline
