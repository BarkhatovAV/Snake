using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrafab;
    [SerializeField] private int _detailCount;

    private Controller _controller;
    private Snake _snake;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(_snake)
            {
                _snake.Destroy();
                Destroy(_controller);
            }



            _snake = Instantiate(_snakePrafab);
            _snake.Init(_detailCount);
            _controller = Instantiate(_controllerPrefab);
            _controller.Init(_snake);
        }
    }
}
