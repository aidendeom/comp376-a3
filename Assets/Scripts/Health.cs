using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public struct DamageArgs
    {
        public int amount;

        // Point at which the damage happened
        public Vector3 point;
        // Normal from that point
        public Vector3 normal;
    }

    // Health related delegates
    public delegate void OnKilledDelegate();
    public delegate void OnTakeDamageDelegate(DamageArgs args);

    // Members
    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; }
    public bool Dead { get; private set; }
    public bool Alive { get { return !Dead; } }

    public OnKilledDelegate OnKilled = null;
    public OnTakeDamageDelegate OnTakeDamage = null;

    void Start()
    {
        CurrentHealth = MaxHealth;
        Dead = false;
    }

    public void TakeDamage(DamageArgs args)
    {
        int previousHealth = CurrentHealth;
        CurrentHealth -= args.amount;

        if (OnTakeDamage != null)
            OnTakeDamage(args);

        if (CurrentHealth <= 0 && previousHealth > 0)
        {
            Dead = true;
            if (OnKilled != null)
                OnKilled();
        }
    }
}
