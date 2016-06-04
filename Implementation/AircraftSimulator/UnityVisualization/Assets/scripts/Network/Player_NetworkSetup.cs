using System;
using Assets.scripts;
using Assets.scripts.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Player_NetworkSetup : NetworkBehaviour
    {
        // Use this for initialization
        public override void OnStartLocalPlayer()
        {
            //GameObject.FindGameObjectWithTag(Tags.EnvironmentCreator).GetComponent<GameController>().enabled = true;
        }

        public override void PreStartClient()
        {
            GetComponent<Player_ID>().PlayerIdentitySet += OnPlayerIdentitySet;
            GetComponent<Player_ID>().enabled = true;
        }

        public void OnPlayerIdentitySet()
        {
            var aircraftsController = GetComponent<AircraftsController>();
            aircraftsController.Initialize();
            aircraftsController.enabled = true;
            AircraftsControllerOnInitialized();
        }

        private MissileController getLocalMissileController()
        {
            var localPlayer = GetComponent<Player_ID>().GetLocalPlayer();
            return localPlayer.GetComponent<MissileController>();
        }

        private CameraSmoothFollow getLocalCameraSmoothFollow()
        {
            var localPlayerName = GetComponent<Player_ID>().GetLocalPlayerName();
            return Tags.FindGameObjectWithTagInParent(Tags.CameraManager, localPlayerName).GetComponent<CameraSmoothFollow>();
        }

        private void AircraftsControllerOnInitialized()
        {
            //Debug.Log("on controller initialized");
            var aircraftsController = GetComponent<AircraftsController>();
            if (isLocalPlayer)
            {
                GetComponent<BoxCollider>().enabled = true;

                var cameraManager = Tags.FindGameObjectWithTagInParent(Tags.CameraManager, name);
                cameraManager.GetComponent<CameraSmoothFollow>().enabled = true;
                cameraManager.GetComponent<CameraSmoothFollow>().Initialize();

                var mainCamera = Tags.FindGameObjectWithTagInParent(Tags.MainCamera, name);
                mainCamera.GetComponent<Camera>().enabled = true;

                var miniMap = GameObject.FindGameObjectWithTag(Tags.MiniMap);
                miniMap.GetComponent<MapCanvasController>().AircraftsController = aircraftsController;
                miniMap.GetComponent<MapCanvasController>().enabled = true;

                var networkController =
                    GameObject.FindGameObjectWithTag(Tags.ApplicationManager).GetComponent<NetworkController>();
                networkController.InitializeUnlocalPlayers(getLocalMissileController(), getLocalCameraSmoothFollow());
            }
            //if it is not a local player
            else
            {
                var networkController =
                   GameObject.FindGameObjectWithTag(Tags.ApplicationManager).GetComponent<NetworkController>();
                if (networkController.IsLocalPlayerInitialized)
                {
                    networkController.InitializeSingleUnlocalPlayer(gameObject, getLocalMissileController(), getLocalCameraSmoothFollow());
                }
                else
                {
                    networkController.UnlocalPlayers.Add(gameObject);
                }
            }
        }
    }
}
