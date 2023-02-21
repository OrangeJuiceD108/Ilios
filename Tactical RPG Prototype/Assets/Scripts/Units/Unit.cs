using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Tile currentTile;

    [SerializeField] private int moveDistance;

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
        foreach(Tile i in currentTile)
        {
            GenerateMoveRadius(i, moveDistance-1);
        }
    }
    
    public void GenerateMoveRadius(Tile tile, int movesLeft)
    {
        tile.secondHighlight.enabled = true;
        currentMoveRadius.Add(tile);
        foreach (Tile i in tile)
        {
            if(!currentMoveRadius.Contains(i) && movesLeft > 0)
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
}
