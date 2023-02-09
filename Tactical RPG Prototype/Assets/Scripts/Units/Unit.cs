using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Tile currentTile;

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
}
