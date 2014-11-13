using UnityEngine;
using System;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Animator RightGun = null;
    public Animator LeftGun = null;
    public Transform ShootTransform = null;
    public float RoF = 10;
    public int Damage = 1;

    private float SecondsPerShot
    {
        get { return 1 / RoF; }
    }

    private float lastShot = 0;

    private bool CanShoot
    {
        get { return Time.time - lastShot > SecondsPerShot; }
    }

    void Update()
    {
        bool shoot = Input.GetMouseButton(0);

        RightGun.SetBool("Shoot", shoot);
        LeftGun.SetBool("Shoot", shoot);

        if (shoot && CanShoot)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Balloon");

            var hits = Physics.RaycastAll(ShootTransform.position, ShootTransform.forward, 10000f, layerMask);

            if (hits.Length > 0)
            {
                Array.Sort(hits, new Comparison<RaycastHit>((i, j) =>
                {
                    if (Mathf.Approximately(i.distance, j.distance))
                        return 0;
                    else if (i.distance < j.distance)
                        return -1;
                    else
                        return 1;
                }));

                // This is the first balloon hit
                var hit = hits[0];

                var health = hit.collider.GetComponent<Health>();
                if (health != null)
                    health.TakeDamage(Damage);
            }
        }
    }
}
