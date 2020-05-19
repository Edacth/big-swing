using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxType;

public class PlayerWeaponBox : MonoBehaviour
{
    public delegate void Triggered(TriggerInfo triggerInfo);
    public Triggered triggered = null;
    
    Transform playerTrans;
    public bool TriggerEnabled { get; set; }

    public void Init(Transform _playerTrans)
    {
        playerTrans = _playerTrans;
    }

    private void Update()
    {
        Debug.Log(playerTrans.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TriggerEnabled) { return; }
        if (other.CompareTag("Hitbox"))
        {
            WeaponInfo weaponInfo = new WeaponInfo("sword", 5, playerTrans.position);
            other.GetComponent<EnemyHitbox>().RecieveHit(weaponInfo);

            triggered(new TriggerInfo(BOXTYPE.Hitbox));
        }
        else if (other.CompareTag("Toughbox"))
        {
            triggered(new TriggerInfo(BOXTYPE.Toughbox));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!TriggerEnabled) { return; }
        Debug.Log(collision.gameObject.name);
    }
}

public struct WeaponInfo // Used for sending info to enemy hitboxes
{
    public WeaponInfo(string _weaponType, int _baseDamage, Vector3 _attackerPos)
    {
        weaponType = _weaponType;
        baseDamage = _baseDamage;
        attackerPos = _attackerPos;
    }

    public string weaponType;
    public int baseDamage;
    public Vector3 attackerPos;
}

public struct TriggerInfo // Used for sending info to the player swing script
{
    public TriggerInfo(BOXTYPE _boxType)
    {
        boxType = _boxType;
    }
    
    public BOXTYPE boxType;
}