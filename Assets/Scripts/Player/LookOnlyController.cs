using UnityEngine;

public class LookOnlyController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float sensitivity = 2f;

    private float _xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Vertical rotation (camera pitch)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Horizontal rotation (player yaw)
        transform.Rotate(Vector3.up * mouseX);
    }
}