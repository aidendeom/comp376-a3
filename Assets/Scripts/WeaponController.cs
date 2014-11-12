using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Animator RightGun = null;
    public Animator LeftGun = null;
    public Transform HeadTransform = null;
    public float RoF = 10;

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
            var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = transform.position;
            bullet.transform.localScale = Vector3.one * 0.1f;
            var r = bullet.AddComponent<Rigidbody>();
            r.useGravity = false;
            r.drag = 0;
            r.velocity = HeadTransform.forward * 10;
            Destroy(bullet, 5f);
        }
    }
}
