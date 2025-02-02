using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet;

public class GunController : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int projectileSpeed = 3;

    int bulletDamage;

    void Start()
    {
        firePoint = gameObject.GetComponent<Rigidbody2D>().transform;  //TODO: change
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        InstanceFinder.ServerManager.Spawn(projectile, null);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = firePoint.forward * projectileSpeed;
        Debug.Log(firePoint.forward);
        projectile.GetComponent<BulletController>().SetDamage(bulletDamage);  // Specific to the player's loadout

        Spawn(projectile);

    }


}
