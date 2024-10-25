using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _movementSpeed = 5;
    private float _horizontal;
    private float _vertical;

    [SerializeField] private float _gravity = -9.81f;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Movement();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        _controller.Move(direction * _movementSpeed * Time.deltaTime);
    }
}
