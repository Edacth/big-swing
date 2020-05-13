using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public delegate void Triggered(HitInfo _info);
    public Triggered triggered = null;
    [SerializeField] public string Name;

    public void RecieveHit(WeaponInfo weaponInfo)
    {
        HitInfo info = new HitInfo(Name);

        triggered?.Invoke(info);
    }
}

public struct HitInfo
{
    public HitInfo(string _name)
    {
        sourceBox = _name;
    }

    public string sourceBox;
}