using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Navigation.Tabs {

    [RequireComponent(typeof(Button))]
    public class Tab : MonoBehaviour {
        [SerializeField, HideInInspector]
        private Button _button;

        [SerializeField] public GameObject[] contents;

        [SerializeField] public GameObject childPanel;

        [SerializeField] public Tab parentTab;

        // if level is 0 then level of this tab is not set. So the minimum tab level should be 1.
        [SerializeField] private byte _level = 0;
        public byte GetLevel() { return _level; }

        [SerializeField]
        private TabView _tabView;

        public event Action<Tab> TabClicked;

        private void Start() {
            _button = GetComponent<Button>();

            if (_button == null) {
                Debug.LogError("Button reference was null.");
            }

            if (_tabView == null) {
                Debug.LogAssertion("TabView ref was null");
            }

            if (_level == 0) {
                Debug.LogAssertion("level was not set");
            }

            _tabView.AddTab(this);

            _button.onClick.AddListener(OnClicked);
        }

        private void OnClicked() {
            TabClicked?.Invoke(this);
        }

        private void OnDestroy() {
            _tabView?.RemoveTab(this);

            _button.onClick.RemoveListener(OnClicked);
        }
    }
}
