using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class XRJump : MonoBehaviour
{
    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public float jumpBufferTime = 0.12f;
    
    [Header("Input (New Input System)")]
    public InputActionProperty jumpAction;
    
    CharacterController controller;
    float verticalVel;
    float bufferTimer;
    
    void Awake() => controller = GetComponent<CharacterController>();
    void OnEnable() { if (jumpAction.reference != null) jumpAction.action.Enable(); }
    void OnDisable(){ if (jumpAction.reference != null) jumpAction.action.Disable(); }
    
    void Update()
    {
        bool grounded = controller.isGrounded;
        
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
        
        Vector3 move = Vector3.zero;
        move.y = verticalVel * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.W)) move.z = 3f * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) move.z = -3f * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) move.x = -3f * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) move.x = 3f * Time.deltaTime;
        
        // COLLISION DEBUG
        Vector3 beforePos = transform.position;
        CollisionFlags flags = controller.Move(move);
        Vector3 afterPos = transform.position;
        
        if (flags != CollisionFlags.None)
        {
            Debug.Log($"COLLISION! Type: {flags}, Attempted: {move}, Actual: {afterPos - beforePos}");
        }
        
        if (move.magnitude > 0.01f && (afterPos - beforePos).magnitude < move.magnitude * 0.5f)
        {
            Debug.Log("MOVEMENT BLOCKED!");
        }
    }
    
    public void ResetVertical() => verticalVel = 0f;
}