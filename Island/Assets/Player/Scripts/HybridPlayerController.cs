using System.Collections;
using UnityEngine;

public class HybridPlayerController : MonoBehaviour {
    [SerializeField] Transform playerCamera;

    [Header("Настройки управления игроком")]
    [SerializeField, Range(1.0f, 10.0f)] float mouseSensitivity = 4.0f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    public bool alwaysShowCursor = false;

    //------------------------------------
    [Header("Настройки свободной камеры")]
    public bool allowFreeCamera = true;
    public bool headFollowsCamera = true;
    public float speed = 4.0f;
    public float shiftSpeed = 16.0f;
    public bool showInstructions = true;
    [SerializeField] Transform playerHead;
    [SerializeField] Transform playerHand;
    [SerializeField] Transform VRFallbackObjects;
    //------------------------------------

    public bool canvasFollowsCamera = false;
    [SerializeField] Transform canvas;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private void Start() {
        controller = GetComponent<CharacterController>();
        startRotCam = Vector3.zero;
        _freeCamera = FreeCamera();
    }

    IEnumerator _freeCamera;
    bool freeCamera = false;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (freeCamera) {
                freeCamera = false;

                StopCoroutine(_freeCamera);

                SetupTransformForFreeCamera(false);
            }
            else {
                freeCamera = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SetupTransformForFreeCamera(true);

                StartCoroutine(_freeCamera);
            }
        }
        if (!freeCamera) {
            UpdateMouseLook();
            UpdateMovement();
        }
    }
    Vector3 startPosCam, startRotCam;
    Vector3 startPosHead, startRotHead;
    Vector3 startPosHand, startRotHand;
    Vector3 startPosCanvas, startRotCanvas;
    void SetupTransformForFreeCamera(bool isFree) {
        if (isFree) {
            startPosCam = playerCamera.localPosition; startRotCam = playerCamera.localEulerAngles;
            startPosHead = playerHead.localPosition; startRotHead = playerHead.localEulerAngles;
            startPosHand = playerHand.localPosition; startRotHand = playerHand.localEulerAngles;
            startPosCanvas = canvas.localPosition; startRotCanvas = canvas.localEulerAngles;

            if (headFollowsCamera) playerHead.parent = playerCamera;
            if (canvasFollowsCamera) canvas.parent = playerCamera;

            playerHand.parent = playerCamera;
        }
        else {
            playerCamera.localPosition = startPosCam; playerCamera.localEulerAngles = startRotCam; //playerCamera.localScale = Vector3.one;

            if (headFollowsCamera) {
                playerHead.parent = VRFallbackObjects;
                playerHead.localPosition = startPosHead; playerHead.localEulerAngles = startRotHead; //playerHead.localScale = Vector3.one;
            }

            if (canvasFollowsCamera) {
                canvas.parent.parent = VRFallbackObjects;
                canvas.localPosition = startPosCanvas; canvas.localEulerAngles = startRotCanvas;
            }

            playerHand.parent = VRFallbackObjects;
            playerHand.localPosition = startPosHand; playerHand.localEulerAngles = startRotHand; //playerHand.localScale = Vector3.one;
        }
    }
    [SerializeField] EscapeMenu escapeMenu;
    void UpdateMouseLook() {
        if (!alwaysShowCursor) {
            if (Input.GetMouseButton(1)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else {
                if (!escapeMenu.pauseGame) {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(currentMouseDelta.x * mouseSensitivity * Vector3.up);
    }
    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded) {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }


    Vector3 startEulerAngles;
    Vector3 startMousePosition;
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

            float currentSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                currentSpeed = shiftSpeed;
            }

            float realTimeNow = Time.realtimeSinceStartup;
            float deltaRealTime = realTimeNow - realTime;
            realTime = realTimeNow;

            Vector3 delta = new Vector3(right, up, forward) * currentSpeed * deltaRealTime;


            playerCamera.transform.position += playerCamera.TransformDirection(delta);

            Vector3 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(1) /* right mouse */) {
                startMousePosition = mousePosition;
                startEulerAngles = playerCamera.localEulerAngles;
            }

            if (Input.GetMouseButton(1) /* right mouse */) {
                Vector3 offset = mousePosition - startMousePosition;
                playerCamera.localEulerAngles = startEulerAngles + new Vector3(-offset.y * 360.0f / Screen.height, offset.x * 360.0f / Screen.width, 0.0f);
            }
            yield return null;
        }
    }

    float realTime;
    private void OnEnable() {
        realTime = Time.realtimeSinceStartup;
    }

    void OnGUI() {
        if (showInstructions && freeCamera) {
            GUI.Label(new Rect(10.0f, 10.0f, 600.0f, 400.0f),
                "WASD EQ/Arrow Keys to translate the camera\n" +
                "Right mouse click to rotate the camera\n" +
                "Left mouse click for standard interactions.\n");
        }
    }
}
