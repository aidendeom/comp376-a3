using UnityEngine;
using System.Collections;

public class ClusterSpawner : MonoBehaviour
{
    public Transform SpawnAnchor;

    public void Spawn(GameObject go)
    {
        Instantiate(go, SpawnAnchor.position, SpawnAnchor.rotation);
    }
}
