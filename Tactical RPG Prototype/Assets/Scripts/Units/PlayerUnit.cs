using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    // List containing all of the tiles that the unit can attack
    [SerializeField] private List<Tile> currentAttackVisual = new List<Tile>();


    // Recursive pair of functions that generates all of tiles that the unit can move to
    public override void GenerateMoveRadius()
    {
        currentTile.ChangeHighlightColor(currentTile.moveColor);
        currentTile.secondHighlight.enabled = true;
        currentMoveRadius.Add(currentTile);
        foreach(Tile i in currentTile)
        {
            if(i.isWalkable())
            {
                GenerateMoveRadius(i, moveDistance-1);
            }
            else if(attackDistance > 0 && !currentAttackVisual.Contains(i))
            {
                GenerateAttackVisual(i, attackDistance-1);
            }
        }
    }
    protected override void GenerateMoveRadius(Tile tile, int movesLeft)
    {
        tile.ChangeHighlightColor(currentTile.moveColor);
        tile.secondHighlight.enabled = true;
        currentMoveRadius.Add(tile);
        foreach (Tile i in tile)
        {
            if(!currentMoveRadius.Contains(i))
            {
                if(movesLeft > 0 && i.isWalkable())
                {
                    GenerateMoveRadius(i, movesLeft-1);
                }
                else if(attackDistance > 0 && !currentAttackVisual.Contains(i))
                {
                    GenerateAttackVisual(i, attackDistance-1);
                }
            }
        }
    }
    private void GenerateAttackVisual(Tile tile, int distLeft)
    {
        tile.ChangeHighlightColor(currentTile.attackColor);
        tile.secondHighlight.enabled = true;
        currentAttackVisual.Add(tile);
        foreach (Tile i in tile)
        {
            if(!currentMoveRadius.Contains(i) && !currentAttackVisual.Contains(i) && distLeft > 0)
            {
                GenerateAttackVisual(i, distLeft-1);
            }
        }
    }


    // Gets ride of the move radius and deletes the highlights
    public override void DeleteMoveRadius()
    {
        currentMoveRadius.ForEach(delegate(Tile tile)
        {
            tile.secondHighlight.enabled = false;
        });
        currentMoveRadius.Clear();
        DeleteAttackVisual();
    }
    private void DeleteAttackVisual()
    {
        currentAttackVisual.ForEach(delegate(Tile tile)
        {
            tile.secondHighlight.enabled = false;
        });
        currentAttackVisual.Clear();
    }
}
