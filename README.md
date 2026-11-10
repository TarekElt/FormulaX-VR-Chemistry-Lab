# Formula X — Virtual Chemistry Laboratory

An interactive virtual reality chemistry simulation developed for **CIM423: Building Virtual Worlds** at the University of Miami.

## Project Overview

Formula X is a VR chemistry laboratory where players use natural hand tracking to complete a guided chemical experiment.

Players interact with laboratory equipment, physically grab chemical flasks, combine substances, activate a mixer, and initiate a final heating reaction.

## Features

- Meta Quest 3 hand tracking
- Physical chemical flask grabbing
- Interactive laboratory control buttons
- Chemical placement detection
- Dynamic liquid color changes
- Purple mixing and green heating particle effects
- Wrist-mounted instructions
- Custom Canva-designed VR interface
- Introduction, formula selection, and completion menus
- Restartable experiment

## Technology

| Technology | Purpose |
|---|---|
| Unity 2022.3.42f1 | Game engine |
| C# | Experiment and interaction logic |
| XR Interaction Toolkit 2.6.5 | VR interactions |
| XR Hands / OpenXR | Quest 3 hand tracking |
| Unity URP | Rendering |
| Particle System | Chemical reaction effects |
| Canva | Interface graphics |

## Source Code

The `Scripts` folder contains the main custom scripts:

- `LabManager.cs` — Experiment stages, chemical colors, and reaction effects
- `LabUI.cs` — Menus, instructions, and restart behavior
- `MixingZone.cs` — Chemical placement detection
- `FormulaXWristUI.cs` — Tracked wrist display and instruction switching

## Experiment Sequence

1. Start the experiment.
2. Select Formula X.
3. Grab and place Chemical A.
4. Grab and place Chemical B.
5. Activate the mixer.
6. Activate the heater.
7. Observe the final reaction and completion screen.

## Documentation and Demo

**Project Documentation:** [INSERT NOTION LINK]

**Video Walkthrough:** [INSERT VIDEO LINK]

**Meta Quest 3 APK:** [INSERT APK LINK]

## Assets and Credits

The simulation uses the Chemistry Lab Items Pack from the Unity Asset Store, Unity XR interaction components, and custom interface graphics created in Canva.

Third-party asset packages are not redistributed through this public source-code repository.

## Development Assistance

ChatGPT was used to assist with XR configuration, C# scripting, debugging, and implementation guidance.

## Author

Tarek Eltantawy

University of Miami — CIM423: Building Virtual Worlds
