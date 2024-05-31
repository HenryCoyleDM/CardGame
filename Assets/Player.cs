using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    private CharacterController CharacterController;
    private Camera PlayerCamera;
    public float MovementSpeed;
    public float MouseSensitivity;
    public float CameraFollowDistance;
    public float JumpForce;
    public float Gravity;
    public float HP;
    public float MaxHP;
    public Vector3 HitboxFrameDisplacement = Vector3.zero;
    public float DownStairsMaxStickSlope;
    public bool WalkForwardAutomatically;
    public bool LockCameraAngle;
    private Vector3 LastHitboxFrameDisplacement = Vector3.zero;
    private float CameraPitch = 0.0f;
    public Vector3 Velocity = Vector3.zero;
    private bool JumpedThisTick = false;
    private Animator PlayerAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        PlayerCamera = GetComponentInChildren<Camera>();
        PlayerAnimator = GetComponent<Animator>();
        // keep cursor in center of frame
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // characterController.attachedRigidbody.useGravity = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space")) {
            Jump(JumpForce);
        }
        AdjustCameraAngle();
        // use WASD to determine horizontal displacement od character
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = WalkForwardAutomatically ? 1 : Input.GetAxisRaw("Vertical");
        Velocity = Velocity.y * Vector3.up + (transform.right * horizontal_input + transform.forward * vertical_input) * MovementSpeed;
        if (!JumpedThisTick && CharacterController.isGrounded) {
            KeepOnGround();
        } else {
            Debug.DrawRay(transform.position, Velocity, new Color(0.5f, 0.0f, 0.8f), 0.0f, false);
            Velocity.y -= Gravity * Time.deltaTime;
        }
        CompensateForHitboxDisplacement();
        // move the character and add in gravity
        CollisionFlags flags = CharacterController.Move(Velocity * Time.deltaTime);
        EliminateVerticalVelocityIfHitCeiling(flags);
        if (IsOutOfBounds()) {
            transform.position = new Vector3(0.0f, 10.0f, 0.0f);
            Velocity = Vector3.zero;
        }
        JumpedThisTick = false;
    }

    void FixedUpdate()
    {
        
    }

    public void Jump(float force) {
        if (CharacterController.isGrounded) {
            Velocity.y = force;
            JumpedThisTick = true;
            Debug.Log("Player jumped with force " + force);
        } else {
            Debug.Log("Player tried to jump but wasn't grounded");
        }
    }

    public bool IsOutOfBounds() {
        return transform.position.y < -50.0f;
    }

    private void EliminateVerticalVelocityIfHitCeiling(CollisionFlags flags) {
        if ((flags & CollisionFlags.Above) != 0) {
            Velocity.y = 0;
        }
    }
    
    public void RecieveDamage(float amount) {
        HP -= amount;
    }

    public void StartStabbing() {
        PlayerAnimator.SetBool("IsStabbing", true);
    }

    public void ExecuteStab() {
        PlayerAnimator.SetBool("IsStabbing", false);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f);
        foreach (Collider collider in colliders) {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.RecieveDamage(10.0f);
                return;
            }
        }
    }

    public void CompensateForHitboxDisplacement() {
        Vector3 hitboxDisplacement = HitboxFrameDisplacement - LastHitboxFrameDisplacement;
        Vector3 transformedHitboxDisplacement = transform.rotation * hitboxDisplacement;
        LastHitboxFrameDisplacement = HitboxFrameDisplacement;
        Velocity += transformedHitboxDisplacement / Time.deltaTime;
    }

    public void AdjustCameraAngle() {
        if (!LockCameraAngle) {
            float delta_camera_yaw = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * MouseSensitivity;
            transform.rotation *= Quaternion.AngleAxis(delta_camera_yaw, Vector3.up);
            CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * MouseSensitivity;
            CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
            PlayerCamera.transform.localRotation = Quaternion.Euler(-CameraPitch, 0.0f, 0.0f);
        }
    }

    public Vector3 GetHorizontalVelocity() {
        return Velocity.x * Vector3.right + Velocity.z * Vector3.forward;
    }

    public void KeepOnGround() {
        Vector3 horizontal_velocity = GetHorizontalVelocity();
        if (CharacterController.isGrounded && horizontal_velocity.sqrMagnitude > 0.0f) {
            Vector3 in_front_of_player_position = transform.position + horizontal_velocity * Time.deltaTime;
            float raycast_distance = CharacterController.height / 2 + CharacterController.skinWidth + horizontal_velocity.magnitude * DownStairsMaxStickSlope * Time.deltaTime;
            bool found_point_under_player = Physics.Raycast(transform.position, Vector3.down, out RaycastHit under_player_hit_info, raycast_distance + 1.5f);
            bool found_point_on_slope = Physics.Raycast(in_front_of_player_position, Vector3.down, out RaycastHit on_slope_hit_info, raycast_distance + 1.5f);
            if (found_point_under_player && found_point_on_slope) {
                float current_elevation = under_player_hit_info.point.y;
                float target_elevation = on_slope_hit_info.point.y;
                float delta_elevation = target_elevation - current_elevation;
                if (delta_elevation >= -horizontal_velocity.magnitude * DownStairsMaxStickSlope && delta_elevation < 0.0f) {
                    Velocity.y = delta_elevation / Time.deltaTime - 0.1f;
                    Debug.DrawRay(transform.position, Velocity, Color.magenta, 0.0f, false);
                    return;
                }
            }
            Velocity.y = -0.1f;
            Debug.DrawRay(transform.position, Velocity, Color.blue, 0.0f, false);
        }
    }
}
