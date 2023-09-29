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
    #endregion

    #region Enemy

    private void CreateEnemy(string key, Player player)
    {
    }

    private void RemoveEnemy(string key, Player player)
    {
    }
    #endregion

    #region Player

    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrafab;

    private void CreatePlayer(Player player)
    {
        Snake snake = Instantiate(_snakePrafab);
        snake.Init(player.d);
        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(snake);
    }
    #endregion
}
