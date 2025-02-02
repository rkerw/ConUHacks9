using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : NetworkBehaviour
{
    [SerializeField] private float StartingHealth = 100;
    public readonly SyncVar<float> CurrentHealth = new SyncVar<float>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStartServer()
    {
        base.OnStartServer();
        CurrentHealth.Value = StartingHealth;
    }

    public void ServerApplyDamage(int damage, int damager)
    {
        if (!InstanceFinder.IsServer)
            return;

        CurrentHealth.Value -= damage;
        CurrentHealth.DirtyAll();
        if (CurrentHealth.Value <= 0)
        {
            GetComponent<PlayerController>()?.ServerOnDeath(damager);
            Debug.Log("DEAD!!!");
            return;
        }

        Debug.Log("New Health: " + CurrentHealth.Value);
    }
}
