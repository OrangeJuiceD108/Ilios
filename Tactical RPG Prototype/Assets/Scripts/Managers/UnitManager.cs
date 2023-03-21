using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // This hunk of code makes this a singleton
    public static UnitManager Instance;
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] int allyDeaths;
    Unit aeneas;

    // Contains the list of unit prefabs
    public GameObject[] playerUnitPrefabs;
    public GameObject[] enemyUnitPrefabs;
    public GameObject[] alliedUnitPrefabs;

    // The unit currently selected by the player
    [SerializeField] private Unit selectedUnit;

    // Lists of each type of unit
    [SerializeField] private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();
    private List<Unit> alliedUnits = new List<Unit>();

    // Array of units and their positions that will be added when Generate units is called
    /*  Uses a 4 digit code, explained as follows 
        Digits 1 and 2:     The position x
        Digits 3 and 4:     The position y
        Digit 5:            The index of the unit in its array plus one
        Digit 6:            The array the unit is contained in, 1 = player, 2 = enemy, 3 = ally
    */
    private List<int> plannedUnits = new List<int>();

    // This function spawns the units, right now most of the functionality is not there, this function's code will probably get replaced
    public void GenerateUnits()
    {
        foreach(int i in plannedUnits)
        {
            int code = i;
            Unit newUnit;
            if(code % 10 == 1)
            {
                code /= 10;
                newUnit = Instantiate(playerUnitPrefabs[(code % 10)]).GetComponent<Unit>();
                playerUnits.Add(newUnit);
                if(code % 10 == 0)
                {
                    aeneas = newUnit;
                }
            }
            else if(i % 10 == 2)
            {
                code /= 10;
                newUnit = Instantiate(enemyUnitPrefabs[(code % 10)]).GetComponent<Unit>();
                enemyUnits.Add(newUnit);
            }
            else
            {
                code /= 10;
                newUnit = Instantiate(alliedUnitPrefabs[(code % 10)]).GetComponent<Unit>();
                alliedUnits.Add(newUnit);
            }
            code /= 10;
            Tile startTile = GridManager.Instance.GetTile(new Vector2(code / 100, code % 100));
            UpdatePosition(newUnit, startTile);
            newUnit.Init();
        }
    }

    public void AddUnit(int code, int x, int y)
    {
        int newCode = (((x * 100) + y) * 100) + ((code % 10) * 10) + (code /10);
        plannedUnits.Add(newCode);
    }

    public void DeleteUnit(Unit unit)
    {
        Unit.Faction faction = unit.GetFaction();
        if(faction == Unit.Faction.Enemy)
        {
            enemyUnits.Remove(unit);
        }
        else if (faction == Unit.Faction.Player)
        {
            playerUnits.Remove(unit);
            if(unit == aeneas)
            {
                aeneas = null;
            }
        }
        else
        {
            alliedUnits.Remove(unit);
            if(alliedUnits.Count == 0 && allyDeaths != 0)
            {
                GameManager.Instance.ChangeState(GameState.Victory);
            }
        }
    }

    public void AllyDeath()
    {
        allyDeaths -= 1;
        if(allyDeaths <= 0)
        {
            UnitManager.Instance.ExhaustAll();
            GameManager.Instance.ChangeState(GameState.Defeat);
        }
    }

    // For moving and placing units
    public void UpdatePosition(Unit unit, Tile newLocation)
    {
        if(unit.HasTile())
        {
            unit.GetTile().RemoveUnit();
        }
        newLocation.SetUnit(unit);
        unit.SetTile(newLocation);
        unit.transform.position = newLocation.transform.position;
    }

    // Following two functions are just for selecting and deselecting units, with the SelectUnit function running the GenerateMoveRadius function
    public void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedUnit.GenerateMoveRadius();
        MenuManager.Instance.SetSelectedUnitUI(selectedUnit);
        // selectedUnit.GenerateAttackRadius();
    }
    public void DeselectUnit()
    {
        selectedUnit.DeleteMoveRadius();
        MenuManager.Instance.RemoveSelectedUnitUI();
        selectedUnit = null;
    }

    // Attempts to get the selected unit
    public bool TryGetUnit(out Unit unit)
    {
        if(selectedUnit != null)
        {
            unit = selectedUnit;
            return true;
        }
        unit = null;
        return false;
    }

    // Returns the selected unit
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    // Finds the nearest unit of the given type
    public Unit NearestUnit(Unit.Faction faction, Vector2 location)
    {
        switch (faction)
        {
            case Unit.Faction.Player:
                return NearestUnit(playerUnits, location);
            case Unit.Faction.Enemy:
                return NearestUnit(enemyUnits, location);
            case Unit.Faction.NPC:
                return NearestUnit(alliedUnits, location);
            case Unit.Faction.PlayerOrNPC:
                List<Unit> list = new List<Unit>(playerUnits);
                list.AddRange(alliedUnits);
                return NearestUnit(list, location);
        }
        return null;
    }
    public Unit NearestUnit(List<Unit> list, Vector2 location)
    {
        if(list.Count == 0)
        {
            return null;
        }
        Queue<Tile> queue = new Queue<Tile>();
        List<Tile> checkedTiles = new List<Tile>();
        Tile tile = GridManager.Instance.GetTile(location);
        queue.Enqueue(tile);
        bool found = false;
        while(!found)
        {
            Tile item = queue.Dequeue();
            Unit unit = item.GetUnit();
            if(unit != null && list.Contains(unit))
            {
                return unit;
            }
            checkedTiles.Add(item);
            item.GetAdjacencies().ForEach(delegate(Tile i)
            {
                if(i.isWalkable() && (!(queue.Contains(i) || checkedTiles.Contains(i))))
                {    
                    queue.Enqueue(i);
                }
            });
        }
        return null;
    }

    public void EnemyMoveTurn()
    {
        // foreach(Unit i in enemyUnits)
        // {
        //     i.ChangeUnitState(Unit.UnitState.Move);
        // }
        for(int i = enemyUnits.Count - 1; i >= 0; i--)
        {
            enemyUnits[i].ChangeUnitState(Unit.UnitState.Move);
        }
    }

    public void RunEnemyAttacks()
    {
        // foreach(Unit i in enemyUnits)
        // {
        //     i.ChangeUnitState(Unit.UnitState.Action);
        // }
        for(int i = enemyUnits.Count - 1; i >= 0; i--)
        {
            enemyUnits[i].ChangeUnitState(Unit.UnitState.Action);
        }
    }

    public void AllyMoveTurn()
    {
        for(int i = alliedUnits.Count - 1; i >= 0; i--)
        {
            alliedUnits[i].ChangeUnitState(Unit.UnitState.Move);
        }
    }

    public void StartPlayerTurn()
    {
        foreach(Unit i in playerUnits)
        {
            i.ChangeUnitState(Unit.UnitState.Move);
        }
    }
    public void TryEndPlayerTurn()
    {
        if(enemyUnits.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.Victory);
        }
        if(playerUnits.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.Defeat);
            return;
        }

        bool done = true;
        foreach(Unit i in playerUnits)
        {
            if(i.GetUnitState() != Unit.UnitState.Exhausted)
            {
                done = false;
            }
        }
        if(done)
        {
            if(GameManager.Instance.gameState == GameState.FirstPlayerTurn)
            {
                GameManager.Instance.ChangeState(GameState.AllyTurn);
            }
            else if(GameManager.Instance.gameState == GameState.PlayerTurn)
            {
                GameManager.Instance.ChangeState(GameState.EnemyAttackTurn);
            }
        }
    }

    public bool TryLoss()
    {
        if(playerUnits.Count == 0 || allyDeaths == 0 || aeneas == null)
        {
            Debug.Log((playerUnits.Count == 0) + " " + (allyDeaths == 0) + " " + (aeneas == null));
            GameManager.Instance.ChangeState(GameState.Defeat);
            return true;
        }
        return false;
    }

    public void ExhaustAll()
    {
        playerUnits.ForEach(delegate(Unit i)
        {
            i.ChangeUnitState(Unit.UnitState.Exhausted);
        });
    }
}