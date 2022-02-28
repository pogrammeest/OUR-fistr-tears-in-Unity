using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public float jumpPower = 5.0f;
    private float _moveSpeed;

    public float lookSpeed = 0.11f;
    public float vertLimit = 85.0f;

    private Vector2 _moveDirection;

    private Vector2 _lookDirection;
    private float _vertRotation;

    private GameObject _playerCamera;
    private GameObject _gun;
    PlayerStates states;
    private Rigidbody _rb;
    private PlayerInput _input;

    private void Awake()
    {
        _playerCamera = transform.Find("PlayerCam").gameObject;
        _gun = transform.Find("Gun").gameObject;

        _rb = GetComponent<Rigidbody>();
        _moveSpeed = walkSpeed;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.freezeRotation = true;

        states = GetComponent<PlayerStates>();


        _input = new PlayerInput();
        
        _input.Player.Jump.performed += context => Jump();

        _input.Player.Sprint.performed += context => _moveSpeed = runSpeed;
        _input.Player.Sprint.canceled += context => _moveSpeed = walkSpeed;

        _input.Player.Shoot.performed += context => Shoot();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }
    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void Update()
    {
        _moveDirection = _input.Player.Move.ReadValue<Vector2>();
        Move();

        _lookDirection = _input.Player.Look.ReadValue<Vector2>();
        Look();
    }

    public void Move()
    {
        Vector3 moveDirection = new Vector3(_moveDirection.x * _moveSpeed, 0, _moveDirection.y * _moveSpeed);
        moveDirection = Vector3.ClampMagnitude(moveDirection, _moveSpeed);
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
        _gun.transform.localEulerAngles = new Vector3 (_vertRotation/2, 0, 0);
    }
    public void Jump()
    {
        if (states.isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpPower * _rb.mass, ForceMode.Impulse);
        }
    }

    public void Shoot()
    {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<PlayerStates>())
            {
                hit.collider.gameObject.GetComponent<PlayerStates>().OnHit();
            }          
        }
    }
}
