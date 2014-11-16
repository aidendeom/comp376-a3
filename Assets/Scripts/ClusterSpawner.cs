using UnityEngine;
using System.Collections;

public class ClusterSpawner : MonoBehaviour
{
    public Transform SpawnAnchor;

    public void Spawn(GameObject go)
    {
        var b = (GameObject)Instantiate(go, SpawnAnchor.position, SpawnAnchor.rotation);
        b.rigidbody.velocity = SpawnAnchor.up * UnityEngine.Random.Range(10f, 30f);
    }
}
