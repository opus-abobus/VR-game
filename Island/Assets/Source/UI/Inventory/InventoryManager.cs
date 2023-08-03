using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    private void Awake() {
        _canvas.SetActive(false);
    }

    private bool _isInvenoryOpen = false;
    public bool IsInventoryOpen { get { return _isInvenoryOpen; } }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (_isInvenoryOpen) {
                _isInvenoryOpen = false;
                _canvas.SetActive(false);
            }
            else {
                _isInvenoryOpen = true;
                _canvas.SetActive(true);
            }
        }
    }
}
