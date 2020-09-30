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
    private bool _topOfLadder = false;
    private bool _onLadder = false;
    private UIManager _uiManager;
    private Ladder _activeLadder;
    private Vector3 _facingLadder;
    private bool _rolling = false;
    [SerializeField]
    private float _rollDistance = 3.0f;
    private bool _rollRight = true;
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
        if (!_onLadder)
        {
            CalculateMovement();
        }
        else
        {
            LadderMovement();
        }
        
        if (_onLedge)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
    }
    void Roll()
    {
        if (_rolling)
        {
            _rolling = false;
            _anim.SetBool("Rolling", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rolling = true;
            _direction.z += _rollDistance;
            _anim.SetBool("Rolling",true);
        }
    }

    void CalculateMovement()
    {
        if (_controller.isGrounded == true && !_rolling)
        {
            if (_jumping)
            {
                _jumping = false;
                _anim.SetBool("Jumping", false);
            }
            float h = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, h) * _speed;
            _anim.SetFloat("Speed", Mathf.Abs(h));
            
            
            if (h != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                
                facing.y = _direction.z > 0 ? 0 : 180;
                if(facing.y == 0)
                {
                    _rollRight = true;
                }
                else
                {
                    _rollRight = false;
                }
                transform.localEulerAngles = facing;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _direction.y += _jumpHeight;
                _jumping = true;
                _anim.SetBool("Jumping", true);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _direction = new Vector3(0, 0, 0);
                _rolling = true;
                if (_rollRight)
                {
                    _direction.z += _rollDistance;
                }
                else
                {
                    _direction.z -= _rollDistance;
                }
                
                _anim.SetBool("Rolling", true);
                StartCoroutine(RollTime());
            }
            //Roll();
        }
        _direction.y -= _gravity * Time.deltaTime;
        _controller.Move(_direction * Time.deltaTime);
      
    }
    public void FinishRolling()
    {
        _rolling = false;
        _anim.SetBool("Rolling", false);
    }
    IEnumerator RollTime()
    {
        yield return new WaitForSeconds(1f);
        FinishRolling();
    }
    
    void LadderMovement()
    {
        float v = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(0, v, 0)*3; 
        _anim.SetFloat("LadderSpeed", v);
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
    public void GrabLadder(Vector3 handPos, Ladder currentLadder)
    {
        _onLadder = true;

        transform.position = handPos;
    }
    public void AddGems()
    {
        _gems++;
        _uiManager.UpateGemDisplay(_gems);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.blue);
        //IF touching the ladder

        if(hit.transform.tag == "Ladder" && !_topOfLadder)
        {
            //HangingIdle
            //push one unit up the ladder for not being 
            
            _controller.Move(Vector3.up * 0.1f);
            _controller.enabled = false;
            _activeLadder = hit.transform.GetComponent<Ladder>();
            transform.position = _activeLadder.GetHanPosBottomOfLadder();
            _anim.SetBool("Ladder", true);
            _onLadder = true;
            //wait 0.1 seconds o activate again the controller
            StartCoroutine(WaitToMovePosition());
            Debug.Log("Move Up the ladder");
        }
        if(hit.transform.tag == "Ladder" && _topOfLadder)
        {
            //MoveDownAnimation trigger
            //HangingIdle
            _controller.Move(Vector3.left * 0.1f);
            _onLadder = true;
            _controller.enabled = false;
            
            _activeLadder = hit.transform.GetComponent<Ladder>();
            transform.position = _activeLadder.GetHandPosTopOfLadder();
            //flip
            flipPlayer();
            _anim.SetTrigger("ClimbDownLadder");
            //StartCoroutine(WaitToMovePosition());
            
            
            
            
            Debug.Log("MoveDownTheLadder");
        }
        if(_controller.isGrounded == true && _onLadder)
        {
            _onLadder = false;
            _anim.SetFloat("LadderSpeed", 0.0f);
            _anim.SetBool("Ladder", false);
            //Moving down, if it hits ground remove Player from Ladder
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TopOfLadder" && _onLadder)
        {
            _onLadder = false;
            _controller.enabled = false;
            _anim.SetTrigger("ClimbTopLadder");
           
            // _anim.SetBool("Ladder", false);
            //transform.position = other.transform.parent.GetComponent<Ladder>().GetStandPos();
            _activeLadder = other.transform.parent.GetComponent<Ladder>();
            Debug.Log("reached top of the ladder");
            //Reached the top
            //activate final animation
        }
        
    }
    IEnumerator WaitToMovePosition()
    {
        yield return new WaitForSeconds(0.1f);
        //yield return null;
        //_anim.SetTrigger("ClimbDownLadder");
        _controller.enabled = true;
    }
    public void StartClimbDown()
    {

    }
    public void flipPlayer()
    {
        Debug.Log("FlipPlayer()");
        
        _facingLadder = transform.localEulerAngles;
        _facingLadder.y = _facingLadder.y == 0 ? 180 : 0;
        Debug.Log(_facingLadder);
        transform.localEulerAngles = _facingLadder;
        //transform.eulerAngles = Vector3(0, 180, 0);
    }
    public void ClimbDownLadder()
    {
        _anim.SetFloat("Speed", 0.0f);
        _anim.SetFloat("LadderSpeed", 0.0f);
        _anim.SetBool("Ladder", true);
        _onLadder = true;
       
        _controller.enabled = true;
        _topOfLadder = false;
    }
    public void ClimbUpLadderComplete()
    {


        _anim.SetFloat("Speed", 0.0f);
        _anim.SetFloat("LadderSpeed", 0.0f);
        _anim.SetBool("Ladder", false);
        _onLadder = false;
        transform.position = _activeLadder.GetStandPos();
        _controller.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "TopOfLadder")
        {
            _topOfLadder = true;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "TopOfLadder")
        {
            _topOfLadder = false;
        }
    }
}
