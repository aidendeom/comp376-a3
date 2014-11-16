using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Ongoing, 
        Won,
        Lost
    }

    public GameObject ClusterToSpawn = null;
    public int BalloonsPerCluster = 32;
    public float ClusterSpawnDelay = 10f;
    public int NumberOfClustersToSpawn = 4;

    public int CurrentScore = 0;

    public int ClusterScore = 2;
    public int SingleBalloonScore = 1;
    public int HotAirBalloonScore = 10;

    public Font GUIFont;

    private int totalBalloons;
    private int balloonsPopped = 0;

    private int totalSplits;
    private int currentSplits = 0;

    private ClusterSpawner[] spawners;

    private GameState currentState = GameState.Ongoing;

    void Start()
    {
        BalloonCluster.BalloonPoppedEventGlobal += OnBalloonPop;

        totalBalloons = NumberOfClustersToSpawn * BalloonsPerCluster;

        // TODO: This should be calculated!!!
        totalSplits = 63 * NumberOfClustersToSpawn;

        spawners = GameObject.FindObjectsOfType<ClusterSpawner>();

        StartCoroutine("SpawnClusterCoroutine");
    }

    public void OnBalloonPop(bool isCluster)
    {
        currentSplits++;

        if (isCluster)
        {
            CurrentScore += ClusterScore;
        }
        else
        {
            CurrentScore += SingleBalloonScore;
            balloonsPopped++;
        }

        if (balloonsPopped >= totalBalloons)
        {
            currentState = GameState.Won;
        }
    }

    private IEnumerator SpawnClusterCoroutine()
    {
        for (int i = 0; i < NumberOfClustersToSpawn; i++)
        {
            int choice = UnityEngine.Random.Range(0, spawners.Length);
            spawners[choice].Spawn(ClusterToSpawn);

            yield return new WaitForSeconds(ClusterSpawnDelay);
        }
    }

    void OnGUI()
    {
        // Score
        Rect scorePos = new Rect(10, 10, 300, 50);
        string scoreString = string.Format("Score: {0}\n{1:P}", CurrentScore, (float)currentSplits / totalSplits);
        GUIContent scoreContent = new GUIContent(scoreString);
        GUIStyle scoreStyle = new GUIStyle();
        scoreStyle.fontSize = 60;
        scoreStyle.font = GUIFont;
        GUI.Label(scorePos, scoreContent, scoreStyle);

        // Victory / Loss
        if (currentState != GameState.Ongoing)
        {
            Rect resultPos = new Rect(Screen.width / 2f, Screen.height / 2f, 10, 10);
            string resultString = "Nothing!";
            if (currentState == GameState.Won)
                resultString = "You Win!";
            else if (currentState == GameState.Lost)
                resultString = "You Lose!";

            GUIContent resultContent = new GUIContent(resultString);
            GUIStyle resultStyle = new GUIStyle();
            resultStyle.font = GUIFont;
            resultStyle.fontSize = 100;
            resultStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(resultPos, resultContent, resultStyle);
        }
    }
}
