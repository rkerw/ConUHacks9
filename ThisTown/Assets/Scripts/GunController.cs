using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet;
using FishNet.Managing;

public class GunController : NetworkBehaviour
{
    public struct BulletSpawnInfo
    {
        public int Owner;
        public int OwnerConn;
        public float BulletsPerShot;
        public float BulletSpread;
        public int DamagePerBullet;
    }

    public NetworkObject projectilePrefab;
    public Transform firePoint;

    [SerializeField] GunParameters gunParameters;

    float nextFire = 0;

    public bool CanFire { get; set; } = true;


    void Start()
    {
        firePoint = gameObject.GetComponent<Rigidbody2D>().transform;  //TODO: change
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner && Input.GetMouseButton(0) && Time.time >= nextFire && CanFire)
        {
            if(gunParameters == null)
                gunParameters = Resources.Load<GunParameters>(HackyMemory.GetPlayerWeapons()[Owner.ClientId]);

            nextFire = Time.time + (60f / gunParameters.FireRate);
            CalculateBulletsLeft();
            ServerRPC_Shoot(new BulletSpawnInfo
            {
                Owner = this.ObjectId,
                OwnerConn = this.Owner.ClientId,
                BulletsPerShot = gunParameters.TotalBulletsPerShot,
                BulletSpread = gunParameters.BulletSpreadInAngles,
                DamagePerBullet = gunParameters.DamagePerBullet
            });
        }
    }


    //Do all that shizz
    void CalculateBulletsLeft()
    {

    }

    [ServerRpc(RequireOwnership =true)]
    void ServerRPC_Shoot(BulletSpawnInfo spawnInfo)
    {
        var networkManager = InstanceFinder.NetworkManager;
        for(int i = 0;i< spawnInfo.BulletsPerShot;i++)
        {
            var rotation = firePoint.rotation.eulerAngles;
            if (spawnInfo.BulletSpread > 0 && spawnInfo.BulletsPerShot > 1)
            {
                float angleOffset = Mathf.Lerp(-spawnInfo.BulletSpread / 2, spawnInfo.BulletSpread / 2, (float)i / (spawnInfo.BulletsPerShot - 1));
                rotation.z += angleOffset;
            }

            NetworkObject nob = networkManager.GetPooledInstantiated(projectilePrefab, firePoint.position, Quaternion.Euler(rotation), true);
            networkManager.ServerManager.Spawn(nob, null);

            nob.GetComponent<BulletController>()?.Initialise(spawnInfo.DamagePerBullet, spawnInfo.Owner, spawnInfo.OwnerConn);
        }
    }

}
