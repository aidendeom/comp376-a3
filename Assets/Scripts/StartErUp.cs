using UnityEngine;
using System.Collections;

public class StartErUp : MonoBehaviour
{
    void Go()
    {
        GetComponentInChildren<BossController>().StartShooting();
    }
}
