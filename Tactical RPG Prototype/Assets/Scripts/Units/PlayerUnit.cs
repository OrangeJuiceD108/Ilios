using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    // List containing the highlighted tiles showing the tiles that the unit can attack beyond its move radius
    [SerializeField] private List<Tile> currentAttackVisual = new List<Tile>();

    

    // Recursive pair of functions that generates all of tiles that the unit can move to
    public override void GenerateMoveRadius()
    {
        currentTile.ChangeHighlightColor(currentTile.moveColor);
        currentTile.secondHighlight.enabled = true;
        currentMoveRadius.Add(currentTile);
        foreach(Tile i in currentTile)
        {
            if(i.isWalkable(faction))
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
                if(movesLeft > 0 && i.isWalkable(faction))
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

    public override void GenerateAttackRadius(Tile tile, int distLeft)
    {
        if(tile != currentTile)
        {
            tile.ChangeHighlightColor(currentTile.attackColor);
            tile.secondHighlight.enabled = true;
            currentAttackRadius.Add(tile);
        }
        foreach(Tile i in tile)
        {
            if(i != currentTile && !currentAttackRadius.Contains(i) && distLeft > 0)
            {
                GenerateAttackRadius(i, distLeft-1);
            }
        }
    }
    public override void DeleteAttackRadius()
    {
        currentAttackRadius.ForEach(delegate(Tile tile)
        {
            tile.secondHighlight.enabled = false;
        });
        currentAttackRadius.Clear();
    }

    public override void ChangeUnitState(UnitState state)
    {
        unitState = state;
        switch (state)
        {
            case UnitState.Move:
                break;
            case UnitState.Action:
                GameManager.Instance.ActivateButtons();
                break;
            case UnitState.Attack:
                GenerateAttackRadius(GetTile(), GetAttackDistance());
                break;
            case UnitState.Push:
                GenerateAttackRadius(GetTile(), GetAttackDistance());
                break;
            case UnitState.Exhausted:
                UpdateOrigPosition();
                break;
        }
    }
}
