using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour {
    //---------------  Скрипт НЕ для Hybrid Player  ------------------

    private SteamVR_Action_Vector2 _touchpad;
    private SteamVR_Action_Boolean _m_Boolen;

    private CharacterController _controller;

    public float _speed = 4.0f;

    private bool _checkWalk = false;

    [SerializeField]
    private Transform _fallbackVR;

    public bool _useKeyboard = false;
    public bool _useMouseLook = false;
    // Поля для перемещения игрока, используя стрелки клавиатуры
    [Header("Перемещение с помощью Home, End, PgDn и Del")]

    [SerializeField, Range(0.0f, 0.5f)]
    private float _moveSmoothTime = 0.3f;

    public float _keyboardWalkSpeed = 6.0f;
    public float _gravity = -13.0f;

    private float _velocityY = 0.0f;
    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    void Awake() {
        _touchpad = SteamVR_Actions.default_Touchpad;
        _m_Boolen = SteamVR_Actions.default_TouchClick;
        _controller = GetComponent<CharacterController>();
    }

    void Update() {
        SmoothMovement();

        if (_useKeyboard) {
            UseKeyboardMovement();
        }
        if (_useMouseLook) {
            UpdateMouseLook();
        }
    }
    void SmoothMovement() {
        if (_m_Boolen.GetStateDown(SteamVR_Input_Sources.RightHand)) {
            _checkWalk = true;
        }
        if (_m_Boolen.GetStateUp(SteamVR_Input_Sources.RightHand)) {
            _checkWalk = false;
        }

        if (_touchpad.axis.magnitude > 0.1f) {
            if (_checkWalk) {
                Vector3 dir = Player.instance.hmdTransform.TransformDirection(new Vector3(_touchpad.axis.x, 0, _touchpad.axis.y));
                _controller.Move(_speed * Time.deltaTime * Vector3.ProjectOnPlane(dir, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
            }
        }
    }
    void UseKeyboardMovement() {
        int _w, _a, _s, _d;
        if (Input.GetKey(KeyCode.Home)) _w = 1;
        else _w = 0;
        if (Input.GetKey(KeyCode.End)) _s = -1;
        else _s = 0;
        if (Input.GetKey(KeyCode.Delete)) _a = -1;
        else _a = 0;
        if (Input.GetKey(KeyCode.PageDown)) _d = 1;
        else _d = 0;
        Vector2 targetDir = new Vector2(_a + _d, _w + _s);

        targetDir.Normalize();

        _currentDir = Vector2.SmoothDamp(_currentDir, targetDir, ref _currentDirVelocity, _moveSmoothTime);

        if (_controller.isGrounded) {
            _velocityY = 0.0f;
        }
        _velocityY += _gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * _currentDir.y + transform.right * _currentDir.x) * _keyboardWalkSpeed + Vector3.up * _velocityY;
        _controller.Move(velocity * Time.deltaTime);
    }

    [SerializeField, Range(1.0f, 10.0f)] 
    private float _mouseSensitivity = 4.0f;
    [SerializeField, Range(0.0f, 0.5f)] 
    private float _mouseSmoothTime = 0.03f;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
    private float _cameraPitch = 0.0f;

    void UpdateMouseLook() {
        if (!(Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))) {

            Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

            _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

            _fallbackVR.localEulerAngles = Vector3.right * _cameraPitch;
            transform.Rotate(_currentMouseDelta.x * _mouseSensitivity * Vector3.up);
        }
    }
}
