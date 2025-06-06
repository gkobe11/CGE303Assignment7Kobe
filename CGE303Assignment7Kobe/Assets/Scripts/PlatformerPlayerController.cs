using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerController : MonoBehaviour
{
    //player movement speed
    public float moveSpeed = 5f;

    //jump force
    public float jumpForce = 10f;

    //layer mask for ground check
    public LayerMask groundLayer;
    public Transform GroundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    //reference to rigid body comp
    private Rigidbody2D rb;

    //player input
    private float horizontalInput;
    private float verticalInput;

    //audio
    public AudioClip jumpSound;
    public AudioClip scoreSound;
    private AudioSource playerAudio;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //attach rigid body comp to rb
        rb = GetComponent<Rigidbody2D>();

        //set reference to sudio source comp
        playerAudio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>(); //get animator component

        //check ground check is assigned
        if (GroundCheck == null)
        {
            Debug.LogError("GroundCheck not assigned to player controller");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");
        //check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            playerAudio.PlayOneShot(jumpSound, 1.0f); //play jump sound
        }
    }

    void FixedUpdate()
    {
        // if player has not been hit recently, allow movement
        if (!PlayerHealth.hitRecently)
        {
            //move player using rigid body
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        //set xVelocity parameter
        animator.SetFloat("xVelocityAbs", Mathf.Abs(rb.velocity.x)); 

        //set yVelocity parameter
        animator.SetFloat("yVelocity", rb.velocity.y); 

        //check if player is grounded
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);

        //set isGrounded parameter in animator
        animator.SetBool("onGround", isGrounded);

        //add animation here

        //ensure player is facing in movement direction
        if (horizontalInput > 0)
        {
           // transform.localScale = new Vector3(1f, 1f, 1f); //facing right
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); //reset rotation
        }
        else if (horizontalInput < 0)
        {
           // transform.localScale = new Vector3(-1f, 1f, 1f); //facing left
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); //reset rotation
        }
    }

    public void playCoinSound()
    {
        playerAudio.PlayOneShot(scoreSound, 1.0f); //play coin sound
    }
}
