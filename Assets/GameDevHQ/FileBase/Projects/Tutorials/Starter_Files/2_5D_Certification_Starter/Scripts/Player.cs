using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // speed
    //gravity
    //direction
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    private Vector3 _velocity;
    private Vector3 _direction;
    private CharacterController _controller;
    [SerializeField]
    private float _jumpHeight = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if grounded
        //calculate movement direction based on user input
        //if jump
        //adjust jumpheight
        //
        if(_controller.isGrounded == true)
        {
            float h = Input.GetAxis("Horizontal");
            _direction = new Vector3(0, 0, h) * _speed;
            //_velocity = _direction * _speed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _direction.y += _jumpHeight;
            }
        }
        
        _direction.y -= _gravity * Time.deltaTime;
        _controller.Move(_direction * Time.deltaTime);
    }
}
