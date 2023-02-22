using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Highlights
    [Header("Highlights")]
    public SpriteRenderer highlight;
    public SpriteRenderer secondHighlight;

    // Unit occupying the tile
    private Unit unit;

    // Tile Attributes
    [Header("Tile Attributes")]
    [SerializeField] private bool walkable;
    // Move penalty is the multiplier for the amount of movement required to enter that space (i.e., if the move penalty is 2, it takes double the movement to enter this space)
    [SerializeField] private int moveCost;

    // Tiles adjacent to this tile
    [SerializeField] private List<Tile> adjacencies;



    // The following functions are for mouse events
    /*
    OnMouseEnter and OnMouseExit at the moment both just deal with highlighting, highlighting the tile as the mouse passes
    over it so that it is clear what tile the player is hovering over.
    */

    void OnMouseEnter()
    {
        highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
    }

    /* 
    Checks to see if a unit is selected
    - If a unit isn't selected, selects the unit on the current tile [need to make this check if the character ]
    */
    void OnMouseDown()
    {
        Unit currUnit;
        if(UnitManager.Instance.TryGetUnit(out currUnit))
        {
            if((unit == null || unit == currUnit) && currUnit.withinMoveRange(this))
            {
                UnitManager.Instance.GetSelectedUnit().DeleteMoveRadius();
                UnitManager.Instance.UpdatePosition(currUnit, this);
                UnitManager.Instance.DeselectUnit();
            }
        }
        else
        {
            if(unit != null)
            {
                UnitManager.Instance.SelectUnit(unit);
            }
        }
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

    // The following functions have to do with adjacencies
    /*
    GenerateAdjacencies creates a list of adjacencies by using the "GetTile" method four times,
    once in each cardinal direction, and then it uses a while loop to delete all of the null values
    in the list of adjacencies, creating a list that only contains adjacent tiles
    */
    public void GenerateAdjacencies(int x, int y)
    {
        adjacencies = new List<Tile>();

        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x+1,y)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x,y+1)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2((x - 1),y)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x,(y - 1))));
        while(adjacencies.Contains(null))
        {
            adjacencies.Remove(null);
        }
    }

    /*
    GetEnumerator returns an enumerator of the list of tile adjacencies, allowing tiles to be used
    in foreach loops without grabbing the list within the tile directly
    */
    public List<Tile>.Enumerator GetEnumerator()
    {
        return adjacencies.GetEnumerator();
    }
}
