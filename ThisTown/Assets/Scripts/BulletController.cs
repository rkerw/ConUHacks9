using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : NetworkBehaviour
{

    [SerializeField] private float moveSpeed;

    int damageToDeal=1;
    Rigidbody2D rb2D;
    bool initialised = false;
    int SpawnerId;
    int OwnerConn;

    public override void OnStartServer()
    {
        base.OnStartServer();
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(ServerKillRoutine());
    }

    IEnumerator ServerKillRoutine()
    {
        yield return new WaitForSeconds(10);
        this.Despawn();
    }

    private void FixedUpdate()
    {
        if (rb2D)
        {
            rb2D.MovePosition(transform.position + transform.right * moveSpeed * Time.fixedDeltaTime);
        }
       
    }

    public void Initialise(int damage, int Spawner, int ownerConnection)
    {
        damageToDeal = damage;
        initialised = true;
        SpawnerId = Spawner;
        OwnerConn = ownerConnection;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!InstanceFinder.IsServer)
            return;

        if (!initialised)
            return;

        var nob = collider.GetComponent<NetworkObject>();
        if (nob != null && nob.ObjectId == SpawnerId)
            return;

        Debug.Log("Bullet Hit Something");
        var healthComp = collider.GetComponent<Health>();
        if(healthComp == null) {
            return;
        }

        healthComp.ServerApplyDamage(damageToDeal, OwnerConn);
        Debug.Log("Hit Player " + healthComp.gameObject.name);
        Despawn();
        //Deal damage here.
    }
}
