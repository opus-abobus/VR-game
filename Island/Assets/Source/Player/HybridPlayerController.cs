using System;
using System.Collections;
using UnityEngine;

public class HybridPlayerController : MonoBehaviour, Bootstrap.IBootstrap {
    [SerializeField]
    private Transform _playerCamera;

    [Header("Настройки управления игроком")]
    [SerializeField, Range(1.0f, 10.0f)]
    float _mouseSensitivity = 4.0f;

    [SerializeField]
    float _walkSpeed = 6.0f;

    [SerializeField]
    float _gravity = -13.0f;

    [SerializeField, Range(0.0f, 0.5f)]
    float _moveSmoothTime = 0.3f;
    [SerializeField, Range(0.0f, 0.5f)]
    float _mouseSmoothTime = 0.03f;

    //------------------------------------
    public bool _allowFreeCamera = true;

    public event Action OnFreeCameraActivated;

    [SerializeField]
    private FreeCameraController _freeCameraController;

    [SerializeField]
    private Transform _playerHead;
    [SerializeField]
    private Transform _playerHand;
    [SerializeField]
    private Transform _VRFallbackObjects;
    //------------------------------------
    [SerializeField]
    private Transform _canvas;

    [SerializeField]
    private SpriteRenderer _headSpriteRenderer;

    private float _cameraPitch = 0.0f;
    private float _velocityY = 0.0f;

    private CharacterController _controller = null;

    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
    public bool IsFreeCamActive { get; private set; }

    void Bootstrap.IBootstrap.Initialize() {
        _controller = GetComponent<CharacterController>();
        _startRotCam = Vector3.zero;
        _headSpriteRenderer.enabled = false;

        IsFreeCamActive = false;

        if (_freeCameraController == null)
            _allowFreeCamera = false;
        else
            _freeCameraController.Init();

        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        _freeCameraController.OnFreeCamDisabled += OnFreeCamDisabled;

        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess() {
        yield return new WaitForEndOfFrame();
        //print("updating player controller");

        while (true) {
            if (_allowFreeCamera && Input.GetKeyDown(KeyCode.Alpha0)) {
                IsFreeCamActive = true;

                SetupTransformForFreeCamera(true);

                OnFreeCameraActivated?.Invoke();

                break;
            }

            UpdateMouseLook();
            UpdateMovement();

            yield return null;
        }

        yield return null;
    }

    private Vector3 _startPosCam, _startRotCam;
    private Vector3 _startPosHand, _startRotHand;
    void SetupTransformForFreeCamera(bool isFree) {
        if (isFree) {
            _headSpriteRenderer.enabled = true;

            _startPosCam = _playerCamera.localPosition; _startRotCam = _playerCamera.localEulerAngles;

            _startPosHand = _playerHand.localPosition; _startRotHand = _playerHand.localEulerAngles;

            _playerHand.parent = _playerCamera;
        }
        else {
            _headSpriteRenderer.enabled = false;

            _playerCamera.localPosition = _startPosCam; _playerCamera.localEulerAngles = _startRotCam;

            _playerHand.parent = _VRFallbackObjects;
            _playerHand.localPosition = _startPosHand; _playerHand.localEulerAngles = _startRotHand;
        }
    }

    void UpdateMouseLook() {
        if (Input.GetMouseButton(1)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

        _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

        _playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(_currentMouseDelta.x * _mouseSensitivity * Vector3.up);
    }
    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        _currentDir = Vector2.SmoothDamp(_currentDir, targetDir, ref _currentDirVelocity, _moveSmoothTime);

        if (_controller.isGrounded) {
            _velocityY = 0.0f;
        }
        _velocityY += _gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * _currentDir.y + transform.right * _currentDir.x) * _walkSpeed + Vector3.up * _velocityY;
        _controller.Move(velocity * Time.deltaTime);
    }

    void OnGameStateChanged() {
        GameManager.GameStates gameState = GameManager.Instance.State;
        switch (gameState) {
            case GameManager.GameStates.ACTIVE: {
                    if (!IsFreeCamActive)
                        StartCoroutine(UpdateProcess());
                    else
                        OnFreeCameraActivated?.Invoke();
                    break;
                }
            case GameManager.GameStates.DEAD:
            case GameManager.GameStates.PAUSE: {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    StopAllCoroutines();
                    break;
                }
            default: {
                    StopAllCoroutines();
                    break;
                }
        }
    }

    void OnFreeCamDisabled() {
        IsFreeCamActive = false;
        SetupTransformForFreeCamera(false);

        //print("OnFreeCamDisabled");

        StopCoroutine(UpdateProcess());
        StartCoroutine(UpdateProcess());
    }

    private void OnDisable() {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _freeCameraController.OnFreeCamDisabled -= OnFreeCamDisabled;
    }
}
