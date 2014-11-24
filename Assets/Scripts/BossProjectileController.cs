using UnityEngine;
using System.Collections;

public class BossProjectileController : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            var h = c.gameObject.GetComponent<Health>();

            h.TakeDamage(new Health.DamageArgs { amount = 100, normal = c.contacts[0].normal, point = c.contacts[0].point });
        }

        Destroy(gameObject);
    }
}
