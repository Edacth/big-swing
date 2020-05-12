using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponBox : MonoBehaviour
{
    // TODO: Add tag detection and communicate to both the swing system and to the collision box information about the collisions.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
