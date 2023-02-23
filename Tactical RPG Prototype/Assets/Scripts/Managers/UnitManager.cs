using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // needs one or more arrays to keep track of the various units, maybe one array containing enemies (red), one for pcs (blue), and one for npcs (green)

    // This hunk of code makes this a singleton
    public static UnitManager Instance;
    void Awake()
    {
        Instance = this;
    }

    // Contains the list of unit prefabs
    public GameObject[] unitPrefabs;

    // The unit currently selected by the player
    private Unit selectedUnit;

    
    // This function spawns the units, right now most of the functionality is not there, this function's code will probably get replaced
    public void GenerateUnits()
    {
        Unit newUnit = Instantiate(unitPrefabs[0]).GetComponent<Unit>();
        newUnit.gameObject.name = "Slime";

        Tile startTile = GridManager.Instance.GetTile(new Vector2(3, 3));

        this.UpdatePosition(newUnit, startTile);



        newUnit = Instantiate(unitPrefabs[1]).GetComponent<Unit>();
        newUnit.gameObject.name = "Frog";

        startTile = GridManager.Instance.GetTile(new Vector2(12, 3));

        this.UpdatePosition(newUnit, startTile);
    }

    // For moving and placing units
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

    // Following two functions are just for selecting and deselecting units, with the SelectUnit function running the GenerateMoveRadius function
    public void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedUnit.GenerateMoveRadius();
    }

    public void DeselectUnit()
    {
        selectedUnit = null;
    }

    // Attempts to get the selected unit
    public bool TryGetUnit(out Unit unit)
    {
        if(selectedUnit != null)
        {
            unit = selectedUnit;
            return true;
        }
        unit = null;
        return false;
    }

    // Returns the selected unit
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
