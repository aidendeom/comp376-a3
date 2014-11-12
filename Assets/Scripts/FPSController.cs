using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    [Range(0.01f, 1f)]
    public float MouseSensitivity = 0.01f;
    public Transform HeadTransform = null;

    new Transform transform = null;
    new Rigidbody rigidbody = null;

    private float rotX = 0f;
    private float rotY = 0f;

    // Controls
    private bool up;
    private bool down;
    private bool left;
    private bool right;

    void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();

        Screen.lockCursor = true;
    }


    void Update()
    {
        UpdateInputs();
        UpdateRotation();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdateInputs()
    {
        up = Input.GetKey(KeyCode.W);
        down = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
    }

    private void UpdatePosition()
    {
        Vector3 moveDirection = Vector3.zero;

        if (up && !down)
        {
            moveDirection += transform.forward;
        }
        else if (down && !up)
        {
            moveDirection -= transform.forward;
        }

        if (left && !right)
        {
            moveDirection -= transform.right;
        }
        else if (right && !left)
        {
            moveDirection += transform.right;
        }

        moveDirection.Normalize();

        rigidbody.AddForce(moveDirection * MoveSpeed, ForceMode.Acceleration);
    }

    private void UpdateRotation()
    {
        float dx = Input.GetAxis("Mouse X") * MouseSensitivity;
        float dy = Input.GetAxis("Mouse Y") * MouseSensitivity;

        rotY += dx;
        rotX -= dy;

        transform.rotation = Quaternion.Euler(0f, rotY, 0f);
        HeadTransform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }
}
