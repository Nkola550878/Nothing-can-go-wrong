using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float groundDetectionDistance;
    [SerializeField] float wallDetectionDistance;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float slowDownFactor;

    float velocity;
    float direction;
    int lastDirection;
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
        #region Movement

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

        #endregion

        #region Movement fix

        if(direction != 0)
        {
            lastDirection = direction > 0 ? 1 : -1;
        }

        //Debug.DrawRay(transform.position - Vector3.up * (boxCollider.size.y * transform.localScale.y / 2), Vector2.right * direction * (boxCollider.size.x * transform.localScale.x / 2 + wallDetectionDistance), Color.red);
        if (Physics2D.Raycast(transform.position - Vector3.up * (boxCollider.size.y * transform.localScale.y / 2), Vector2.right * lastDirection, boxCollider.size.x * transform.localScale.x / 2 + wallDetectionDistance, groundLayerMask))
        {
            velocity = 0;
        }

        rb.velocity = new Vector2(velocity, rb.velocity.y);

        #endregion

        #region Jump

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        #endregion
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
