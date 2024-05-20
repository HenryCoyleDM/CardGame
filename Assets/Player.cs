using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    new private Rigidbody rigidbody;
    private Collision LastCollision;
    public GameObject PlayerCamera;
    public float MovementSpeed;
    public float MouseSensitivity;
    public float CameraFollowDistance;
    public float JumpForce;
    public float Gravity;

    private float CameraYaw = 0.0f;
    private float CameraPitch = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        LastCollision = null;
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
    }

    void FixedUpdate()
    {        
        // adjust the angle of the camera
        CameraYaw += Input.GetAxisRaw("Mouse X") * Time.deltaTime * MouseSensitivity;
        CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * MouseSensitivity;
        CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
        // feed the new angle to the main camera
        PlayerCamera.transform.rotation = Quaternion.Euler(-CameraPitch, CameraYaw, 0);
        // rotate the player around the vertical axis only
        rigidbody.rotation = Quaternion.Euler(0.0f, CameraYaw, 0.0f);
        // use WASD to determine horizontal displacement od character
        Vector3 horizontalMovement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        rigidbody.AddForce(horizontalMovement * MovementSpeed, ForceMode.VelocityChange);
        if (IsOutOfBounds()) {
            transform.position = new Vector3(0.0f, 10.0f, 0.0f);
            rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision) {
        LastCollision = collision;
    }

    void OnCollisionExit() {
        LastCollision = null;
    }

    public void Jump(float force) {
        if (LastCollision != null) {
            rigidbody.AddForce(Vector3.up * force, ForceMode.VelocityChange);
            Debug.Log("Player jumped with force " + force);
        } else {
            Debug.Log("Player tried to jump but wasn't grounded");
        }
    }

    public bool IsOutOfBounds() {
        return transform.position.y < -50.0f;
    }
}
