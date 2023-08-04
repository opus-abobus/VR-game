using System;
using UnityEngine;

public class GameManager : MonoBehaviour, Bootstrap.IBootstrap
{
    //<summary> Данный класс отвечает за управление менеджерами, например, GameSettingsManager
    //</summary>

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    

    public Action OnInitialized;

    void Bootstrap.IBootstrap.Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }

        OnInitialized?.Invoke();
    }
}
