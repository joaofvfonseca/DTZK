using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_PlayerCamera : MonoBehaviour
{
    [SerializeField] private float mouseSens;
    [SerializeField] private Camera _camera;

    private float clampCameraX;
    private float mouseX;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        clampCameraX = 0;
    }

    private void Update()
    {
        Looking();
    }

    private void Looking()
    {
        mouseX = Input.GetAxis("Mouse X") * (mouseSens * 100) * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSens * 100) * Time.fixedDeltaTime;

        clampCameraX += mouseY;
        if (clampCameraX > 90)
        {
            clampCameraX = 90;
            mouseY = 0;
            ClampCameraXTo(270);
        }
        else if (clampCameraX < -90)
        {
            clampCameraX = -90;
            mouseY = 0;
            ClampCameraXTo(90);
        }

        _camera.transform.Rotate(Vector3.left * mouseY);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void ClampCameraXTo(float param)
    {
        Vector3 eulerRotation = _camera.transform.eulerAngles;
        eulerRotation.x = param;
        _camera.transform.eulerAngles = eulerRotation;
    }
}
