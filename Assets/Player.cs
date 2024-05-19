using System;
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

    private float CameraYaw = 0.0f;
    private float CameraPitch = 0.0f;
    private float VerticalVelocity = 0.0f;
    
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
    void FixedUpdate()
    {        
        // adjust the angle of the camera
        CameraYaw += Input.GetAxisRaw("Mouse X") * Time.deltaTime * MouseSensitivity;
        CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * MouseSensitivity;
        CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
        // feed the new angle to the main camera
        PlayerCamera.transform.rotation = Quaternion.Euler(-CameraPitch, CameraYaw, 0);
        // rotate the player around the vertical axis only
        transform.rotation = Quaternion.Euler(0.0f, CameraYaw, 0.0f);
        // apply gravity and jumping force
        if (characterController.isGrounded) {
            VerticalVelocity = 0.0f;
            if (Input.GetAxis("Jump") > 0) {
                VerticalVelocity = JumpForce;
            }
        } else {
            VerticalVelocity -= Gravity * Time.deltaTime;
        }
        // use WASD to determine horizontal displacement od character
        Vector3 horizontalMovement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        // move the character and add in gravity
        characterController.Move((horizontalMovement * MovementSpeed + Vector3.up * VerticalVelocity) * Time.deltaTime);
    }
}
