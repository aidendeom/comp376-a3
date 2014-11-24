using UnityEngine;
using System.Collections;

public class BalloonCluster : MonoBehaviour
{
    public delegate void OnPoppedDelegate(bool isCluster);

    public GameObject NextCluster;
    [HideInInspector]
    public Health health;
    public GameObject DamageParticlePrefab;
    public Vector3 Velocity;
    public AudioSource Audio;

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
        Audio = GetComponent<AudioSource>();
        StartCoroutine(ModulateSpeedMultiplier());
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Velocity * speedMult * (GameController.Fast ? 1.5f : 1f);
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

            c1.GetComponent<BalloonCluster>().Velocity = randomDirection * UnityEngine.Random.Range(5f, 15f);
            c2.GetComponent<BalloonCluster>().Velocity = -randomDirection * UnityEngine.Random.Range(5f, 15f);
        }

        GameObject go = new GameObject("Pop audio");
        var source = go.AddComponent<AudioSource>();
        source.clip = Audio.clip;
        source.Play();
        Destroy(go, 1.5f);

        if (BalloonPoppedEventGlobal != null)
            BalloonPoppedEventGlobal(NextCluster != null);

        Destroy(gameObject);
    }

    IEnumerator ModulateSpeedMultiplier()
    {
        while(health.Alive)
        {
            speedMult = Mathf.Sin(sinParam) + 1.5f;
            sinParam += Time.deltaTime;
            yield return null;
        }
    }
}
