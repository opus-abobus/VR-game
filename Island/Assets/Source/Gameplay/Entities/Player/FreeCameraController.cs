using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour {
    [SerializeField]
    private HybridPlayerController _playerController;

    [SerializeField]
    private Transform _playerCamera;

    public event Action OnFreeCamDisabled;

    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _shiftSpeed = 16.0f;

    [SerializeField]
    private bool _showInstructions = true;

    public void Init() {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        _playerController.OnFreeCameraActivated += OnFreeCameraActivated;
    }

    private void OnEnable() {
        _realTime = Time.realtimeSinceStartup;
    }

    private void OnDisable() {
        _playerController.OnFreeCameraActivated -= OnFreeCameraActivated;
        StopAllCoroutines();
    }

    void OnFreeCameraActivated() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(UpdateProcess());
    }

    private float _realTime;
    IEnumerator UpdateProcess() {
        Vector3 _startEulerAngles = Vector3.zero;
        Vector3 _startMousePosition = Vector3.zero;

        //yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        while (true) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha0)) {
                OnFreeCamDisabled?.Invoke();
                break;
            }

            float forward = 0.0f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                forward += 1.0f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                forward -= 1.0f;
            }

            float up = 0.0f;
            if (Input.GetKey(KeyCode.E)) {
                up += 1.0f;
            }
            if (Input.GetKey(KeyCode.Q)) {
                up -= 1.0f;
            }

            float right = 0.0f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                right += 1.0f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                right -= 1.0f;
            }

            float currentSpeed = _speed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                currentSpeed = _shiftSpeed;
            }

            float realTimeNow = Time.realtimeSinceStartup;
            float deltaRealTime = realTimeNow - _realTime;
            _realTime = realTimeNow;

            Vector3 delta = new Vector3(right, up, forward) * currentSpeed * deltaRealTime;

            _playerCamera.transform.position += _playerCamera.TransformDirection(delta);

            Vector3 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(1) /* right mouse down */) {
                _startMousePosition = mousePosition;
                _startEulerAngles = _playerCamera.localEulerAngles;
            }

            if (Input.GetMouseButton(1) /* right mouse held down */) {
                Vector3 offset = mousePosition - _startMousePosition;
                _playerCamera.localEulerAngles = _startEulerAngles + new Vector3(-offset.y * 360.0f / Screen.height, offset.x * 360.0f / Screen.width, 0.0f);
            }

            yield return null;
        }
        yield return null;
    }

    void OnGameStateChanged() {
        var gameState = GameManager.Instance.CurrentState;
        switch (gameState) {
            case GameManager.GameStates.DEAD:
            case GameManager.GameStates.PAUSE: {
                    StopAllCoroutines();
                    //OnFreeCamDisabled?.Invoke();

                    break;
                }
            case GameManager.GameStates.ACTIVE: {
                    if (_playerController.IsFreeCamActive)
                        StartCoroutine(UpdateProcess());

                    break;
                }
            default: {
                    break;
                }
        }
    }

    void OnGUI() {
        if (_showInstructions && _playerController.IsFreeCamActive) {
            GUI.Label(new Rect(10.0f, 10.0f, 600.0f, 400.0f),
                "WASD EQ/Arrow Keys to translate the camera\n" +
                "Right mouse click to rotate the camera\n" +
                "Left mouse click for standard interactions.\n");
        }
    }
}
