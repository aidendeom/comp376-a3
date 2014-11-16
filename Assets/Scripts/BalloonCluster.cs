using UnityEngine;
using System.Collections;

public class BalloonCluster : MonoBehaviour
{
    public delegate void OnPoppedDelegate(bool isCluster);

    public GameObject NextCluster;
    [HideInInspector]
    public Health health;
    public GameObject DamageParticlePrefab;

    public static OnPoppedDelegate BalloonPoppedEventGlobal;

    private float speedMult = 1f;
    private float sinParam = 0f;
    new private Rigidbody rigidbody;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnKilled += Pop;
        health.OnTakeDamage += OnTakeDamage;

        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(ModulateSpeedMultiplier());
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity * speedMult, 10f);
    }

    void OnTakeDamage(Health.DamageArgs args)
    {
        var p = (GameObject)Instantiate(DamageParticlePrefab, args.point, Quaternion.LookRotation(args.normal));
        Destroy(p, 5f);
    }

    void Pop()
    {
        if (NextCluster != null)
        {
            var c1 = (GameObject)Instantiate(NextCluster, transform.position, Quaternion.identity);
            var c2 = (GameObject)Instantiate(NextCluster, transform.position, Quaternion.identity);

            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

            c1.transform.position += randomDirection * c1.collider.bounds.extents.x;
            c2.transform.position += -randomDirection * c2.collider.bounds.extents.x;

            c1.rigidbody.velocity = randomDirection * UnityEngine.Random.Range(1f, 5f);
            c2.rigidbody.velocity = -randomDirection * UnityEngine.Random.Range(1f, 5f);
        }

        if (BalloonPoppedEventGlobal != null)
            BalloonPoppedEventGlobal(NextCluster != null);

        Destroy(gameObject);
    }

    IEnumerator ModulateSpeedMultiplier()
    {
        while(!health.Dead)
        {
            speedMult = Mathf.Sin(sinParam) + 1.5f;
            sinParam += Time.deltaTime;
            yield return null;
        }
    }
}
