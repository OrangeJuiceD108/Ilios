using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void Attack()
    {
        Debug.Log("ATTACK");
        Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();
        selectedUnit.GenerateAttackRadius(selectedUnit.GetTile(), selectedUnit.GetAttackDistance());
    }
    public void Push()
    {
        Debug.Log("PUSH");
    }
    public void Wait()
    {
        Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();
        selectedUnit.ChangeUnitState(Unit.UnitState.Exhausted);
        UnitManager.Instance.DeselectUnit();
        GameManager.Instance.DeactivateButtons();
        UnitManager.Instance.TryEndPlayerTurn();
    }
}
