using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraModExample {

    #region BepInPlugin Notes
    // The BepInPlugin attribute in [brackets] is setting parameters for bepin to load your mod

    // GUID: "TheTimeSweeper.CameraModExample"
    // The the identifier for your mod. This one simply follows the convention of "AuthorName.YourModName"

    // Name: "CameraModExample"
    // The the full human-readable name of your mod.

    // Version: "0.1.0"
    // The Version, as you'd expect, useful for differing between updates.
    //     Customary to follow Semantic Versioning (major.minor.patch). 
    //         You don't have to, but you'll just look silly in front of everyone. It's ok. I won't make fun of you.
    #endregion
    [BepInPlugin("TheTimeSweeper.CameraModExample", "CameraModExample", "0.1.0")]
    public class CameraModPlugin : BaseUnityPlugin {
        #region BaseUnityPlugin Notes
        // BaseUnityPlugin is the main class that gets loaded by bepin.
        // It inherits from MonoBehaviour, so it gains all the familiar Unity callback functions you can use: 
        //     Awake, Start, Update, FixedUpdate, etc.

        //     Awake is most important. it's basically where we initialize everything we do

        //     For further reading, you can check out https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

        // now, close these two Notes regions so the script looks little nicer to work with 
        #endregion

        // Just declaring a variable we'll use later
        private bool hasZoomedCamera;

        // This Awake() function will run at the very start when the mod is initialized
        void Awake() {

            // This is the just a first little tester code to see if our mod is running on WoL. You'll see it in the BepInEx console
            Debug.Log("I belive in you c:");
            
            // These are hooks
            // They hook on to the functions in the game. When the hooked functon is called, our functon will be called
            On.CameraController.Awake += CameraController_Awake;
            On.CameraController.Update += CameraController_Update;
        }

        // This Update() function will run every frame
        void Update() {

            // Every frame, we'll check if the user is currently pressing f1 on the keyboard
            if (Input.GetKeyDown(KeyCode.F1)) {

                //if they just pressed f1 this frame, let's zoom the camera out
                CameraController.originalCameraSize += 0.5f;
                Debug.Log("zooming the camera out to " + CameraController.originalCameraSize);
            }

            // same as above, but pressing f2 to zoom in
            if (Input.GetKeyDown(KeyCode.F2)) {

                CameraController.originalCameraSize -= 0.5f;
                Debug.Log("zooming the camera in to " + CameraController.originalCameraSize);
            }
        }

        // Here, we'll hook on to the CameraController's Awake function, to mess with things when it initializes.
        private void CameraController_Awake(On.CameraController.orig_Awake orig, CameraController self) {

            // This orig() line is very important. this is executing the original function we've hooked to.
            // If you don't call this orig() function, the game's original function will not run 
            orig(self);

            // After the camera initalizes, I'm swoocing right in and increasing these values so the players can run away from each other a huge distance (instead of being stuck at the camera border)
            // 'self' refers to the current reference of the CameraController that's calling this function, so we use that to mess with its current variables.
            self.maxHorizontalDistBetweenPlayers = 100;
            self.maxVerticalDistBetweenPlayers = 100;
            self.teleportToOtherPlayerRange = 120;

            // Just Zooming out the camera a little bit by default cause I want that
            if (!hasZoomedCamera) {
                //But only need to do it once
                hasZoomedCamera = true;

                CameraController.originalCameraSize *= 1.2f; //Change this value to whatever you like!
            }
        }

        // Here, we're hooking on the Update function. This code runs every frame on that CameraController
        private void CameraController_Update(On.CameraController.orig_Update orig, CameraController self) {

            orig(self);

            if (self.overrideCameraUpdate)
                return;
            if (GameController.playerScripts[0] == null || GameController.playerScripts[1] == null)
                return;
            if (GameController.coopOn && GameController.PlayerIsDead())
                return;

            //Copied from CameraController.Update in dnSpy:
            //if (GameController.pvpOn || SceneManager.GetActiveScene().name.Contains("PvP"))

            // This the bit of code ordinarly runs in the 'if' statement above (checks are we pvp right now).
            // It grabs the distance between players, and uses it to set the camera size if the players are far enough away from each other
            self.distanceBetweenPlayers = self.playerDiff.magnitude * 0.42f;
            Camera.main.orthographicSize = (self.distanceBetweenPlayers <= CameraController.originalCameraSize) ? CameraController.originalCameraSize : self.distanceBetweenPlayers;

            // So for this hook i'm simply doing it again, but without the pvp check, so it always does it.

            // And we're done! Build this mod, put it in Plugins, and run the game!
        }
    }
}
