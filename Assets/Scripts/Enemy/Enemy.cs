using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyToughbox[] toughboxes = null;
    [SerializeField] EnemyHitbox[] hitboxes = null;

    protected virtual void OnHitboxTrigger(HitInfo info) { }

    public virtual void TakeDamage(int _damageAmount) { }

    protected void HitboxSetup()
    {
        for (int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i].triggered += OnHitboxTrigger;
        }
    }
}
