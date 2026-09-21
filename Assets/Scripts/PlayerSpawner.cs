using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    // Player stuff
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private NetworkRunner _runner;
    private PlayerControls _playerControls;
    private bool _jumpPressed;
    private bool _attackPressed;

    // Debugging 
    private bool _damagePlayer;
    private bool _healPlayer;

    // Enemy stuff
    public List<NetworkObject> _spawnedEnemies = new List<NetworkObject>();

    private void Awake() 
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        // Subscribing to the event directly because of weird behaviour
        _playerControls.Player.Jump.performed += _ => _jumpPressed = true;
        _playerControls.Player.Attack.performed += _ => _attackPressed = true;

        // Debugging
        _playerControls.Test.DamagePlayer.performed += _ => _damagePlayer = true;
        _playerControls.Test.HealPlayer.performed += _ => _healPlayer = true;
    }

    private void OnDestroy() 
    {
        _playerControls.Disable();
    }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();

        // Read from new Input System
        var move = _playerControls.Player.Move.ReadValue<Vector2>();
        data.Direction = new Vector3(move.x, 0f, move.y);
        data.Jump = _jumpPressed;
        data.Attack = _attackPressed;
        _jumpPressed = false;
        _attackPressed = false;

        // Debugging
        data.DamagePlayer = _damagePlayer;
        data.HealPlayer = _healPlayer;
        _damagePlayer = false;
        _healPlayer = false;

        input.Set(data);
    }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);
        DontDestroyOnLoad(gameObject);

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/SampleScene.unity"));
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    //private void OnGUI()
    //{
    //    if (_runner == null)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
    //        {
    //            StartGame(GameMode.Host);
    //        }

    //        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
    //        {
    //            StartGame(GameMode.Client);
    //        }
    //    }
    //}
    public void SpawnEnemy(NetworkPrefabRef enemy, Vector3 spawnPosition)
    {
        if (_runner.IsServer)
        {
            NetworkObject networkEnemyObject = _runner.Spawn(enemy, spawnPosition, Quaternion.identity, PlayerRef.None);
            _spawnedEnemies.Add(networkEnemyObject);
        }
    }

    public void SpawnItem(NetworkPrefabRef item, Vector3 spawnPosition)
    {

    }

    public void CallHostGame()
    {
        StartGame(GameMode.Host);
    }

    public void CallJoinGame()
    {
        StartGame(GameMode.Client);
    }
}