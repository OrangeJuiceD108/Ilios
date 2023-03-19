using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // This hunk of code makes this a singleton
    public static MenuManager Instance;
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TMP_Text selectedUnitNameText;
    [SerializeField] private TMP_Text selectedUnitHP;
    [SerializeField] private TMP_Text selectedUnitMaxHP;
    [SerializeField] private TMP_Text highlightedUnit;
    [SerializeField] private TMP_Text highlightedUnitHP;

    public void SetSelectedUnitUI(Unit unit)
    {
        selectedUnitNameText.text = unit.GetName();
        selectedUnitHP.text = unit.GetCurrentHP().ToString();
        selectedUnitMaxHP.text = unit.GetMaxHP().ToString();
    }

    public void RemoveSelectedUnitUI()
    {
        selectedUnitNameText.text = "Select Unit";
        selectedUnitHP.text = "-";
        selectedUnitMaxHP.text = "-";
    }

    public void SetHighlightedUnitUI(Unit unit)
    {
        highlightedUnit.text = unit.GetName();
        highlightedUnitHP.text = unit.GetCurrentHP().ToString();
    }

    public void RemoveHighlightedUnitUI()
    {
        highlightedUnitHP.text = "-";
        highlightedUnit.text = "Highlight Unit";
    }
}
