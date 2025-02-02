using FishNet;
using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameStateMananger;

public class ShooterGameController : NetworkBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<Transform> spawnPoints;

    [SerializeField] private int TotalRounds = 3;
    private static List<int> roundWins;

    private static ShooterGameController _instance;
    private List<NetworkObject> players;

    public static ShooterGameController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<ShooterGameController>();

            return _instance;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if(roundWins == null)
            roundWins = new List<int>();
        int spawnIndx = 0;
        var networkManager = InstanceFinder.NetworkManager;
        players = new List<NetworkObject>();
        foreach (var kv in InstanceFinder.NetworkManager.ServerManager.Clients)
        {
            NetworkObject nob = networkManager.GetPooledInstantiated(playerPrefab, spawnPoints[spawnIndx].position, spawnPoints[spawnIndx].rotation, true);
            networkManager.ServerManager.Spawn(nob, kv.Value);
            spawnIndx = (spawnIndx + 1) % spawnPoints.Count;
            players.Add(nob);
        }

        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        yield return new WaitForSeconds(0.5f);
        RPCShowRoundStartGUI(roundWins.Count + 1);
    }

    [ObserversRpc]
    void RPCShowRoundStartGUI(int roundNumber)
    {
        ShooterGUI.Instance?.ShowRound(roundNumber);
    }

    public void SetRoundWinServer(int connectionId)
    {
        if (GameStateMananger.Instance.CurrentState != GameState.Shooter)
            return;

        if (!InstanceFinder.NetworkManager.IsServer)
            return;

        if (roundWins == null)
            roundWins = new List<int>();

        roundWins.Add(connectionId);
        int won = roundWins.Count(n => n == connectionId);
        float needed = ((float)TotalRounds / 2);
        Debug.Log($"ROUND WINS: {won} / (Needed) {needed}");
        if (won > needed)
        {
            var mostRepeated = roundWins.GroupBy(n => n).OrderByDescending(g => g.Count()).FirstOrDefault();
            roundWins.Clear(); //reset this
            foreach(var player in players)
            {
                if(player != null)
                {
                    player.Despawn();
                }
            }

            UpdateWinnerInfoForClients(connectionId);
            HackyMemory.SetWinner(connectionId);
            StartCoroutine(DelayedAction(() =>
            {
                GameStateMananger.Instance.SwitchState(GameState.GameOver);
            }, 0.75f));
        }
        else
        {
            StartCoroutine(DelayedAction(() =>
            {
                GameStateMananger.Instance.SwitchState(GameState.Poker);
            }, 1.5f));
        }
    }

    IEnumerator DelayedAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    [ObserversRpc]
    private void UpdateWinnerInfoForClients(int id)
    {
        HackyMemory.SetWinner(id);
    }
}
