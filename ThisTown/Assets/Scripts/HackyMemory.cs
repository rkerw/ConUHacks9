using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HackyMemory : MonoBehaviour
{
    static Dictionary<int, string> _playerWeapons = new Dictionary<int, string>();
    static int _winner;

    public static void SetPlayerWeapon(Dictionary<int, string> weaponsPath)
    {
        _playerWeapons = weaponsPath;
        Debug.Log("Memory Updated: PLAYER WEAPONS");
    }

    public static void SetWinner(int winner)
    {
        _winner = winner;
        Debug.Log("Memory Updated: WINNER");
    }

    public static Dictionary<int, string> GetPlayerWeapons()
    {
        return _playerWeapons;
    }

    public static int GetWinner()
    {
        return _winner;
    }

}
