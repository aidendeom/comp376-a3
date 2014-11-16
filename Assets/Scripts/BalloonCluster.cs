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
        Vector3 newVel = Vector3.ClampMagnitude(rigidbody.velocity, 30f);

        if (GameController.Fast)
        {
            newVel *= 1.5f;
        }

        rigidbody.velocity = newVel;

    }

    void OnCollisionEnter(Collision c)
    {
        var h = c.gameObject.GetComponent<Health>();
        if (c.gameObject.tag == "Player" && h != null)
        {
            Health.DamageArgs args = new Health.DamageArgs
            {
                amount = 100,
                point = c.contacts[0].point,
                normal = c.contacts[0].normal
            };
            h.TakeDamage(args);
            Pop();
        }
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

            c1.rigidbody.velocity = randomDirection * UnityEngine.Random.Range(10f, 30f);
            c2.rigidbody.velocity = -randomDirection * UnityEngine.Random.Range(10f, 30f);
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
