using UnityEngine;

namespace UI.WindowsManagement
{
    public class WindowView : MonoBehaviour
    {
        [SerializeField] public GameObject home;
        [SerializeField] public GameObject settingsMain;
        [SerializeField] public GameObject saveSelection;
        [SerializeField] public ModalWindowUnsavedChanges modalUnsavedChanges;
        [SerializeField] public NewGameWindow newGame;
    }
}