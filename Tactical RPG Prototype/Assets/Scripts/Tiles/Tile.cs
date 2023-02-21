using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer highlight;
    public SpriteRenderer secondHighlight;
    private Unit unit;
    [SerializeField] private List<Tile> adjacencies;

    void OnMouseEnter()
    {
        highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
    }

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

    public List<Tile>.Enumerator GetEnumerator()
    {
        return adjacencies.GetEnumerator();
    }
}
