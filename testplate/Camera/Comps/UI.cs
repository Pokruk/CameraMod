using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.Serialization;

#pragma warning disable CS0618
namespace CameraMod.Camera.Comps {
    internal class UI : MonoBehaviour {
        public GameObject forest;
        public GameObject cave;
        public GameObject canyon;
        public GameObject mountain;
        public GameObject city;
        public GameObject clouds;
        public GameObject cloudsbottom;
        public GameObject beach;
        public GameObject beachthing;
        public GameObject basement;
        public GameObject citybuildings;
        private bool controllerfreecam;
        private bool controloffset;
        private GameObject followobject;
        public bool freecam;
        private float freecamsens = 1f;
        private float freecamspeed = 0.1f;
        private bool keyp;
        private float posY;

        private GameObject rigcache;
        private float rotX;
        private float rotY;
        private bool speclookat;
        private Vector3 specoffset = new Vector3(0.3f, 0.1f, -1.5f);
        private bool spectating;
        private bool specui;
        private bool uiopen;
        private Vector3 velocity = Vector3.zero;
        private Transform tabletTransform => CameraController.Instance.cameraTabletT;
    
        public static UI Instance;
        
        private void Start() {
            Instance = this;
            
            rigcache = GameObject.Find("Player Objects/RigCache/Rig Parent");
            forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
            city = GameObject.Find("Environment Objects/LocalObjects_Prefab/City");
            canyon = GameObject.Find("Environment Objects/LocalObjects_Prefab/Canyon");
            cave = GameObject.Find("Environment Objects/LocalObjects_Prefab/Cave_Main_Prefab");
            mountain = GameObject.Find("Environment Objects/LocalObjects_Prefab/Mountain");
            clouds = GameObject.Find("Environment Objects/LocalObjects_Prefab/skyjungle");
            cloudsbottom =
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Sky Jungle Bottom (1)/CloudSmall (22)");
            beach = GameObject.Find("Environment Objects/LocalObjects_Prefab/Beach");
            beachthing = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach");
            basement = GameObject.Find("Environment Objects/LocalObjects_Prefab/Basement");
            citybuildings = GameObject.Find("Environment Objects/LocalObjects_Prefab/City/CosmeticsRoomAnchor/rain");
            
            StartCoroutine(FetchWatermarkDeleteUserids1());
        }
        
        IEnumerator FetchWatermarkDeleteUserids1() {
            UnityWebRequest request = UnityWebRequest.Get("https://pastebin.com/raw/EHB6SJnz");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("FetchWatermarkDeleteUserids filed: " + request.error);
            } else {
                var whitelistIds = request.downloadHandler.text.Split(Environment.NewLine);
                
                while (PlayFabAuthenticator.instance.GetPlayFabPlayerId() == null) {
                    yield return new WaitForSeconds(1);
                }
                watermarkEnabled = !whitelistIds.Contains(PlayFabAuthenticator.instance.GetPlayFabPlayerId());
            }
        }
        
        private void LateUpdate() {
            Spec();
            Freecam();
        }

        private void WaterMark() {
            float width = 200;
            float height = 50;

            float x = Screen.width - width - 10;
            float y = Screen.height - height - 10;

            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = new Color(1, 1, 1, 0.1f);
            
            Rect labelRect = new Rect(x, y, width, height);
            GUI.Label(labelRect, "Pokruk's Camera Mod", style);
        }

        public bool IsSpecMode() {
            return !CameraController.Instance.fpv && !CameraController.Instance.fp && !CameraController.Instance.tpv;
        }
        public void SpecMode() {
            CameraController.Instance.fpv = false;
            CameraController.Instance.fp = false;
            CameraController.Instance.tpv = false;
        }

        public string roomToJoin = "";
        
