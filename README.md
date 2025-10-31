# Pokruk's Camera Mod

## Check out [the mod showcase on TikTok](https://www.tiktok.com/@pokrukgt/video/7427872588166073608) <br />
![Download (4)_5](https://github.com/user-attachments/assets/6e87dc89-a3bf-49af-80f7-8a9e3655374f)
![](https://private-user-images.githubusercontent.com/46229617/508195014-0af0fd28-0ab5-47e1-89c3-82c347f0daa8.gif?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjE5MTMyNjYsIm5iZiI6MTc2MTkxMjk2NiwicGF0aCI6Ii80NjIyOTYxNy81MDgxOTUwMTQtMGFmMGZkMjgtMGFiNS00N2UxLTg5YzMtODJjMzQ3ZjBkYWE4LmdpZj9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTEwMzElMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMDMxVDEyMTYwNlomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWIxODA3YWY3M2E3MjUwMjZjNmQzY2U0ODE1MmFhMGQ2MzEyNjgzNWJjNTQ5ZjkyODU3MDk3N2I1OWM2ODZkMzkmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.-VeK9i2UAM6bXqiPfIIEl1mBftS1bNnN2w9jQN4v9ow)

## Main Exclusive Features
* No camera delay, no more laggy cosmetics **glitching** on the camera.
* Settings save: smoothing, camera clipping, FOV — all that **automatically saves**, so you don't need to set it up every time you restart the game.
* Toggleable Head Cosmetics Hide feature for first person view camera
* Toggleable Roll Lock feature
* Adjustable camera activation bind.
* You can adjust camera bind settings at runtime, no need to restart the game.
* Spectating other players
* You can now switch spec mode to first person through the gui as well
* First person roll lock

## Controls
* Press your camera activation bind to spawn the camera in front of you (see the controls config guide below).
* Hold the grip on the banana handle to grab the camera.
* Press **Tab** on the IRL keyboard for monitor UI.
* Use **WASD/arrow keys** for freecam (also a toggle for the gamepad).

### How to Set Camera Activation Bind
1. Start the game with the mod running at least once.
2. Open the `YourGameRootFolder/CameraMod/Configs/Controls.json` config.
3. Set the `activateBind` to the aliases of the buttons you want:

| Alias | Generic meaning | Quest 2 controllers meaning |
|----------|----------|----------|
| RP   | Right Primary  | A |
| RS   | Right Secondary  | B |
| LP   | Left Primary   | X  |
| LS   | Left Secondary | Y  |
4. Save the file and watch controls change immediately (you don't need to restart the game).

### Examples
If you want to bind the camera activation to multiple buttons, set the config bind value like this:

`Controls.json`
```json
{
  "activateBind": "RP RS LP LS"
}
```
If you want to bind it to just the Right Primary button (which is A for Quest 2 controllers):

`Controls.json`
```json
{
  "activateBind": "RP"
}
```
And so on.

---
Camera Mod with in game UI!
### *Features:*
* Monitor UI (tab to enable, shitty OnGUI but im lazy so cope)
* Freecam with gamepad support
* Spectator with ajustable offset (and a toggle to control it with wasd)
* First Person View with smoothing
* Third Person View (like gtags default camera just smoother,misc page for settings, front/back and follow head rotation)
* Follow Player (Camera will look at and follow player, misc page for settings, minimum distance and speed)
* Grabbable Handles (you can only grab right side with right hand and left side with left hand)
* Green Screen (in city)
* Adjustable FOV, Nearclip and smoothing

# *Disclamers:*
* **This product is not affiliated with Gorilla Tag or Another Axiom LLC and is not endorsed or otherwise sponsored by Another Axiom LLC. Portions of the materials contained herein are property of Another Axiom LLC. ©2021 Another Axiom LLC.**
* **controls can be different depending what you're playing on(steamvr,oculuspcvr,index, etc.)**
