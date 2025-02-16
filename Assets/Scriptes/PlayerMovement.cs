using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float groundDetectionDistance = .1f;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float slowDownFactor;

    float velocity;
    float direction;
    float jumpVelocity;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        jumpVelocity = Mathf.Sqrt(-2 * rb.gravityScale * jumpHeight * Physics2D.gravity.y);
    }

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if (direction == 0 && Grounded())
        {
            velocity *= slowDownFactor;
        }
        if(Mathf.Abs(velocity) < 0.01)
        {
            velocity = 0;
        }

        velocity += direction * movementSpeed;
        velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(velocity, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }

    bool Grounded()
    {
        //Debug.DrawRay(transform.position + transform.localScale.x / 2 * Vector3.right, Vector2.down * (boxCollider.size.y * transform.localScale.y / 2 + groundDetectionDistance), Color.red);
        return 
            Physics2D.Raycast(transform.position + transform.localScale.x / 2 * Vector3.right, Vector2.down, boxCollider.size.y * transform.localScale.y / 2 + groundDetectionDistance, groundLayerMask) ||
            Physics2D.Raycast(transform.position - transform.localScale.x / 2 * Vector3.right, Vector2.down, boxCollider.size.y * transform.localScale.y / 2 + groundDetectionDistance, groundLayerMask);
    }

    void Jump()
    {
        if (Grounded())
        {
            rb.velocity += Vector2.up * jumpVelocity;
        }
    }
}
