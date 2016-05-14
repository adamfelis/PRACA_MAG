using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Player_NetworkSetup : NetworkBehaviour
    {

        [SerializeField]
        private Camera FPSCharacterCam;
        [SerializeField]
        private AudioListener audioListener;

        private GameObject aircraftModel;

        // Use this for initialization
        public override void OnStartLocalPlayer()
        {
        }

        public override void PreStartClient()
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            GetComponent<Player_ID>().PlayerIdentitySet += OnPlayerIdentitySet;
            aircraftModel = Tags.FindGameObjectWithTagInParent(Tags.F15, name);
            //aircraftModel.SetActive(false);
        }

        private void OnPlayerIdentitySet()
        {
            var aircraftsController = GetComponent<AircraftsController>();
            aircraftsController.Initialized += AircraftsControllerOnInitialized;
            aircraftsController.enabled = true;
        }

        private void AircraftsControllerOnInitialized()
        {
            //aircraftModel.SetActive(true);
        }
    }
}
