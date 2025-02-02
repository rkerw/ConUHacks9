using FishNet.Component.Transforming;
using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField]
    private Vector3 moveInput;
    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isFacingRight = true;

    public void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public void UpdateMovement()
    {
       moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
       if (moveInput.magnitude > 1)
       {
           moveInput.Normalize();
       }
    }

    private void FixedUpdate()
    {
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        var newPosition = transform.position + (moveInput * moveSpeed * Time.fixedDeltaTime);
        rb2d.MovePosition(newPosition);

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }

        if (moveInput.x > 0 && !isFacingRight)
        {
            flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            flip();
        }
        
    }
}
