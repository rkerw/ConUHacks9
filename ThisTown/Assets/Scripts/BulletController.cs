using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class BulletController : NetworkBehaviour
{

    int bulletDamage;  // Specific to player's loadout

    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
