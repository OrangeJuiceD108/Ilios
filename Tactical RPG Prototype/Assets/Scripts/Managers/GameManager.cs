using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // List of buttons so I can mess with them
    [SerializeField] List<Button> buttonList = new List<Button>();

    [SerializeField] GameObject victory;
    [SerializeField] GameObject defeat;

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



    // Activates & Deactivates the Buttons
    public void ActivateButtons()
    {
        buttonList.ForEach(delegate(Button button)
        {
            button.interactable = true;
        });
    }
    public void DeactivateButtons()
    {
        buttonList.ForEach(delegate(Button button)
        {
            button.interactable = false;
        });
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
                ChangeState(GameState.FirstPlayerTurn);
                break;
            case GameState.AllyTurn:
                // Following line is temp logic
                UnitManager.Instance.AllyMoveTurn();
                ChangeState(GameState.EnemyMoveTurn);
                break;
            case GameState.EnemyMoveTurn:
                UnitManager.Instance.EnemyMoveTurn();
                ChangeState(GameState.PlayerTurn);
                break;
            case GameState.PlayerTurn:
                UnitManager.Instance.StartPlayerTurn();
                break;
            case GameState.EnemyAttackTurn:
                // Following line is temp logic
                UnitManager.Instance.RunEnemyAttacks();
                if(!UnitManager.Instance.TryLoss())
                {
                    ChangeState(GameState.AllyTurn);
                }
                break;
            case GameState.FirstPlayerTurn:
                UnitManager.Instance.StartPlayerTurn();
                break;
            case GameState.Victory:
                UnitManager.Instance.ExhaustAll();
                victory.SetActive(true);
                if(SceneManager.GetActiveScene().name == "Scene_1")
                {
                    Invoke("LoadScene2", 5.0f);
                }
                break;
            case GameState.Defeat:
                UnitManager.Instance.ExhaustAll();
                defeat.SetActive(true);
                if(SceneManager.GetActiveScene().name == "Scene_1")
                {
                    Invoke("LoadScene1", 5.0f);
                }
                else if(SceneManager.GetActiveScene().name == "Scene_2")
                {
                    Invoke("LoadScene2", 5.0f);
                }
                break;
        }
    }

    void LoadScene2()
    {
        SceneManager.LoadScene("Scene_2", LoadSceneMode.Single);
    }
    void LoadScene1()
    {
        SceneManager.LoadScene("Scene_1", LoadSceneMode.Single);
    }
}



// enum for each game state
public enum GameState
{
    GenerateGrid = 1,
    SpawnUnits = 2,
    AllyTurn = 3,
    EnemyMoveTurn = 4,
    PlayerTurn = 5,
    EnemyAttackTurn = 6,
    FirstPlayerTurn = 7,
    Victory = 8,
    Defeat = 9,
}