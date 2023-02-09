using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                ChangeState(GameState.SpawnUnits);
                break;
            case GameState.SpawnUnits:
                UnitManager.Instance.GenerateUnits();
                break;
        }
    }
}

public enum GameState
{
    GenerateGrid = 1,
    SpawnUnits = 2,
}