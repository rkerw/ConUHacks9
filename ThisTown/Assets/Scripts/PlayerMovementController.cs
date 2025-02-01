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


    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public void UpdateMovement()
    {
       moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    private void FixedUpdate()
    {
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }

        var newPosition = transform.position + (moveInput * moveSpeed * Time.fixedDeltaTime);
        rb2d.MovePosition(newPosition);
        
    }
}
