using UnityEngine;
using System.Collections;

public class ClusterSpawner : MonoBehaviour
{
    public Transform SpawnAnchor;

    public void Spawn(GameObject go)
    {
        var b = (GameObject)Instantiate(go, SpawnAnchor.position, SpawnAnchor.rotation);
        var bc = b.GetComponent<BalloonCluster>();
        bc.Velocity = SpawnAnchor.up * UnityEngine.Random.Range(10f, 30f);
    }
}
