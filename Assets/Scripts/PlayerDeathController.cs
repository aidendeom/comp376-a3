using UnityEngine;
using System.Collections;

public class PlayerDeathController : MonoBehaviour
{
    private Health health;
    private Transform playerTrans;

    void Start()
    {
        playerTrans = GetComponent<Transform>();
        health = GetComponent<Health>();

        health.OnKilled += OnKill;
    }

    private void OnKill()
    {
        rigidbody.useGravity = true;
        Destroy(GetComponent<FPSController>());
        var weapon = GetComponent<WeaponController>();
        weapon.StopAnims();
        Destroy(weapon);

        StartCoroutine(ThirdPersonCamera());
        Destroy(gameObject, GameController.GlobalRespawnTime);
    }

    private IEnumerator ThirdPersonCamera()
    {
        Camera cam = Camera.main;
        Transform camTrans = cam.transform;

        Camera[] cams = GameObject.FindObjectsOfType<Camera>();

        foreach (var c in cams)
        {
            if (c != cam)
            {
                c.enabled = false;
            }
        }

        camTrans.parent = null;

        while (true)
        {
            camTrans.LookAt(playerTrans.position);
            yield return null;
        }
    }
}
