using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // needs one or more arrays to keep track of the various units, maybe one array containing enemies (red), one for pcs (blue), and one for npcs (green)

    public static GridManager Instance {get; private set;}

    public void UpdatePosition(Unit unit, Tile newLocation)
    {
        unit.GetTile().RemoveUnit();
        newLocation.SetUnit(unit);
        unit.SetTile(newLocation);
        unit.transform.position = newLocation.transform.position;
    }
}
