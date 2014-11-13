using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    // Health related delegates
    public delegate void OnKilledDelegate();

    // Members
    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; }
    public bool Dead { get; private set; }

    public OnKilledDelegate OnKilled = null;

    void Start()
    {
        CurrentHealth = MaxHealth;
        Dead = false;
    }

    public void TakeDamage(int amount)
    {
        int previousHealth = CurrentHealth;
        CurrentHealth -= amount;

        Debug.Log(string.Format("{0} damaged. {1} -> {2}", gameObject.name, previousHealth, CurrentHealth));

        if (CurrentHealth <= 0 && previousHealth > 0)
        {
            Dead = true;
            if (OnKilled != null)
                OnKilled();
        }
    }
}
