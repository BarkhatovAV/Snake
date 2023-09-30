using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System;
using Colyseus.Schema;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    #region Server
    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
        Connection();
    }

    private async void Connection()
    {
        _room = await client.JoinOrCreate<State>(GameRoomName);
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;
        _room.OnStateChange -= OnChange;

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;

    }

    protected  override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        _room?.Leave();

    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
    #endregion

    #region Enemy

    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    private void CreateEnemy(string key, Player player)
    {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Snake snake = Instantiate(_snakePrafab, position, Quaternion.identity);
        snake.Init(player.d);

        EnemyController enemy = snake.gameObject.AddComponent<EnemyController>();
        enemy.Init(player, snake);

        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player player)
    {
        if(_enemies.ContainsKey(key) == false)
        {
            Debug.LogError("Try remove enemy");
        }

        EnemyController enemy = _enemies[key];
        _enemies.Remove(key);
        enemy.Destroy();

    }
    #endregion

    #region Player

    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrafab;

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Snake snake = Instantiate(_snakePrafab, position, Quaternion.identity);
        snake.Init(player.d);
        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(snake);
    }
    #endregion
}
