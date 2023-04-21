using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    SteamVR_Action_Vector2 touchpad = null;
    SteamVR_Action_Boolean m_Boolen = null;

    CharacterController controller = null;
    public float speed = 4.0f;
    bool checkWalk = false;

    public bool UseKeyboard = false;
    // Поля для перемещения игрока, используя стрелки клавиатуры
    [Header("Перемещение с помощью Home, End, PgDn и Del")]
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    public float keyboardWalkSpeed = 6.0f;
    public float gravity = -13.0f;
    public Transform fallbackVRCamera;
    float velocityY = 0.0f;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    void Awake()
    {
        touchpad = SteamVR_Actions.default_Touchpad;
        m_Boolen = SteamVR_Actions.default_TouchClick;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        SmoothMovement();

        if (UseKeyboard) {
            UseKeyboardMovement();
        }
    }
    void SmoothMovement() {
        if (m_Boolen.GetStateDown(SteamVR_Input_Sources.RightHand)) {
            checkWalk = true;
        }
        if (m_Boolen.GetStateUp(SteamVR_Input_Sources.RightHand)) {
            checkWalk = false;
        }

        if (touchpad.axis.magnitude > 0.1f) {
            if (checkWalk) {
                Vector3 dir = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpad.axis.x, 0, touchpad.axis.y));
                controller.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(dir, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
            }
        }
    }
    void UseKeyboardMovement() {
        //Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

        transform.localEulerAngles = fallbackVRCamera.localEulerAngles;
        //Vector3 v3 = transform.localEulerAngles;
        //float rayDist = 10.0f;
        //Debug.DrawRay(v3, new Vector3(v3.x, v3.y, v3.z + rayDist), Color.red);

        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded) {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * keyboardWalkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }
}
