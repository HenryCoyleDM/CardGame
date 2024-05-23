using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    new private Rigidbody rigidbody;
    new private Collider collider;
    private Collision LastCollision;
    public GameObject PlayerCamera;
    public float MovementSpeed;
    public float MouseSensitivity;
    public float CameraFollowDistance;
    public float JumpForce;
    public float Gravity;
    public float RideHeight;
    public float RideSpringStrength;
    public float RideSpringDamper;
    public float UprightSpringStrength;
    public float UprightSpringDamper;

    private float CameraYaw = 0.0f;
    private float CameraPitch = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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
        // adjust the angle of the camera
        // CameraYaw += Input.GetAxisRaw("Mouse X") * Time.deltaTime * MouseSensitivity;
        CameraPitch += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * MouseSensitivity;
        CameraPitch = Math.Clamp(CameraPitch, -90.0f, 90.0f);
        // Debug.Log("rotation degrees: "+Input.GetAxisRaw("Mouse X") * Time.deltaTime * MouseSensitivity);
        transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * Time.deltaTime * MouseSensitivity);
        // feed the new angle to the main camera
        PlayerCamera.transform.rotation = Quaternion.Euler(-CameraPitch, transform.rotation.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        // use WASD to determine horizontal displacement of character
        Quaternion upright_rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        Vector3 goal_velocity = ((upright_rotation * Vector3.forward) * Input.GetAxisRaw("Vertical") + (upright_rotation * Vector3.right) * Input.GetAxisRaw("Horizontal")) * MovementSpeed;
        rigidbody.AddForce(goal_velocity + rigidbody.velocity.y * Vector3.up - rigidbody.velocity, ForceMode.VelocityChange);
        if (rigidbody.velocity.y < JumpForce - 0.5) {
            AddRidingForce();
        }
        AddUprightSeekingForce();
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
        if (IsGrounded()) {
            rigidbody.AddForce(Vector3.up * force, ForceMode.VelocityChange);
            Debug.Log("Player jumped with force " + force);
        } else {
            Debug.Log("Player tried to jump but wasn't grounded");
        }
    }

    public bool IsOutOfBounds() {
        return transform.position.y < -50.0f;
    }

    public void AddRidingForce() {
        Vector3 ray_direction = Vector3.down;
        bool hit_other = Physics.Raycast(transform.position, ray_direction, out RaycastHit hitInfo, RideHeight);
        // Debug.Log(hitInfo.distance);
        Debug.DrawRay(transform.position, ray_direction * hitInfo.distance, Color.green, 0.0f, false);
        if (hit_other) {
            Vector3 hit_object_velocity = hitInfo.rigidbody == null ? Vector3.zero : hitInfo.rigidbody.velocity;
            float velocity_in_ray_direction = Vector3.Dot(ray_direction, rigidbody.velocity);
            float other_velocity_in_ray_direction = Vector3.Dot(ray_direction, hit_object_velocity);
            float spring_displacement = hitInfo.distance - RideHeight;
            float spring_force = spring_displacement * RideSpringStrength + (other_velocity_in_ray_direction - velocity_in_ray_direction) * RideSpringDamper;
            // Debug.Log("Generated spring force: "+spring_force);
            rigidbody.AddForce(ray_direction * spring_force);
        }
    }
    
    public void AddUprightSeekingForce() {
        Quaternion goal_rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        Quaternion torque_rotation = goal_rotation * Quaternion.Inverse(transform.rotation);
        torque_rotation.ToAngleAxis(out float rotation_amount, out Vector3 rotation_axis);
        if (rotation_amount > 180.0f) {
            rotation_axis = -rotation_axis;
            rotation_amount = 360.0f - rotation_amount;
        }
        // Debug.Log("upright rotation amount: "+rotation_amount);
        rotation_axis.Normalize();
        if (rotation_axis.x != float.NaN) {
            Debug.DrawRay(transform.position, rotation_axis * rotation_amount, Color.cyan, 0.0f, false);
            rigidbody.AddTorque(rotation_axis * rotation_amount * Mathf.Deg2Rad * UprightSpringStrength - rigidbody.angularVelocity * UprightSpringDamper);
        }
    }

    public bool IsGrounded() {
        Vector3 ray_direction = Vector3.down;
        bool hit_other = Physics.Raycast(transform.position, ray_direction, RideHeight);
        return hit_other;
    }
}
