using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject ClusterToSpawn = null;
    public int BalloonsPerCluster = 32;
    public float ClusterSpawnDelay = 10f;
    public int NumberOfClustersToSpawn = 4;

    public int CurrentScore = 0;

    public int ClusterScore = 2;
    public int SingleBalloonScore = 1;
    public int HotAirBalloonScore = 10;

    private int totalBalloons;
    private int balloonsPopped = 0;

    void Start()
    {
        totalBalloons = NumberOfClustersToSpawn * BalloonsPerCluster;
    }

    public void OnBalloonPop(bool isCluster)
    {
        balloonsPopped++;

        if (isCluster)
        {
            CurrentScore += ClusterScore;
        }
        else
        {
            CurrentScore += SingleBalloonScore;
        }

        if (balloonsPopped >= totalBalloons)
        {
            // You win!
        }
    }

    private IEnumerator SpawnClusterCoroutine()
    {
        for (int i = 0; i < NumberOfClustersToSpawn; i++)
        {
            yield return new WaitForSeconds(ClusterSpawnDelay);
            var go = (GameObject)Instantiate(ClusterToSpawn);
            var cluster = go.GetComponent<BalloonCluster>();

            cluster.OnPopped = OnBalloonPop;
        }
    }
}
