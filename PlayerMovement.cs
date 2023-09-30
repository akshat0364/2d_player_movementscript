using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; 
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 0.1f;
    Vector2 moveInput;
    [SerializeField]
    private bool _isRunning = false;
    [SerializeField]
    private bool _isMoving = false;
    public bool _isFacingRight = true;
private bool isGrounded;
public Transform groundCheck;
public LayerMask groundLayer;
private bool hasJumped = false;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        } 
        private set
        {
            if(_isFacingRight !=value)
            {
                transform.Rotate(0f,180f,0f);
            }
            _isFacingRight =value;
        }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if(IsMoving)
            {
                if(IsRunning)
                {
                    return runSpeed;
                } 
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving =value;
            animator.SetBool("isMoving",value);
        }
    }

    private bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning=value;
            animator.SetBool("isRunning",value);
        }
    }
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

   void Update()
    {
        if (!isGrounded && hasJumped)
        {
            hasJumped = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasJumped = true;
        }
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed * Time.fixedDeltaTime, rb.velocity.y);

        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
       
    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasJumped = false;
        }
    }
    // New method for jumping
    //doesnt work
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            Debug.Log("Jump input detected.");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
