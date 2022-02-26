using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpPower = 20.0f;

    public float lookSpeed = 0.75f;
    public float vertLimit = 85.0f;

    private Vector2 _moveDirection;
    public bool _isGrounded;

    private Vector2 _lookDirection;
    private float _vertRotation;

    private GameObject _playerCamera;
    private Rigidbody _rb;

    private void Awake()
    {
        _playerCamera = transform.Find("PlayerCam").gameObject;
        _rb = GetComponent<Rigidbody>();

        UnityEngine.Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.freezeRotation = true;
    }

    private void Update()
    {
        Move();
        Look();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookDirection = context.ReadValue<Vector2>();
    }

    public void Move()
    {
        Vector3 moveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection * moveSpeed, moveSpeed);
        moveDirection *= Time.deltaTime;

        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection;
    }
    public void Look()
    {
        transform.Rotate(0, _lookDirection.x * lookSpeed, 0);
        _vertRotation -= _lookDirection.y * lookSpeed;
        _vertRotation = Mathf.Clamp(_vertRotation, -vertLimit, vertLimit);
        _playerCamera.transform.localEulerAngles = new Vector3 (_vertRotation, 0, 0);
    }
    public void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    public void Sprint()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = false;
    }
}
