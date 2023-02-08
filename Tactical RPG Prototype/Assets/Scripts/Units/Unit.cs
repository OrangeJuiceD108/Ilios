using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Tile currentTile;

    public Tile GetTile()
    {
        return currentTile;
    }

    public void SetTile(Tile newTile)
    {
        currentTile = newTile;
    }
}
