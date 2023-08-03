using System.Collections;
using UnityEngine;

public class HybridPlayerController : MonoBehaviour {
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

    public bool _alwaysShowCursor = false;

    //------------------------------------
    [Header("Настройки свободной камеры")]
    public bool _allowFreeCamera = true;
    public bool _headFollowsCamera = true;
    public float _speed = 4.0f;
    public float _shiftSpeed = 16.0f;
    public bool _showInstructions = true;

    [SerializeField] 
    private Transform _playerHead;
    [SerializeField] 
    private Transform _playerHand;
    [SerializeField] 
    private Transform _VRFallbackObjects;
    //------------------------------------

    public bool _canvasFollowsCamera = false;

    [SerializeField] 
    private Transform _canvas;

    [SerializeField] 
    private SpriteRenderer _headSpriteRenderer;

    [SerializeField] 
    private AudioSource _audioSource;

    private float _cameraPitch = 0.0f;
    private float _velocityY = 0.0f;

    private CharacterController _controller = null;

    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;

    private void Start() {
        _controller = GetComponent<CharacterController>();
        _startRotCam = Vector3.zero;
        _freeCamControl = FreeCamera();
        _headSpriteRenderer.enabled = false;
        _audioSource = GetComponent<AudioSource>();

        //--
        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess() {
        yield return new WaitForSecondsRealtime(1);

        while (true) {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                if (_freeCamera) {
                    _freeCamera = false;

                    _audioSource.mute = false;

                    StopCoroutine(_freeCamControl);

                    SetupTransformForFreeCamera(false);
                }
                else {
                    _freeCamera = true;

                    _audioSource.mute = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    SetupTransformForFreeCamera(true);

                    StartCoroutine(_freeCamControl);
                }
            }
            if (!_freeCamera) {
                UpdateMouseLook();
                UpdateMovement();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator _freeCamControl;
    private bool _freeCamera = false;

    private Vector3 _startPosCam, _startRotCam;
    private Vector3 _startPosHead, _startRotHead;
    private Vector3 _startPosHand, _startRotHand;
    private Vector3 _startPosCanvas, _startRotCanvas;
    void SetupTransformForFreeCamera(bool isFree) {
        if (isFree) {
            _headSpriteRenderer.enabled = true;

            _startPosCam = _playerCamera.localPosition; _startRotCam = _playerCamera.localEulerAngles;
            _startPosHead = _playerHead.localPosition; _startRotHead = _playerHead.localEulerAngles;
            _startPosHand = _playerHand.localPosition; _startRotHand = _playerHand.localEulerAngles;
            _startPosCanvas = _canvas.localPosition; _startRotCanvas = _canvas.localEulerAngles;

            if (_headFollowsCamera) _playerHead.parent = _playerCamera;
            if (_canvasFollowsCamera) _canvas.parent = _playerCamera;

            _playerHand.parent = _playerCamera;
        }
        else {
            _headSpriteRenderer.enabled = false;

            _playerCamera.localPosition = _startPosCam; _playerCamera.localEulerAngles = _startRotCam; //playerCamera.localScale = Vector3.one;

            if (_headFollowsCamera) {
                _playerHead.parent = _VRFallbackObjects;
                _playerHead.localPosition = _startPosHead; _playerHead.localEulerAngles = _startRotHead; //playerHead.localScale = Vector3.one;
            }

            if (_canvasFollowsCamera) {
                _canvas.parent.parent = _VRFallbackObjects;
                _canvas.localPosition = _startPosCanvas; _canvas.localEulerAngles = _startRotCanvas;
            }

            _playerHand.parent = _VRFallbackObjects;
            _playerHand.localPosition = _startPosHand; _playerHand.localEulerAngles = _startRotHand; //playerHand.localScale = Vector3.one;
        }
    }

    [SerializeField] 
    private EscapeMenu _escapeMenu;

    [SerializeField]
    private HungerSystem _hungerSystem;

    void UpdateMouseLook() {
        if (!_alwaysShowCursor) {
            if (Input.GetMouseButton(1)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else {
                if (_escapeMenu.PauseGame || _hungerSystem.IsGameOver || EvacuationSystem.Instance._isEvacuated)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
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

    private Vector3 _startEulerAngles;
    private Vector3 _startMousePosition;
    IEnumerator FreeCamera() {
        while (true) {
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

            if (Input.GetMouseButtonDown(1) /* right mouse */) {
                _startMousePosition = mousePosition;
                _startEulerAngles = _playerCamera.localEulerAngles;
            }

            if (Input.GetMouseButton(1) /* right mouse */) {
                Vector3 offset = mousePosition - _startMousePosition;
                _playerCamera.localEulerAngles = _startEulerAngles + new Vector3(-offset.y * 360.0f / Screen.height, offset.x * 360.0f / Screen.width, 0.0f);
            }
            yield return null;
        }
    }

    private float _realTime;
    private void OnEnable() {
        _realTime = Time.realtimeSinceStartup;
    }

    void OnGUI() {
        if (_showInstructions && _freeCamera) {
            GUI.Label(new Rect(10.0f, 10.0f, 600.0f, 400.0f),
                "WASD EQ/Arrow Keys to translate the camera\n" +
                "Right mouse click to rotate the camera\n" +
                "Left mouse click for standard interactions.\n");
        }
    }
}
