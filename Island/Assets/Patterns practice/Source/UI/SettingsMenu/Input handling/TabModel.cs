using System.Collections.Generic;
using UnityEngine;

namespace UI.Navigation.Tabs {

    [RequireComponent(typeof(TabView))]
    public class TabModel : MonoBehaviour {
        [SerializeField, HideInInspector]
        private TabView _view;

        [SerializeField, HideInInspector]
        public Tab currentTab, previousTab;

        public void Init() {
            _view = GetComponent<TabView>();
        }

        public GameObject[] GetPanelsForTab(Tab tab) {
            List<GameObject> result = new List<GameObject>();

            while (tab.parentTab != null) {
                if (tab.childPanel != null) {
                    result.Add(tab.childPanel);
                }

                result.AddRange(tab.contents);

                tab = tab.parentTab;
            }

            if (tab.childPanel != null) {
                result.Add(tab.childPanel);
            }

            result.AddRange(tab.contents);

            return result.ToArray();
        }

        public GameObject[] GetNextPanelsToShow() {
            List<GameObject> result = new List<GameObject>();

            if (currentTab.childPanel != null) {
                result.Add(currentTab.childPanel);
            }
            result.AddRange(currentTab.contents);

            return result.ToArray();
        }

        public GameObject[] GetNextPanelsToHide() {
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
    }
}
