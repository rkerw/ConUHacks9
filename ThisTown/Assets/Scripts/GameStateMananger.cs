using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMananger : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Poker,
        Shooter,
        GameOver
    }

    private const int MIN_PLAYERS_NEEDED = 2;
    private const string MAIN_MENU_SCENE = "GameMenu";
    private const string POKER_GAME_SCENE = "PokerScene";
    private const string SHOOTER_GAME_SCENE = "ShooterScene";
    private const string GAMEOVER_SCENE = "GameOver";


    private static GameStateMananger _instance;
    private List<int> players;

    public static GameStateMananger Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GameStateMananger>();

            return _instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }


    public void StartGameAsServer(string ipAddr)
    {
        
        if (!InstanceFinder.NetworkManager.ServerManager.Started)
        {
            InstanceFinder.NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnection;
            if (InstanceFinder.NetworkManager.ServerManager.StartConnection())
            {
                StartGameAsClient(ipAddr);
            }
        }
    }

    public void StartGameAsClient(string ipAddr)
    {
        FindFirstObjectByType<Tugboat>()?.SetClientAddress(ipAddr);
        FindFirstObjectByType<Tugboat>()?.SetPort(7770);
        if (!InstanceFinder.NetworkManager.ClientManager.Started)
            InstanceFinder.NetworkManager.ClientManager.StartConnection();
    }

    public void OnRemoteConnection(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        if (players == null)
            players = new List<int>();

        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            if (!players.Contains(args.ConnectionId))
                players.Add(args.ConnectionId);
        }
        else if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            if (players.Contains(args.ConnectionId))
               players.Remove(args.ConnectionId);
        }


        if (players.Count < MIN_PLAYERS_NEEDED)
            return;

        SwitchState(GameState.Poker);

    }

    void LoadSceneNetworked(string sceneName)
    {
        if (!InstanceFinder.NetworkManager.IsServer)
            return;

        var data = new SceneLoadData(sceneName);
        data.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.NetworkManager.SceneManager.LoadGlobalScenes(data);
    }

    public void SwitchState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;

            case GameState.Poker:
                LoadSceneNetworked(POKER_GAME_SCENE);
                break;
            case GameState.Shooter:
                LoadSceneNetworked(SHOOTER_GAME_SCENE);
                break;
            case GameState.GameOver:
                LoadSceneNetworked(GAMEOVER_SCENE);
                break;
        }
    }

    private void LoadMainMenu()
    {
        if(InstanceFinder.NetworkManager.ServerManager.Started)
        {
            InstanceFinder.NetworkManager.ServerManager.StopConnection(true);
        }

        if (InstanceFinder.NetworkManager.ClientManager.Started)
        {
            InstanceFinder.NetworkManager.ClientManager.StopConnection();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

}
