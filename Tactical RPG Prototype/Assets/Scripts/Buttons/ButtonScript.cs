using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void Attack()
    {
        Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();
        selectedUnit.ChangeUnitState(Unit.UnitState.Attack);
        GameManager.Instance.DeactivateButtons();
    }
    public void Push()
    {
        Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();
        selectedUnit.ChangeUnitState(Unit.UnitState.Push);
        GameManager.Instance.DeactivateButtons();
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
