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
                List<Tile> path = currentTile.FindPath(nearestUnit.GetTile(), faction);
                UnitManager.Instance.UpdatePosition(this, FindOpenSpace(path));
                break;
            case UnitState.Action:
                break;
            case UnitState.Exhausted:
                
                break;
        }
    }
    private Tile FindOpenSpace(List<Tile> list)
    {
        int dist = (list.Count-1) >= moveDistance ? moveDistance : (list.Count-1);
        for(int i = dist; i > 0; i--)
        {
            if(list[i].GetUnit() == null)
            {
                return list[i];
            }
        }
        return list[0];
    }
}
