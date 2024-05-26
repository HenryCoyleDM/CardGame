using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    private CharacterController characterController;
    public GameObject PlayerCamera;
    public float MovementSpeed;
    public float MouseSensitivity;
    public float CameraFollowDistance;
    public float JumpForce;
    public float Gravity;
    public float HP;
    public float MaxHP;

    private float CameraYaw = 0.0f;
    private float CameraPitch = 0.0f;
    private float VerticalVelocity = 0.0f;
    private bool JumpedThisTick = false;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        // adjust the angle of the camera
        CameraYaw += Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * MouseSensitivity;
        CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * MouseSensitivity;
        CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
        // feed the new angle to the main camera
        PlayerCamera.transform.localRotation = Quaternion.Euler(-CameraPitch, 0.0f, 0.0f);
        // rotate the player around the vertical axis only
        transform.rotation = Quaternion.Euler(0.0f, CameraYaw, 0.0f);
        // apply gravity and jumping force
        if (!JumpedThisTick && characterController.isGrounded) {
            VerticalVelocity = -0.1f;
        } else {
            VerticalVelocity -= Gravity * Time.deltaTime;
        }
        // use WASD to determine horizontal displacement od character
        Vector3 horizontalMovement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        // move the character and add in gravity
        characterController.Move((horizontalMovement * MovementSpeed + Vector3.up * VerticalVelocity) * Time.deltaTime);
        EliminateVerticalVelocityIfHitCeiling();
        if (IsOutOfBounds()) {
            transform.position = new Vector3(0.0f, 10.0f, 0.0f);
            VerticalVelocity = 0.0f;
        }
        JumpedThisTick = false;
    }

    void FixedUpdate()
    {
        
    }

    public void Jump(float force) {
        if (characterController.isGrounded) {
            VerticalVelocity = force;
            JumpedThisTick = true;
            Debug.Log("Player jumped with force " + force);
        } else {
            Debug.Log("Player tried to jump but wasn't grounded");
        }
    }

    public bool IsOutOfBounds() {
        return transform.position.y < -50.0f;
    }

    private void EliminateVerticalVelocityIfHitCeiling() {
        if ((characterController.collisionFlags & CollisionFlags.Above) != 0) {
            VerticalVelocity = 0;
        }
    }
    
    public void RecieveDamage(float amount) {
        HP -= amount;
    }
}
