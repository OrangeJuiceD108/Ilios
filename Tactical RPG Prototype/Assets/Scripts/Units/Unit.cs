using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Unit Attributes")]
    [SerializeField] protected int moveDistance;
    [SerializeField] protected int maxHP;
    [SerializeField] protected Faction faction;
    [SerializeField] protected int attackDistance;
    [SerializeField] protected string unitName;

    [SerializeField] protected int currentHP;
    [SerializeField] protected UnitState unitState = UnitState.Exhausted;
    
    [SerializeField] protected Tile currentTile;
    protected Tile originalPosition;

    // List containing all of the tiles that the unit can move to
    protected List<Tile> currentMoveRadius = new List<Tile>();

    // List containing all of the tiles that the unit can attack
    [SerializeField] protected List<Tile> currentAttackRadius = new List<Tile>();

    public void Awake()
    {
        currentHP = maxHP;
    }

    public void Init()
    {
        this.gameObject.name = unitName;
        originalPosition = currentTile;
    }

    public string GetName()
    {
        return unitName;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
    
    public int GetMaxHP()
    {
        return maxHP;
    }

    // Gets the tile that the Unit resides on
    public Tile GetTile()
    {
        if(currentTile == null)
        {
            return null;
        }
        return currentTile;
    }

    // Sets the tile that the unit resides on
    public void SetTile(Tile newTile)
    {
        currentTile = newTile;
    }

    // Checks if the unit is already on a tile
    public bool HasTile()
    {
        if(currentTile == null)
        {
            return false;
        }
        return true;
    }

    public void UpdateOrigPosition()
    {
        originalPosition = currentTile;
    }
    public void ReturnPosition()
    {
        UnitManager.Instance.UpdatePosition(this, originalPosition);
    }

    // Getter method for the faction variable
    public Faction GetFaction()
    {
        return faction;
    }

    public int GetAttackDistance()
    {
        return attackDistance;
    }

    // Checks if the faction given is an enemy of the faction of this unit
    public bool OpposingFaction(Faction otherFaction)
    {
        if(otherFaction == Faction.Enemy)
        {
            if(faction == Faction.NPC || faction == Faction.Player)
            {
                return true;
            }
            return false;
        }
        else
        {
            if(faction == Faction.Enemy)
            {
                return true;
            }
            return false;
        }
    }

    // Unit state function for changing the state, all units need it but they all also implement it differently so this is abstract
    public abstract void ChangeUnitState(UnitState state);

    public UnitState GetUnitState()
    {
        return unitState;
    }

    // Recursive pair of functions that generates all of tiles that the unit can move to
    // Probably need to edit this function later so that it doesn't generate a highlight, and make a different function for generating highlights
    public virtual void GenerateMoveRadius()
    {
        currentMoveRadius.Add(currentTile);
        foreach(Tile i in currentTile)
        {
            if(i.isWalkable(faction))
            {
                GenerateMoveRadius(i, moveDistance-1);
            }
        }
    }
    protected virtual void GenerateMoveRadius(Tile tile, int movesLeft)
    {
        currentMoveRadius.Add(tile);
        foreach (Tile i in tile)
        {
            if(!currentMoveRadius.Contains(i))
            {
                if(movesLeft > 0 && i.isWalkable(faction))
                {
                    GenerateMoveRadius(i, movesLeft-1);
                }
            }
        }
    }

    // Gets ride of the move radius and deletes the highlights
    public virtual void DeleteMoveRadius()
    {
        currentMoveRadius.ForEach(delegate(Tile tile)
        {
            tile.secondHighlight.enabled = false;
        });
        currentMoveRadius.Clear();
    }

    // Checks if a tile is contained within the move radius list
    public bool WithinMoveRange(Tile tile)
    {
        if(currentMoveRadius.Count == 0 || !currentMoveRadius.Contains(tile))
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    // Reduces current HP by the damage parameter and returns the remaining hitpoints
    public int TakeDamage(int damage)
    {
        currentHP-=damage;
        Debug.Log("Remaining HP: " + currentHP);
        return currentHP;
    }

    public abstract void GenerateAttackRadius(Tile tile, int distLeft);
    public abstract void DeleteAttackRadius();

    public bool WithinAttackRange(Tile tile)
    {
        return currentAttackRadius.Contains(tile);
    }

    public bool AttackRadiusIsEmpty()
    {
        Debug.Log(currentAttackRadius.Count == 0);
        return currentAttackRadius.Count == 0;
    }

    public void AddToAttackRadius(Tile tile)
    {
        currentAttackRadius.Add(tile);
        tile.enemyHighlight.enabled = true;
    }
    public void RemoveFromAttackRadius(Tile tile)
    {
        currentAttackRadius.Remove(tile);
        tile.enemyHighlight.enabled = false;
    }
    public Tile GetAttackRadiusAt(int index)
    {
        return currentAttackRadius[index];
    }

    // Removes unit from tile that it is on and then destroys it
    public virtual void Die()
    {
        currentTile.RemoveUnit();
        UnitManager.Instance.DeleteUnit(this);
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return name;
    }

    // Enum for the faction of the unit
    public enum Faction
    {
        Player = 1,
        Enemy = 2,
        NPC = 3,
        PlayerOrNPC = 4,
    }

    public enum UnitState
    {
        Move = 1,
        Action = 2,
        Attack = 3,
        Push = 4,
        Exhausted = 5,
    }
}
