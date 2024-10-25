using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _movementSpeed = 5;
    private float _horizontal;
    private float _vertical;

    //---------Gavedad-----------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------Ground sensor-----------//
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] LayerMask _sueloLayer;
    [SerializeField] private bool _isGrounded;


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
        IsGrounded();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        _controller.Move(direction * _movementSpeed * Time.deltaTime);
    }

    void Gravity()
    {
        if(!_isGrounded)
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }
       
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void IsGrounded()
    {
        if(Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _sueloLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
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
