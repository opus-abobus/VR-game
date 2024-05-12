using System.Collections.Generic;
using UnityEngine;

namespace UI.Navigation.Tabs {

    [RequireComponent(typeof(TabModel), typeof(TabView))]
    public class TabController : MonoBehaviour {
        [SerializeField, HideInInspector]
        private TabView _view;

        [SerializeField, HideInInspector]
        private TabModel _model;

        private List<Tab> _tabs;

        public void Init() {
            _view = GetComponent<TabView>();
            _model = GetComponent<TabModel>();

            _view.settingsHeader.text = string.Empty;

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
            UpdateModel(tab);

            HidePanels();

            ShowPanels();

            UpdateHeader();
        }

        private void UpdateModel(Tab tab) {
            Tab current = _model.currentTab;
            Tab prev = _model.previousTab;

            if (current != tab) {
                current = tab;
                prev = _model.currentTab;
            }
            _model.currentTab = current;
            _model.previousTab = prev;
        }

        private void HidePanels() {
            var panelsToHide = _model.GetNextPanelsToHide();
            if (panelsToHide != null) {
                foreach (var panel in panelsToHide) {
                    panel.SetActive(false);
                }
            }
        }

        private void ShowPanels() {
            var panelsToShow = _model.GetNextPanelsToShow();
            if (panelsToShow != null) {
                for (int i = 0; i < panelsToShow.Length; i++) {
                    panelsToShow[i].SetActive(true);
                }
            }
        }

        private void UpdateHeader() {
            _view.settingsHeader.text = _model.currentTab.name;
        }

        private void HideAllPanels() {
            var panels = _model.GetPanelsForTab(_model.currentTab);
            foreach (var panel in panels) {
                panel.SetActive(false);
            }

            _view.settingsHeader.text = string.Empty;
        }

        private void OnDisable() {
            HideAllPanels();
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
}
