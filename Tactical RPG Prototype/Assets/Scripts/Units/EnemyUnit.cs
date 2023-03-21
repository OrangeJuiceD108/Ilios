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
                Debug.Log(nearestUnit);
                Debug.Log(nearestUnit.GetTile());
                List<Tile> path = currentTile.FindPath(nearestUnit.GetTile(), faction);
                Debug.Log(path);
                if(path != null)
                {
                    UnitManager.Instance.UpdatePosition(this, FindOpenSpace(path));
                    if(currentTile == path[path.Count - 2])
                    {
                        Tile tile = path[path.Count - 1];
                        currentAttackRadius.Add(tile);
                        tile.enemyHighlight.enabled = true;
                    }
                }
                break;
            case UnitState.Action:
                if(currentAttackRadius.Count == 0)
                {return;}
                Unit unit = currentAttackRadius[0].GetUnit();
                if(unit != null)
                {
                    if(unit.TakeDamage(1) < 1)
                    {
                        unit.Die();
                    }
                }
                currentAttackRadius[0].enemyHighlight.enabled = false;
                currentAttackRadius.Clear();
                ChangeUnitState(UnitState.Exhausted);
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

    public override void Die()
    {
        currentTile.RemoveUnit();
        UnitManager.Instance.DeleteUnit(this);
        currentAttackRadius[0].enemyHighlight.enabled = false;
        Destroy(gameObject);
    }
}
