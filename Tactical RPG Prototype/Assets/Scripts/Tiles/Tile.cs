using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;
    private Unit unit;

    void OnMouseEnter()
    {
        highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
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
}
