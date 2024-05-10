using System;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public event Action<Tab> TabAdded, TabRemoved;

    public event Action Destroyed;

    [HideInInspector]
    public List<Tab> _tabs = new List<Tab>();

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
