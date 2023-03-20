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
                List<Tile> path = currentTile.FindPath(GridManager.Instance.GetGoalTile(), faction);
                if(path != null)
                //temp
                {
                    UnitManager.Instance.UpdatePosition(this, FindOpenSpace(path));
                    if(currentTile == path[path.Count - 1])
                    {
                        currentTile.RemoveUnit();
                        UnitManager.Instance.DeleteUnit(this);
                        Destroy(gameObject);
                    }
                }
                ChangeUnitState(UnitState.Exhausted);
                break;
            case UnitState.Exhausted:
                break;
        }
    }
    private Tile FindOpenSpace(List<Tile> list)
    {
        int dist = (list.Count-1) > moveDistance ? moveDistance : (list.Count-1);
        for(int i = dist; i > 0; i--)
        {
            if(list[i].GetUnit() == null)
            {
                return list[i];
            }
        }
        return list[0];
    }
    public override void Die()
    {
        currentTile.RemoveUnit();
        UnitManager.Instance.AllyDeath();
        UnitManager.Instance.DeleteUnit(this);
        Destroy(gameObject);
    }
}
