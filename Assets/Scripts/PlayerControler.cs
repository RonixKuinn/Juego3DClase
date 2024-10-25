using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //---------Componentes-----------//
    private CharacterController _controller;
    

    //---------Inputs-----------//
    [SerializeField] private float _movementSpeed = 5;
    private float _turnSmothVelocity;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    private float _horizontal;
    private float _vertical;

    //---------Gavedad-----------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------Ground sensor-----------//
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] LayerMask _sueloLayer;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Movement();
        Gravity();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        
        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            _controller.Move(direction * _movementSpeed * Time.deltaTime);
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

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _sueloLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position,_sensorRadius);
    }

    void Start()
    {
        
    }
}
