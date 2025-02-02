using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGameController : NetworkBehaviour
{

    [SerializeField] List<GunParameters> gunParameters;
    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(StartupRoutine());
    }

    IEnumerator StartupRoutine()
    {
        yield return new WaitForSeconds(1);
        ServerSupplyWeapons();
        yield return new WaitForSeconds(0.1f);
        GameStateMananger.Instance?.SwitchState(GameStateMananger.GameState.Shooter);
    }

    private void ServerSupplyWeapons()
    {
        Debug.Log("Supplying Weapons");
        StartCoroutine(WeaponSupplyRoutine());
    }

    IEnumerator WeaponSupplyRoutine()
    {
        var list = new List<string>();
        foreach (var kv in InstanceFinder.ServerManager.Clients)
        {
            var rand = Random.Range(0, gunParameters.Count);
            Debug.Log($"GUN SELECTION: {kv.Value.ClientId}:{gunParameters[rand].name}");
            list.Add($"{kv.Value.ClientId}:{gunParameters[rand].name}");
            yield return new WaitForEndOfFrame();
        }
        UpdateWeaponsInfo(list);
    }

    [ObserversRpc]
    void UpdateWeaponsInfo(List<string> weapons)
    {
        Dictionary<int, string> data = new Dictionary<int, string>();
        foreach(var weapon in weapons)
        {
            string[] split = weapon.Split(':');
            data.Add(int.Parse(split[0]), split[1]);
        }
        HackyMemory.SetPlayerWeapon(data);
    }

}
