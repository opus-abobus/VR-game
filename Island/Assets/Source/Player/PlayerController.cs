using UnityEngine;

public class PlayerController : MonoBehaviour {
    //---------------  Скрипт НЕ для Hybrid Player  ------------------

    [SerializeField]
    private Transform _playerCamera;

    [SerializeField, Range(1.0f, 10.0f)] 
    private float _mouseSensitivity = 4.0f;

    [SerializeField] 
    private float _walkSpeed = 6.0f;

    [SerializeField] 
    private float _gravity = -13.0f;

    [SerializeField] 
    private bool _lockCursor = true;

    [SerializeField, Range(0.0f, 0.5f)] 
    private float _moveSmoothTime = 0.3f;

    [SerializeField, Range(0.0f, 0.5f)] 
    private float _mouseSmoothTime = 0.03f;

    private float _cameraPitch = 0.0f;
    private float _velocityY = 0.0f;
    private CharacterController _controller;

    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;

    private void Start() {
        _controller = GetComponent<CharacterController>();

        if (_lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Update() {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook() {
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
}
