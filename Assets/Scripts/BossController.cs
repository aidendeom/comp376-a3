using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public Transform ProjectileAnchor;
    public GameObject ProjectilePrefab;
    public AnimationClip DanceAnim;
    public AnimationClip DieAnim;
    public float ShotCooldown = 1f;

    public bool CanAttack = false;

    private Health _health;
    private Transform _playerTrans;

    void Start()
    {
        _health = GetComponent<Health>();
        _health.OnKilled += OnKill;
        _health.Invincible = true;
    }

    public void StartShooting()
    {
        _health.Invincible = false;
        StartCoroutine("AttackCoroutine");
        var anim = GetComponent<Animation>();
        anim.clip = DanceAnim;
        anim.CrossFade("dance", 1f);
    }

    private void OnKill()
    {
        var anim = GetComponent<Animation>();
        anim.clip = DieAnim;
        anim.CrossFade("die", 1f);
        audio.Play();
    }

    IEnumerator AttackCoroutine()
    {
        while (_health.Alive)
        {
            if (_playerTrans != null)
            {

                var p = (GameObject)Instantiate(ProjectilePrefab, ProjectileAnchor.position, UnityEngine.Random.rotation);
                var r = p.GetComponent<Rigidbody>();
                r.velocity = (_playerTrans.position - ProjectileAnchor.position).normalized * 30f;
            }
            else
            {
                var go = GameObject.FindWithTag("Player");
                if (go != null)
                    _playerTrans = go.transform;
            }
            yield return new WaitForSeconds(ShotCooldown);
        }
    }
}
