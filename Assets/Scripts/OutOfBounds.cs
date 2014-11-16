using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour
{
    public bool x,
                y,
                z;

    void OnCollisionEnter(Collision c)
    {
        Vector3 pos = c.gameObject.transform.position;
        if (x)
        {
            pos.x *= -0.95f;
        }
        else if (y)
        {
            pos.y *= -0.95f;
        }
        else if (z)
        {
            pos.z *= -0.95f;
        }

        c.gameObject.transform.position = pos;
    }
}
