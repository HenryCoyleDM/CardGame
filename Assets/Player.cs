using System;
using System.Collections;
using Unity.VisualScripting;
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
    private Vector3 LastHitboxFrameDisplacement = Vector3.zero;
    private float CameraYaw = 0.0f;
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
        // apply gravity and jumping force
        if (!JumpedThisTick && CharacterController.isGrounded) {
            Velocity.y = -0.1f;
        } else {
            Velocity.y -= Gravity * Time.deltaTime;
        }
        // use WASD to determine horizontal displacement od character
        Velocity = Velocity.y * Vector3.up + (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")) * MovementSpeed;
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
        CameraYaw += Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * MouseSensitivity;
        CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * MouseSensitivity;
        CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(-CameraPitch, 0.0f, 0.0f);
        transform.rotation = Quaternion.Euler(0.0f, CameraYaw, 0.0f);
    }
}
