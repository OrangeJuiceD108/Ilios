using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // needs one or more arrays to keep track of the various units, maybe one array containing enemies (red), one for pcs (blue), and one for npcs (green)

    public static UnitManager Instance;

    public GameObject[] unitPrefabs;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateUnits()
    {
        Unit newUnit = Instantiate(unitPrefabs[0]).GetComponent<Unit>();
        newUnit.gameObject.name = "Slime";

        Tile startTile = GridManager.Instance.GetTile(new Vector2(3, 3));

        this.UpdatePosition(newUnit, startTile);
    }

    public void UpdatePosition(Unit unit, Tile newLocation)
    {
        if(unit.HasTile())
        {
            unit.GetTile().RemoveUnit();
        }
        newLocation.SetUnit(unit);
        unit.SetTile(newLocation);
        unit.transform.position = newLocation.transform.position;
    }
}
