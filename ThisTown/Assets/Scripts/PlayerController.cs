using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    private Vector2 moveInput;


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            // nothing atm
        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb.linearVelocity = moveInput * speed;
    }
}
