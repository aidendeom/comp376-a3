using UnityEngine;
using System.Collections;

public class BalloonCluster : MonoBehaviour
{
    public delegate void OnPoppedDelegate(bool isCluster);

    public GameObject NextCluster;
    public Health health;

    public OnPoppedDelegate OnPopped;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnKilled = Pop;
    }

    void Pop()
    {
        if (NextCluster != null)
        {
            var c1 = (GameObject)Instantiate(NextCluster, transform.position, Quaternion.identity);
            var c2 = (GameObject)Instantiate(NextCluster, transform.position, Quaternion.identity);

            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

            c1.rigidbody.velocity = randomDirection * UnityEngine.Random.Range(1f, 5f);
            c2.rigidbody.velocity = -randomDirection * UnityEngine.Random.Range(1f, 5f);
        }

        OnPopped(NextCluster != null);

        Destroy(gameObject);
    }
}
