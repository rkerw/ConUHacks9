using UnityEngine;

[CreateAssetMenu(fileName = "GunParameters", menuName = "Scriptable Objects/GunParameters")]
public class GunParameters : ScriptableObject
{

    public string GunName;
    public float FireRate;
    public float MagazineSize;
    public float TotalMagazines;

    public float TotalBulletsPerShot;
    public float BulletSpreadInAngles;

    public int DamagePerBullet = 1;
}
