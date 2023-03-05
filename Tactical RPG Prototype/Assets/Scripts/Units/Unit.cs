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
    
    protected Tile currentTile;

    // List containing all of the tiles that the unit can move to
    protected List<Tile> currentMoveRadius = new List<Tile>();

    public void Awake()
    {
        currentHP = maxHP;
    }

    public void Init()
    {
        this.gameObject.name = unitName;
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

    // Getter method for the faction variable
    public Faction GetFaction()
    {
        return faction;
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

    // Recursive pair of functions that generates all of tiles that the unit can move to
    // Probably need to edit this function later so that it doesn't generate a highlight, and make a different function for generating highlights
    public virtual void GenerateMoveRadius()
    {
        currentMoveRadius.Add(currentTile);
        foreach(Tile i in currentTile)
        {
            if(i.isWalkable())
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
            if(!currentMoveRadius.Contains(i) && movesLeft > 0 && i.isWalkable())
            {
                GenerateMoveRadius(i, movesLeft-1);
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

    // Removes unit from tile that it is on and then destroys it
    public void Die()
    {
        currentTile.RemoveUnit();
        Destroy(gameObject);
    }

    // Enum for the faction of the unit
    public enum Faction
    {
        Player = 1,
        Enemy = 2,
        NPC = 3,
        PlayerOrNPC = 4,
    }
}
