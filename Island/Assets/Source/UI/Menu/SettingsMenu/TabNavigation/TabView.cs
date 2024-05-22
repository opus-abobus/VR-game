using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Navigation.Tabs {

    public class TabView : MonoBehaviour {
        [SerializeField] public TextMeshProUGUI settingsHeader;

        public event Action<Tab> TabAdded, TabRemoved;

        public event Action Destroyed;

        [HideInInspector] public List<Tab> _tabs = new List<Tab>();

        public void AddTab(Tab tab) {
            _tabs.Add(tab);

            TabAdded?.Invoke(tab);
        }

        public void RemoveTab(Tab tab) {
            _tabs.Remove(tab);

            TabRemoved?.Invoke(tab);
        }

        private void OnDestroy() {
            Destroyed?.Invoke();
        }
    }
}
