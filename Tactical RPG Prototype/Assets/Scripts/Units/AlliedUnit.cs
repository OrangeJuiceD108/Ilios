using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    public override void GenerateAttackRadius(Tile tile, int distLeft)
    {
        // Generally pointless function for now.
    }
    public override void DeleteAttackRadius()
    {
        // Generally pointless function for now.
    }

    public override void ChangeUnitState(UnitState state)
    {
        unitState = state;
        switch (state)
        {
            case UnitState.Move:
                GenerateMoveRadius();
                ChangeUnitState(UnitState.Action);
                break;
            case UnitState.Action:
                ChangeUnitState(UnitState.Exhausted);
                break;
            case UnitState.Exhausted:
                break;
        }
    }
}
