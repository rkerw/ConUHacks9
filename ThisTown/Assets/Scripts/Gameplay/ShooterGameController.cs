using FishNet;
using FishNet.Managing;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShooterGameController : NetworkBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<Transform> spawnPoints;


    public override void OnStartServer()
    {
        base.OnStartServer();
        int spawnIndx = 0;
        var networkManager = InstanceFinder.NetworkManager;
        foreach (var kv in InstanceFinder.NetworkManager.ServerManager.Clients)
        {
            NetworkObject nob = networkManager.GetPooledInstantiated(playerPrefab, spawnPoints[spawnIndx].position, spawnPoints[spawnIndx].rotation, true);
            networkManager.ServerManager.Spawn(nob, kv.Value);
            spawnIndx = (spawnIndx + 1) % spawnPoints.Count;
        }
    }
}