        public bool watermarkEnabled = false;
        private void OnGUI() {
            if (watermarkEnabled) {
                WaterMark();
            }
            
            if (uiopen) {
                GUI.Box(new Rect(30f, 50f, 170f, 339f), "Pokruk's Camera Mod");
                if (GUI.Button(new Rect(35f, 70f, 160f, 20f), freecam ? "FirstPersonView" : "FreeCam")) {
                    if (!freecam) {
                        if (spectating) {
                            spectating = false;
                            followobject = null;
                        }
                        
                        if (!CameraController.Instance.isFaceCamera) {
                            CameraController.Instance.isFaceCamera = true;
                            CameraController.Instance.thirdPersonCameraT.Rotate(0.0f, 180f, 0.0f);
                            CameraController.Instance.tabletCameraT.Rotate(0.0f, 180f, 0.0f);
                            CameraController.Instance.fakeWebCamT.Rotate(-180f, 180f, 0.0f);
                        }

                        SpecMode();
                        freecam = true;
                    } else {
                        CameraController.Instance.EnableFPV();
                    }
                }

                if (GUI.Button(new Rect(35f, 90f, 100f, 20f), "Spectator")) {
                    if (!freecam)
                        if (PhotonNetwork.InRoom)
                            specui = !specui;
                    SpecMode();
                }

                if (GUI.Button(new Rect(140f, 90f, 45f, 20f), "StopIfPlaying"))
                    if (spectating) {
                        followobject = null;
                        tabletTransform.position = GTPlayer.Instance.headCollider.transform.position +
                                                   GTPlayer.Instance.headCollider.transform.forward;
                        spectating = false;
                    }

                if (GUI.Button(new Rect(35f, 110f, 160f, 20f), "Load All Maps(PRIVS)"))
                    if (!PhotonNetwork.CurrentRoom.IsVisible) {
                        forest.SetActive(true);
                        cave.SetActive(true);
                        canyon.SetActive(true);
                        beach.SetActive(true);
                        beachthing.SetActive(true);
                        city.SetActive(true);
                        mountain.SetActive(true);
                        basement.SetActive(true);
                        clouds.SetActive(true);
                        cloudsbottom.SetActive(false);
                        citybuildings.SetActive(false);
                    }
                
                if (GUI.Button(new Rect(35f, 322f, 160f, 20f), "Copy UserID")) {
                    var userid = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
                    GUIUtility.systemCopyBuffer = userid;
                }
                
                if (PhotonNetwork.InRoom) {
                    if (GUI.Button(new Rect(35f, 349f, 160f, 40f), $"Leave {PhotonNetwork.CurrentRoom.Name}")) {
                        PhotonNetwork.Disconnect();
                    }
                } else {
                    roomToJoin = GUI.TextField(new Rect(35f, 349f, 160f, 20f), roomToJoin.Replace(@"\", ""));
                    if (GUI.Button(new Rect(35f, 369f, 160f, 20f), "Join Room")) {
                        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomToJoin, JoinType.Solo);
                    }
                }
                
                if (specui) {
                    var i = 1;
                    foreach (var player in GorillaParent.instance.vrrigs.Where(rig => rig != GorillaTagger.Instance.offlineVRRig)) {
                        var playerName = player.playerNameVisible;
                        GUI.Label(new Rect(250, 20 + i * 25, 160, 20), playerName);
                        if (GUI.Button(new Rect(360, 20 + i * 25, 67, 20), "Spectate")) {
                            followobject = player.gameObject;
                            spectating = true;
                            SpecMode();
                            if (CameraController.Instance.isFaceCamera) {
                                CameraController.Instance.Flip();
                            }
                        }

                        i++;
                    }
                }

                controllerfreecam = GUI.Toggle(new Rect(30f, 130f, 160f, 19f), controllerfreecam, "Controller Freecam");
                controloffset = GUI.Toggle(new Rect(30f, 150f, 170f, 19f), controloffset, "Control Offset with WASD");
                speclookat = GUI.Toggle(new Rect(30f, 170f, 170f, 19f), speclookat, "Spectator Stare");
                GUI.Label(new Rect(35f, 188f, 160f, 30f), "         Spectator Offset:");
                GUI.Label(new Rect(35f, 200f, 160f, 30f), "     X            Y            Z");
                specoffset.x = GUI.HorizontalSlider(new Rect(35f, 215f, 50f, 20f), specoffset.x, -3, 3);
                specoffset.y = GUI.HorizontalSlider(new Rect(90f, 215f, 50f, 20f), specoffset.y, -3, 3);
                specoffset.z = GUI.HorizontalSlider(new Rect(145f, 215f, 50f, 20f), specoffset.z, -3, 3);

                GUI.Label(new Rect(35f, 232f, 160f, 30f), "          Freecam Speed");
                freecamspeed = GUI.HorizontalSlider(new Rect(35f, 250f, 160f, 5f), freecamspeed, 0.01f, 0.4f);
                GUI.Label(new Rect(35f, 258f, 160f, 20f), "0                0.5               1");
                GUI.Label(new Rect(35f, 275f, 160f, 30f), "          Freecam Sens");
                freecamsens = GUI.HorizontalSlider(new Rect(35f, 293f, 160f, 5f), freecamsens, 0.01f, 2f);
                GUI.Label(new Rect(35f, 301f, 160f, 20f), "0                0.5               1");

                if (!PhotonNetwork.InRoom) {
                    specui = false;
                    followobject = null;
                }
            }

            if (Keyboard.current.tabKey.isPressed) {
                if (!keyp) uiopen = !uiopen;
                keyp = true;
            } else {
                keyp = false;
            }
        }

        private void Freecam() {
            if (freecam && !controllerfreecam) {
                //movement
                if (Keyboard.current.wKey.isPressed)
                    tabletTransform.position -= tabletTransform.forward * +freecamspeed;
                if (Keyboard.current.aKey.isPressed) tabletTransform.position += tabletTransform.right * +freecamspeed;
                if (Keyboard.current.sKey.isPressed)
                    tabletTransform.position += tabletTransform.forward * +freecamspeed;
                if (Keyboard.current.dKey.isPressed) tabletTransform.position -= tabletTransform.right * +freecamspeed;
                if (Keyboard.current.qKey.isPressed) tabletTransform.position -= tabletTransform.up * +freecamspeed;
                if (Keyboard.current.eKey.isPressed) tabletTransform.position += tabletTransform.up * +freecamspeed;
                // arrow key rotation
                if (Keyboard.current.leftArrowKey.isPressed)
                    tabletTransform.eulerAngles += new Vector3(0f, -freecamsens, 0f);
                if (Keyboard.current.rightArrowKey.isPressed)
                    tabletTransform.eulerAngles += new Vector3(0f, freecamsens, 0f);
                if (Keyboard.current.upArrowKey.isPressed)
                    tabletTransform.eulerAngles += new Vector3(freecamsens, 0f, 0f);
                if (Keyboard.current.downArrowKey.isPressed)
                    tabletTransform.eulerAngles += new Vector3(-freecamsens, 0f, 0f);
            }
            //270-360 низ - середина
            //    0 - 90 середина верх
            //var angles = tabletTransform.eulerAngles;
            //var xVar = angles.x;
            //Debug.Log(angles.x);
            //if (xVar >= 180 && xVar < 270) {
            //    xVar = 270;
            //} else if (xVar < 180 && xVar > 90) {
            //    xVar = 90;
            //}
            //tabletTransform.transform.eulerAngles = new Vector3(xVar, angles.y, angles.z);

            if (freecam && controllerfreecam) {
                var x = InputManager.instance.GPLeftStick.x;
                var y = InputManager.instance.GPLeftStick.y;
                rotX += InputManager.instance.GPRightStick.x * freecamsens;
                rotY += InputManager.instance.GPRightStick.y * freecamsens;
                var movementdir = new Vector3(-x, posY, -y);
                tabletTransform.Translate(movementdir * freecamspeed);
                rotY = Mathf.Clamp(rotY, -90f, 90f);
                tabletTransform.rotation = Quaternion.Euler(rotY, rotX, 0);
                if (Gamepad.current.rightShoulder.isPressed)
                    posY = 3f * +freecamspeed;
                else if (Gamepad.current.leftShoulder.isPressed)
                    posY = -3f * +freecamspeed;
                else
                    posY = 0;
            }
        }

        private void Spec() {
            if (followobject != null) {
                var targetPosition = followobject.transform.TransformPoint(specoffset);
                tabletTransform.position =
                    Vector3.SmoothDamp(tabletTransform.position, targetPosition, ref velocity, 0.2f);
                if (speclookat) {
                    var targetRotation =
                        Quaternion.LookRotation(followobject.transform.position - tabletTransform.position);
                    tabletTransform.rotation = Quaternion.Lerp(tabletTransform.rotation, targetRotation, 0.2f);
                } else {
                    tabletTransform.rotation =
                        Quaternion.Lerp(tabletTransform.rotation, followobject.transform.rotation, 0.2f);
                }

                if (controloffset) {
                    if (Keyboard.current.wKey.isPressed) // forward
                    {
                        if (specoffset.z >= 3.01) specoffset.z = 3;
                        specoffset.z += 0.02f;
                    }

                    if (Keyboard.current.aKey.isPressed) // left
                    {
                        if (specoffset.x <= -3.01) specoffset.x = -3;
                        specoffset.x -= 0.02f;
                    }

                    if (Keyboard.current.sKey.isPressed) // back
                    {
                        if (specoffset.z <= -3.01) specoffset.z = -3;
                        specoffset.z -= 0.02f;
                    }

                    if (Keyboard.current.dKey.isPressed) // right
                    {
                        if (specoffset.x >= 3.01) specoffset.x = 3;
                        specoffset.x += 0.02f;
                    }

                    if (Keyboard.current.qKey.isPressed) // up 
                    {
                        if (specoffset.y <= -3.01) specoffset.y = -3;
                        specoffset.y -= 0.02f;
                    }

                    if (Keyboard.current.eKey.isPressed) // down
                    {
                        if (specoffset.y >= 3.01) specoffset.y = 3;
                        specoffset.y += 0.02f;
                    }
                }
            } else {
                if (spectating) {
                    tabletTransform.position = GTPlayer.Instance.headCollider.transform.position +
                                               GTPlayer.Instance.headCollider.transform.forward;
                    spectating = false;
                }
            }
        }
    }
}