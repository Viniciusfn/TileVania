using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runVelocity = 300f;
    [SerializeField] private float jumpVelocity = 9f;
    [SerializeField] private float climbVelocity = 100f;
    [SerializeField] private float deathAnimationSpeed = 9f;

    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform weaponSpot;
    
    bool isAlive = true;
    float regularGravityScale;
    Vector2 moveInput;
    Color32 deadColor = new(255, 165, 165, 255);

    SpriteRenderer mySpriteRenderer;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidbody      = GetComponent<Rigidbody2D>();
        myAnimator       = GetComponent<Animator>();
        myBodyCollider   = GetComponent<CapsuleCollider2D>();
        myFeetCollider   = GetComponent<BoxCollider2D>();
        regularGravityScale = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (isAlive)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))
                && value.isPressed)
            {
                // do jumping
                myRigidbody.velocity += new Vector2(0, jumpVelocity );
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (isAlive)
        {
            if (value.isPressed)
            {
                Instantiate(arrow, weaponSpot.position, transform.rotation);
            }
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x  * runVelocity * Time.deltaTime,
                                            myRigidbody.velocity.y);

        myRigidbody.velocity = playerVelocity;
        
        // Animator set up
        if (Mathf.Abs(playerVelocity.x) > Mathf.Epsilon)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else 
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        { // Climbing a ladder
            myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 
                                    moveInput.y * climbVelocity * Time.deltaTime);

            // Animator set up
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isClimbing", true);
            if (Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon)
            {
                // Unfreeze animation
                myAnimator.speed = 1;
            }
            else 
            {
                // Freeze animation
                myAnimator.speed = 0;
            }

            // Fixing sliding
            myRigidbody.gravityScale = 0;
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
            myAnimator.speed = 1;
            myRigidbody.gravityScale = regularGravityScale;
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;

            // Death Animation
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = new Vector2(0, deathAnimationSpeed);
            mySpriteRenderer.color = deadColor;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.collider.tag == "Enemy")
    //     {
    //         Die();
    //     }
    // }
}
