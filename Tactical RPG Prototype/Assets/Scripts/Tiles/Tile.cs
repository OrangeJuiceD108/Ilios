using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : PathfinderNode
{
    // Highlights
    [Header("Highlights")]
    public SpriteRenderer highlight;
    public SpriteRenderer secondHighlight;
    public SpriteRenderer enemyHighlight;
    public SpriteRenderer goalHighlight;

    //Highlight colors
    public Color attackColor;
    public Color moveColor;

    // Stores the location of the tile
    // Vector2 location;

    // Unit occupying the tile
    private Unit unit;

    // Tile Attributes
    [Header("Tile Attributes")]
    [SerializeField] private bool walkable;

    // Move penalty is the multiplier for the amount of movement required to enter that space (i.e., if the move penalty is 2, it takes double the movement to enter this space)
    [SerializeField] private int moveCost;


    // The following functions are for mouse events
    /*
    OnMouseEnter and OnMouseExit at the moment both just deal with highlighting, highlighting the tile as the mouse passes
    over it so that it is clear what tile the player is hovering over.
    */

    void OnMouseEnter()
    {
        highlight.enabled = true;
        if(unit != null)
        {
            MenuManager.Instance.SetHighlightedUnitUI(unit);
        }
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
        MenuManager.Instance.RemoveHighlightedUnitUI();
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();
            if(selectedUnit.GetUnitState() == Unit.UnitState.Move)
            {
                UnitManager.Instance.DeselectUnit();
            }
            if(selectedUnit.GetUnitState() == Unit.UnitState.Action || selectedUnit.GetUnitState() == Unit.UnitState.Attack || selectedUnit.GetUnitState() == Unit.UnitState.Push)
            {
                if(!selectedUnit.AttackRadiusIsEmpty())
                {
                    selectedUnit.DeleteAttackRadius();
                }
                selectedUnit.ChangeUnitState(Unit.UnitState.Move);
                selectedUnit.ReturnPosition();
                selectedUnit.GenerateMoveRadius();
                GameManager.Instance.DeactivateButtons();
            }
        }
    }

    /* 
    Checks to see if a unit is selected
    - If a unit isn't selected, selects the unit on the current tile [need to make this check if the character ]
    */
    void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {return;}
        // Debug.Log(this);
        Unit currUnit;
        if(UnitManager.Instance.TryGetUnit(out currUnit))
        {
            if((unit == null || unit == currUnit) && currUnit.WithinMoveRange(this) && currUnit.GetUnitState() == Unit.UnitState.Move)
            {
                UnitManager.Instance.UpdatePosition(currUnit, this);
                currUnit.DeleteMoveRadius();
                currUnit.ChangeUnitState(Unit.UnitState.Action);
                return;
            }
            else if(currUnit.GetUnitState() == Unit.UnitState.Attack && currUnit.WithinAttackRange(this) && unit != null && unit.OpposingFaction(currUnit.GetFaction()))
            {
                int curHP = unit.TakeDamage(1);
                if(curHP <=0)
                {
                    unit.Die();
                }
                currUnit.DeleteAttackRadius();
                currUnit.ChangeUnitState(Unit.UnitState.Exhausted);
                UnitManager.Instance.DeselectUnit();
                UnitManager.Instance.TryEndPlayerTurn();
                return;
            }
            Tile moveTile;
            if(currUnit.GetUnitState() == Unit.UnitState.Push && currUnit.WithinAttackRange(this) && CanBePushed(currUnit, out moveTile))
            {
                // UnitManager.Instance.UpdatePosition(unit, moveTile);
                if(!unit.AttackRadiusIsEmpty())
                {
                    Vector2 direction = new Vector2(moveTile.GetLocation().x - location.x, moveTile.GetLocation().y - location.y);
                    Tile tile = unit.GetAttackRadiusAt(0);
                    
                    unit.RemoveFromAttackRadius(tile);
                    Vector2 loc = tile.GetLocation() + direction;
                    Debug.Log(tile.GetLocation());
                    Debug.Log(direction);
                    Debug.Log(loc);
                    unit.AddToAttackRadius(GridManager.Instance.GetTile(loc));
                }
                UnitManager.Instance.UpdatePosition(unit, moveTile);
                currUnit.DeleteAttackRadius();
                currUnit.ChangeUnitState(Unit.UnitState.Exhausted);
                currUnit.UpdateOrigPosition();
                UnitManager.Instance.DeselectUnit();
                UnitManager.Instance.TryEndPlayerTurn();
            }
        }
        else
        {
            if(unit != null && unit.GetFaction() == Unit.Faction.Player && unit.GetUnitState() == Unit.UnitState.Move)
            {
                UnitManager.Instance.SelectUnit(unit);
            }
        }
    }



    // Checks ifs a unit on the current tile can be pushed by the input unit
    private bool CanBePushed(Unit givenUnit, out Tile moveTile)
    {
        moveTile = null;
        if(unit == null)
        {
            return false;
        }

        Tile tile = givenUnit.GetTile();
        Vector2 loc2 = tile.GetLocation();
        float xDirection = location.x - loc2.x;
        float yDirection = location.y - loc2.y;

        Vector2 direction = new Vector2(location.x + xDirection, location.y + yDirection);

        Tile pushLoc = GridManager.Instance.GetTile(direction);

        if(pushLoc.isWalkable() && pushLoc.GetUnit() == null)
        {
            moveTile = pushLoc;
            return true;
        }
        return false;
    }



    // Changes the color of the highlight
    public void ChangeHighlightColor(Color color)
    {
        secondHighlight.material.color = color;
    }


    // A* Search Algorithm
    public List<Tile> FindPath(Tile target, Unit.Faction faction)
    {
        Debug.Log(target);
        this.SetCosts(0,0);
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(this);

        while(openList.Count != 0)
        {
            Tile currentTile = FindLowestF(openList);
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if(currentTile == target)
            {   
                return BackTrack(currentTile, openList, closedList);
            }

            foreach(Tile i in currentTile)
            {
                if(i == target || i.isWalkable(faction))
                {
                    if(closedList.Contains(i))
                    {
                        continue;
                    }
                    if(i.GetF() == -1 || i.GetG() > currentTile.GetG()+1)
                    {
                        i.SetCosts(currentTile.GetG() + 1, Math.Abs((int)i.GetLocation().x - (int)target.GetLocation().x) + Math.Abs((int)i.GetLocation().y - (int)target.GetLocation().y));
                    }
                    else
                    {
                        continue;
                    }
                    
                    openList.Add(i);
                }
            }
        }
        foreach(Tile i in closedList)
        {
            i.Reset();
        }
        foreach(Tile i in openList)
        {
            i.Reset();
        }
        return null;
    }
    private Tile FindLowestF(List<Tile> list)
    {
        Tile tile = list[0];
        foreach(Tile i in list)
        {
            if(tile != i && (tile.GetF() > i.GetF() || (tile.GetF() == i.GetF() && tile.GetH() > i.GetH())))
            {
                tile = i;
            }
        }
        return tile;
    }
    private List<Tile> BackTrack(Tile destination, List<Tile> openList, List<Tile> closedList)
    {
        List<Tile> track = new List<Tile>();
        track.Add(destination);
        Tile currentTile = destination;
        while(currentTile.GetG() != 0)
        {
            currentTile = FindNextLowestG(currentTile);
            track.Add(currentTile);
        }
        track.Reverse();
        foreach(Tile i in closedList)
        {
            i.Reset();
        }
        foreach(Tile i in openList)
        {
            i.Reset();
        } 
        return track;
    }
    private Tile FindNextLowestG(Tile tile)
    {
        Tile someTile = tile;
        foreach(Tile i in tile)
        {
            if(i.GetG() < tile.GetG() && i.GetG() > -1)
            {
                someTile = i;
            }
        }       
        if(tile == someTile)
        {
            return null;
        }
        return someTile;
    }

    // The following functions are for messing with the unit contained on the tile, they do what they say in their names lol

    public Unit GetUnit()
    {
        return unit;
    }

    public void SetUnit(Unit newUnit)
    {
        unit = newUnit;
    }

    public void RemoveUnit()
    {
        unit = null;
    }

    // Getter method for the walkable parameter
    public bool isWalkable()
    {
        return walkable;
    }

    public bool isWalkable(Unit.Faction faction)
    {
        if(unit != null)
        {
            return walkable && !unit.OpposingFaction(faction);
        }
        return walkable;
    }
    
    // Setter method for the walkable parameter, mostly used during initialization
    public void SetWalkable(bool walk)
    {
        walkable = walk;
    }

    public override string ToString()
    {
        return GetLocation().ToString();
    }
}