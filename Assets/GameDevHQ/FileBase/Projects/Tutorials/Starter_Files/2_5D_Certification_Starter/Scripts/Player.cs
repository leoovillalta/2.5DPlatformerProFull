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
    private bool _onLedge = false;

    private Animator _anim;
    private bool _jumping = false;

    private Ledge _activeLedge;
    private int _gems;
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (_onLedge)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
    }

    void CalculateMovement()
    {
        //if grounded
        //calculate movement direction based on user input
        //if jump
        //adjust jumpheight
        //
        if (_controller.isGrounded == true)
        {
            if (_jumping)
            {
                _jumping = false;
                _anim.SetBool("Jumping", false);
            }

            float h = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, h) * _speed;
            _anim.SetFloat("Speed", Mathf.Abs(h));

            //what directio to face
            //if the direction is greater than 0 face right
            //else face left
            if (h != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                facing.y = _direction.z > 0 ? 0 : 180;
                transform.localEulerAngles = facing;
            }


            //_velocity = _direction * _speed;
            //If jumping was previously true

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _direction.y += _jumpHeight;
                _jumping = true;
                _anim.SetBool("Jumping", true);
            }
        }

        _direction.y -= _gravity * Time.deltaTime;
        _controller.Move(_direction * Time.deltaTime);
    }
    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _onLedge = true;
        _controller.enabled = false;
        _anim.SetBool("GrabLedge", true);
        _anim.SetFloat("Speed", 0.0f);
        _anim.SetBool("Jumping", false);
        transform.position = handPos;
        _activeLedge = currentLedge;
    }
    public void ClimbUpComplete()
    {
        _anim.SetBool("GrabLedge", false);
        transform.position = _activeLedge.GetStandPos();
        _controller.enabled = true;
    }

    public void AddGems()
    {
        _gems++;
        _uiManager.UpateGemDisplay(_gems);
    }
}
