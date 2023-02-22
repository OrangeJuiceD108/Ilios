using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Attributes")]
    [SerializeField] private int moveDistance;
    [SerializeField] private int maxHP;
    [SerializeField] private Faction faction;
    

    private int currentHP;
    
    private Tile currentTile;

    private List<Tile> currentMoveRadius = new List<Tile>();

    public Tile GetTile()
    {
        if(currentTile == null)
        {
            return null;
        }
        return currentTile;
    }

    public void SetTile(Tile newTile)
    {
        currentTile = newTile;
    }

    public bool HasTile()
    {
        if(currentTile == null)
        {
            return false;
        }
        return true;
    }

    public void GenerateMoveRadius()
    {
        currentTile.secondHighlight.enabled = true;
        currentMoveRadius.Add(currentTile);
        foreach(Tile i in currentTile)
        {
            if(i.isWalkable())
            {
                GenerateMoveRadius(i, moveDistance-1);
            }
        }
    }
    
    public void GenerateMoveRadius(Tile tile, int movesLeft)
    {
        tile.secondHighlight.enabled = true;
        currentMoveRadius.Add(tile);
        foreach (Tile i in tile)
        {
            if(!currentMoveRadius.Contains(i) && movesLeft > 0 && i.isWalkable())
            {
                GenerateMoveRadius(i, movesLeft-1);
            }
        }
    }

    public void DeleteMoveRadius()
    {
        currentMoveRadius.ForEach(delegate(Tile tile)
        {
            tile.secondHighlight.enabled = false;
        });
        currentMoveRadius.Clear();
    }

    public bool withinMoveRange(Tile tile)
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

    public enum Faction
    {
        Player = 1,
        Enemy = 2,
        NPC = 3,
    }
}
