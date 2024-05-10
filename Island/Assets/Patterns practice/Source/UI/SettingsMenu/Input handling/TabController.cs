using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TabModel), typeof(TabView))]
public class TabController : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private TabView _view;

    [SerializeField, HideInInspector]
    private TabModel _model;

    private List<Tab> _tabs;

    public void Init() {
        _view = GetComponent<TabView>();
        _model = GetComponent<TabModel>();

        _model.Init();

        _model.currentTab = _model.previousTab = null;
;
        _tabs = new List<Tab>();

        _view.TabAdded += OnTabAdded;
        _view.TabRemoved += OnTabRemoved;
        _view.Destroyed += () => {
            foreach (Tab tab in _tabs) {
                if (tab != null) {
                    tab.TabClicked -= OnTabClicked;
                }
            }

            _tabs.Clear();
        };
    }

    private void OnTabAdded(Tab addedTab) {
        if (!_tabs.Contains(addedTab)) {
            _tabs.Add(addedTab);

            addedTab.TabClicked += OnTabClicked;
        }
    }

    private void OnTabRemoved(Tab removedTab) {
        if (_tabs.Contains(removedTab)) {
            _tabs.Remove(removedTab);

            removedTab.TabClicked -= OnTabClicked;
        }
    }

    private void OnTabClicked(Tab tab) {
        Tab current = _model.currentTab;
        Tab prev = _model.previousTab;

        if (current != tab) {
            current = tab;
            prev = _model.currentTab;
        }
        _model.currentTab = current;
        _model.previousTab = prev;

        var panelsToHide = _model.GetPanelsToHide();
        if (panelsToHide != null) {
            foreach (var panel in panelsToHide) {
                panel.SetActive(false);
            }
        }
        
        var panelsToShow = _model.GetPanelsToShow();
        foreach (var panel in panelsToShow) {
            panel.SetActive(true);
        }
    }

    private void OnDestroy() {
        if (_view != null) {
            _view.TabAdded -= OnTabAdded;
            _view.TabRemoved -= OnTabRemoved;
        }

        foreach (Tab tab in _tabs) {
            if (tab != null) {
                tab.TabClicked -= OnTabClicked;
            }
        }
    }  
}
