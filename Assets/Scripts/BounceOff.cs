using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        Rigidbody r = c.gameObject.rigidbody;
        BalloonCluster bc = c.gameObject.GetComponent<BalloonCluster>();
        if (bc != null)
        {
            Vector3 inDir = -bc.Velocity.normalized;
            Vector3 n = transform.up;
            Vector3 outDir = 2 * (Vector3.Dot(n, inDir)) * n - inDir;

            bc.Velocity = outDir * bc.Velocity.magnitude;
        }
        else if (r != null)
        {
            Vector3 inDir = -r.velocity.normalized;
            Vector3 n = transform.up;
            Vector3 outDir = 2 * (Vector3.Dot(n, inDir)) * n - inDir;

            r.velocity = outDir * r.velocity.magnitude;
        }
    }
}
