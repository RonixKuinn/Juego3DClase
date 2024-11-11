using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPSControler : MonoBehaviour
{
    //---------Componentes-----------//
    private CharacterController _controller;
    private Transform _camera;
    private Transform _lookAtPlayer;

    //---------Cameras-----------//
    [SerializeField] private GameObject _normalCamera;
    [SerializeField] private GameObject _aimCamera;

    //---------Inputs-----------//
    private float _horizontal;
    private float _vertical;
    
    [SerializeField] private float _movementSpeed = 5;

    [SerializeField] private float _jumpHeight = 1f;

    [SerializeField] private float _pushForce = 10;

    //---------Gavedad-----------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------Ground sensor-----------//
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] LayerMask _sueloLayer;

    [SerializeField] private AxisState xAsis;
    [SerializeField] private AxisState yAsis;

     void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _lookAtPlayer = GameObject.Find("LookAtPlayer").transform;
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire2"))
        {
            _normalCamera.SetActive(false);
            _aimCamera.SetActive(true);
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            _normalCamera.SetActive(true);
            _aimCamera.SetActive(false);
        }

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        Gravity();
        Movement();
    }

    void Movement()
    {
        Vector3 move = new Vector3(_horizontal, 0, _vertical);
        
        yAsis.Update(Time.deltaTime);
        xAsis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAsis.Value, 0);
        _lookAtPlayer.rotation = Quaternion.Euler(xAsis.Value, yAsis.Value, 0);

        if(move != Vector3.zero)
        {
        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    void Gravity()
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }
       
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _sueloLayer);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
