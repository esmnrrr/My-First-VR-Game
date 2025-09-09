using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class XRJump : MonoBehaviour
{
    [Header("Ground")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundMask = ~0;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;

    [Header("Input (New Input System)")]
    public InputActionProperty jumpAction; // XRI action'ına veya klavyeye bağla (Space)

    CharacterController controller;
    float verticalVel;
    float coyoteTimer;
    float bufferTimer;

    void Awake() => controller = GetComponent<CharacterController>();
    void OnEnable() { if (jumpAction.reference != null) jumpAction.action.Enable(); }
    void OnDisable(){ if (jumpAction.reference != null) jumpAction.action.Disable(); }

    void Update()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
        
        if (grounded && verticalVel < 0f) verticalVel = -2f;
        
        if (jumpAction.action != null && jumpAction.action.WasPressedThisFrame())
            bufferTimer = jumpBufferTime;
        else
            bufferTimer = Mathf.Max(0f, bufferTimer - Time.deltaTime);
        
        if (bufferTimer > 0f && grounded)
        {
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            bufferTimer = 0f;
        }
        
        verticalVel += gravity * Time.deltaTime;
        
        // TEST: Klavye ile yatay hareket ekle
        Vector3 move = Vector3.zero;
        move.y = verticalVel * Time.deltaTime;
        
        // WASD ile test hareketi
        if (Input.GetKey(KeyCode.W)) move.z = 2f * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) move.z = -2f * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) move.x = -2f * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) move.x = 2f * Time.deltaTime;
        
        // Collision test
        Vector3 beforeMove = transform.position;
        CollisionFlags collisionFlags = controller.Move(move);
        Vector3 afterMove = transform.position;
        
        if (collisionFlags != CollisionFlags.None)
        {
            Debug.Log($"Collision Type: {collisionFlags}, Move: {move}, Actual movement: {afterMove - beforeMove}");
        }
    }
    public void ResetVertical() => verticalVel = 0f;
}
