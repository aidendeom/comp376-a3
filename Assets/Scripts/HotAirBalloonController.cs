using UnityEngine;
using System.Collections;

public class HotAirBalloonController : MonoBehaviour
{
    public Transform TurretTrans;
    public Transform GunTrans;

    private Transform playerTrans;

    // Update is called once per frame
    void Update()
    {
        if (playerTrans == null)
        {
            FindPlayer();
        }
        else
        {
            AimTurret();
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
}
