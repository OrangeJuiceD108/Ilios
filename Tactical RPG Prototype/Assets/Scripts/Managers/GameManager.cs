using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This hunk of code is what makes this a singleton
    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
    }


    // Stores the gameState lol
    public GameState gameState;

    

    // Sets the gameState to generate grid so that the game can begin
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }



    // Function for moving between game states
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



// enum for each game state
public enum GameState
{
    GenerateGrid = 1,
    SpawnUnits = 2,
}