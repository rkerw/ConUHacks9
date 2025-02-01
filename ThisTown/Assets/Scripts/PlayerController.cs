using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Unity.Cinemachine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private CinemachineCamera cameraTarget;

    private Vector2 moveInput;
    private PlayerMovementController movementController;

    public override void OnStartClient()
    {
        base.OnStartClient(); 
        //disable for remote client
        if (!IsOwner)
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
            return;
        }

        movementController = GetComponent<PlayerMovementController>();
        cameraTarget.gameObject.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return; //just a safety check

        movementController?.UpdateMovement();
    }
}
