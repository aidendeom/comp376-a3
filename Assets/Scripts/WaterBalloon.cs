﻿using UnityEngine;
using System.Collections;

public class WaterBalloon : MonoBehaviour
{
    public int Damage = 100;
    [HideInInspector]
    public Health Health;

    void Start()
    {
        Health = GetComponent<Health>();
        Health.OnKilled += OnKill;
    }

    private void OnKill()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision c)
    {
        var h = c.collider.gameObject.GetComponent<Health>();

        if (h != null)
        {
            Health.DamageArgs args = new Health.DamageArgs
            {
                amount = Damage,
                point = c.contacts[0].point,
                normal = c.contacts[0].normal
            };

            h.TakeDamage(args);
        }

        Destroy(gameObject);
    }
}
