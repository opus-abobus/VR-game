using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : IServiceLocator
{
    private readonly Dictionary<string, IService> _services;

    void IServiceLocator.Register<T>(T service) {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key)) {
            Debug.LogError("Attempted to register existing service");
            return;
        }

        _services.Add(key, service);
    }

    void IServiceLocator.Unregister<T>() {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key)) {
            Debug.LogError("Attempted to unregister nonexistent service");
            return;
        }

        _services.Remove(key);
    }

    T IServiceLocator.Get<T>() {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key)) {
            Debug.LogError("Attempted to unregister nonexistent service");
            return default;
            //throw new System.InvalidOperationException();
        }

        return (T) _services[key];
    }
}
