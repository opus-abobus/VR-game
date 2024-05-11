using UI.WindowsManagement;
using UnityEngine;

public class WindowModel : MonoBehaviour
{
    [SerializeField] private WindowView _view;

    public Window home, settingsMain, saveSelection;

    public void Init() {
        home = new Window(_view.home, false);
        settingsMain = new Window(_view.settingsMain, false);
        saveSelection = new Window(_view.saveSelection, false);
    }
}
