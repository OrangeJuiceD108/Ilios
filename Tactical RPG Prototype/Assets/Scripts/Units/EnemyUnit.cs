using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public override void GenerateAttackRadius(Tile tile, int distLeft)
    {

    }
    public override void DeleteAttackRadius()
    {

    }

    public override void ChangeUnitState(UnitState state)
    {
        unitState = state;
        switch (state)
        {
            case UnitState.Move:
                GenerateMoveRadius();
                Unit nearestUnit = UnitManager.Instance.NearestUnit(Faction.PlayerOrNPC, currentTile.GetLocation());
                List<Tile> path = currentTile.FindPath(nearestUnit.GetTile());
                if(path.Count-1 <= moveDistance)
                {
                    UnitManager.Instance.UpdatePosition(this, path[path.Count-2]);
                }
                else
                {
                    UnitManager.Instance.UpdatePosition(this, path[moveDistance]);
                }
                break;
            case UnitState.Action:
                break;
            case UnitState.Exhausted:
                
                break;
        }
    }
}
