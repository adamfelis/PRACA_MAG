using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using Assets.scripts.Model;

public class NetworkController : MonoBehaviour
{

    public IList<GameObject> UnlocalPlayers { get; set; }
    public bool IsLocalPlayerInitialized { get; set; }

    void Start()
    {
        UnlocalPlayers = new List<GameObject>();
    }

    public void InitializeUnlocalPlayers(MissileController missileController, CameraSmoothFollow cameraSmoothFollow)
    {
        foreach (var unlocalPlayer in UnlocalPlayers)
        {
            InitializeSingleUnlocalPlayer(unlocalPlayer, missileController, cameraSmoothFollow);
        }
        IsLocalPlayerInitialized = true;
    }

    public void InitializeSingleUnlocalPlayer(GameObject unlocalPlayer, MissileController missileController, CameraSmoothFollow cameraSmoothFollow)
    {
        var rendererDetector = Tags.FindGameObjectWithTagInParent(Tags.WingsMain, unlocalPlayer.name).AddComponent<RendererDetector>();
        rendererDetector.Initialize(cameraSmoothFollow);
        var localMissileController = missileController;
        rendererDetector.EnemyBecameVisible += localMissileController.OnEnemyBecameVisible;
        rendererDetector.EnemyBecameInvisible += localMissileController.OnEnemyBecameInvisible;

        unlocalPlayer.GetComponent<MapMarker>().enabled = true;
    }

}
