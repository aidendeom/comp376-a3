using UnityEngine;
using System.Collections;

public class HotAirBalloonController : MonoBehaviour
{
    public Transform TurretTrans;
    public Transform GunTrans;
    public GameObject WaterBalloonPrefab;
    public float ShotDelay = 1.5f;
    public float ShotPower = 10f;
    public float MoveSpeed = 5f;
    public GameObject DamagePrefab;
    public GameObject DestroyPrefab;
    public AudioClip FireClip;
    public AudioClip DestroyClip;

    private Transform playerTrans;
    new private Transform transform;
    new private Rigidbody rigidbody;
    private Health health;
    private float yCoord;

    void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();

        health = GetComponent<Health>();
        health.OnTakeDamage += OnTakeDamage;
        health.OnKilled += OnKill;

        yCoord = transform.position.y;

        StartCoroutine(FireTurret());
    }

    void Update()
    {
        if (playerTrans == null)
        {
            FindPlayer();
        }
        else
        {
            AimTurret();
            UpdatePosition();
        }
    }

    private void FindPlayer()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTrans = playerGO.GetComponent<Transform>();
        }
    }

    private void AimTurret()
    {
        Vector3 playerPlanePos = playerTrans.position;
        playerPlanePos.y = TurretTrans.position.y;

        TurretTrans.LookAt(playerPlanePos);
    }

    private void UpdatePosition()
    {
        Vector3 playerPlanePos = playerTrans.position;
        playerPlanePos.y = transform.position.y;

        Vector3 dir = (playerPlanePos - transform.position).normalized;
        rigidbody.AddForce(dir * MoveSpeed, ForceMode.Acceleration);

        if (health.Alive)
        {
            Vector3 pos = transform.position;
            pos.y = yCoord;
            transform.position = pos;
        }
    }

    private IEnumerator FireTurret()
    {
        while (health.Alive)
        {
            if (playerTrans != null)
            {
                var b = (GameObject)Instantiate(WaterBalloonPrefab, GunTrans.position, Quaternion.identity);

                Vector3 shotDirection = (playerTrans.position - GunTrans.position).normalized;

                b.rigidbody.velocity = shotDirection * ShotPower;

                GameObject go = new GameObject("Fire audio");
                var source = go.AddComponent<AudioSource>();
                source.clip = FireClip;
                source.Play();
                Destroy(go, 2f);

                yield return new WaitForSeconds(ShotDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnTakeDamage(Health.DamageArgs args)
    {
        var particles = (GameObject)Instantiate(DamagePrefab, args.point, Quaternion.identity);

        Destroy(particles, 2f);
    }

    private void OnKill()
    {
        var particles = (GameObject)Instantiate(DestroyPrefab,
            transform.position + transform.forward * 3,
            Quaternion.identity);

        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForceAtPosition(rigidbody.velocity * 10f, transform.position + Vector3.up * 5f);

        particles.transform.parent = transform;

        gameObject.layer = 0;

        GameObject go = new GameObject("PExplosion audio");
        var source = go.AddComponent<AudioSource>();
        source.clip = DestroyClip;
        source.Play();
        Destroy(go, 5f);

        Destroy(particles, 5f);

        Destroy(gameObject, 5f);
    }
}
