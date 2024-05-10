using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TabView))]
public class TabModel : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private TabView _view;

    [SerializeField, HideInInspector]
    public Tab currentTab, previousTab;

    public GameObject[] GetPanelsToShow() {
        List<GameObject> result = new List<GameObject>();

        if (currentTab.childPanel != null) {
            result.Add(currentTab.childPanel);
        }
        result.AddRange(currentTab.contents);

        return result.ToArray();
    }

    public GameObject[] GetPanelsToHide() {
        if (previousTab == null) {
            return null;
        }

        var currentLevel = currentTab.GetLevel();
        var prevLevel = previousTab.GetLevel();

        List<GameObject> result = new List<GameObject>();

        if (currentLevel < prevLevel) {
            if (previousTab.childPanel != null) {
                result.Add(previousTab.childPanel);
            }

            if (previousTab.parentTab != null && previousTab.parentTab.childPanel != null) {
                result.AddRange(previousTab.parentTab.contents);
                result.Add(previousTab.parentTab.childPanel);
            }

            result.AddRange(previousTab.contents);
        }

        else if (currentLevel == prevLevel) {

            if (previousTab.childPanel != null) {
                result.Add(previousTab.childPanel);
            }

            result.AddRange(previousTab.contents);
        }

        return result.ToArray();
    }

    public void Init() {
        _view = GetComponent<TabView>();
    }
}
