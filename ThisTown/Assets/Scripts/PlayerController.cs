using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Unity.Cinemachine;
using System;
using FishNet;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private CinemachineCamera cameraTarget;

    private Vector2 moveInput;
    private PlayerMovementController movementController;
    private Health healthComp;

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
        healthComp = GetComponent<Health>();
        //healthComp.OnDeath += ServerOnDeath;
        //cameraTarget.gameObject.SetActive(true);
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

    public void ServerOnDeath(int killer)
    {
        if (!InstanceFinder.NetworkManager.IsServer)
            return;

        Debug.Log("Player Won: " + killer);
        ShooterGameController.Instance.SetRoundWinServer(killer);
        Despawn();
    }
}
