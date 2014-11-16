using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Ongoing,
        Won,
        Lost
    }

    public static bool Fast = false;
    public static float GlobalRespawnTime;

    public GameObject PlayerPrefab;
    public float RespawnTime = 3f;
    public int PlayerLives = 3;
    public GameObject ClusterToSpawn;
    public int BalloonsPerCluster = 32;
    public float ClusterSpawnDelay = 10f;
    public int NumberOfClustersToSpawn = 4;

    public int CurrentScore = 0;

    public int ClusterScore = 2;
    public int SingleBalloonScore = 1;
    public int HotAirBalloonScore = 10;

    public Font GUIFont;

    public Transform HABSpawnLocation;
    public GameObject HABPrefab;

    private int totalBalloons;
    private int balloonsPopped = 0;

    private int totalSplits;
    private int currentSplits = 0;

    private int livesLeft;

    private ClusterSpawner[] spawners;

    private GameState currentState = GameState.Menu;

    private bool firstHAB = false,
                 secondHAB = false,
                 thirdHAB = false;

    void Start()
    {
        BalloonCluster.BalloonPoppedEventGlobal += OnBalloonPop;

        totalBalloons = NumberOfClustersToSpawn * BalloonsPerCluster;

        // TODO: This should be calculated!!!
        totalSplits = 63 * NumberOfClustersToSpawn;

        GlobalRespawnTime = RespawnTime;

        spawners = GameObject.FindObjectsOfType<ClusterSpawner>();

        StartMenu();
    }

    private void StartMenu()
    {
        livesLeft = PlayerLives;
        currentSplits = 0;
        CurrentScore = 0;
        currentState = GameState.Menu;

        BalloonCluster[] balloons = GameObject.FindObjectsOfType<BalloonCluster>();
        foreach (var b in balloons)
        {
            Destroy(b);
        }

        HotAirBalloonController[] HABs = GameObject.FindObjectsOfType<HotAirBalloonController>();
        foreach (var b in HABs)
        {
            Destroy(b);
        }

        StartCoroutine("MenuCoroutine");
    }

    private void StartGame()
    {
        currentState = GameState.Ongoing;

        Respawn();

        StartCoroutine("SpawnClusterCoroutine");
    }

    public void OnBalloonPop(bool isCluster)
    {
        currentSplits++;

        if (currentSplits / (float)totalSplits >= .8f)
        {
            Fast = true;
        }

        TrySpawnHAB();

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

    void TrySpawnHAB()
    {
        float percent = currentSplits / (float)totalSplits;
        if (percent >= .3 && !firstHAB)
        {
            Instantiate(HABPrefab, HABSpawnLocation.position, Quaternion.identity);
            firstHAB = true;
        }
        else if (percent >= .6 && !secondHAB)
        {
            Instantiate(HABPrefab, HABSpawnLocation.position, Quaternion.identity);
            secondHAB = true;
        }
        else if (percent >= .9 && !thirdHAB)
        {
            Instantiate(HABPrefab, HABSpawnLocation.position, Quaternion.identity);
            thirdHAB = true;
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
        if (currentState == GameState.Menu)
        {
            Rect titlePos = new Rect(Screen.width / 2f - 150, Screen.height / 2f - 200, 300, 300);

            GUIContent titleContent = new GUIContent("CUBE");
            GUIStyle titleStyle = new GUIStyle();
            titleStyle.font = GUIFont;
            titleStyle.fontSize = 100;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(titlePos, titleContent, titleStyle);

            Rect ButtonRect = new Rect(Screen.width / 2f - 150, Screen.height / 2f - 75, 300, 300);
            GUIStyle ButtonStyle = new GUIStyle();
            ButtonStyle.font = GUIFont;
            ButtonStyle.fontSize = 60;
            ButtonStyle.alignment = TextAnchor.MiddleCenter;
            ButtonStyle.normal.textColor = Color.green;

            if (GUI.Button(ButtonRect, "Start Game", ButtonStyle))
            {
                StopCoroutine("MenuCoroutine");
                StartGame();
            }
        }
        else
        {
            // Score
            Rect scorePos = new Rect(10, 10, 300, 50);
            string scoreString = string.Format("Score: {0}\n{1:P}", CurrentScore, (float)currentSplits / totalSplits);
            GUIContent scoreContent = new GUIContent(scoreString);
            GUIStyle scoreStyle = new GUIStyle();
            scoreStyle.fontSize = 60;
            scoreStyle.font = GUIFont;
            GUI.Label(scorePos, scoreContent, scoreStyle);

            // Lives
            Rect livesPos = new Rect(Screen.width - 10, 10, 0, 0);
            GUIContent livesContent = new GUIContent(string.Format("Lives: {0}", livesLeft));
            GUIStyle livesStyle = new GUIStyle();
            livesStyle.fontSize = 60;
            livesStyle.font = GUIFont;
            livesStyle.alignment = TextAnchor.UpperRight;
            GUI.Label(livesPos, livesContent, livesStyle);

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

    private void Respawn()
    {
        var player = (GameObject)Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        var h = player.GetComponent<Health>();
        h.OnKilled += OnPlayerKilled;

        Camera cam = Camera.main;
        Transform anchor = GameObject.Find("Main Camera Anchor").transform;
        Transform camTrans = cam.transform;
        camTrans.parent = anchor;
        camTrans.localPosition = Vector3.zero;
        camTrans.rotation = Quaternion.LookRotation(anchor.forward);
    }

    private void OnPlayerKilled()
    {
        livesLeft--;
        if (livesLeft > 0)
        {
            StartCoroutine(DelayRespawn());
        }
        else
        {
            currentState = GameState.Lost;
            StopCoroutine("SpawnClusterCoroutine");
            StartCoroutine(DelayMenu());
        }
    }

    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(RespawnTime);
        Respawn();
    }

    private IEnumerator DelayMenu()
    {
        yield return new WaitForSeconds(RespawnTime);
        StartMenu();
    }

    private IEnumerator MenuCoroutine()
    {
        Camera cam = Camera.main;
        Transform trans = cam.transform;

        float angle = 0f;
        float dist = 125f;
        while (true)
        {
            trans.position = new Vector3(dist * Mathf.Cos(angle * Mathf.PI),
                trans.position.y,
                dist * Mathf.Sin(angle * Mathf.PI));

            trans.LookAt(Vector3.zero);

            angle += (15 * Mathf.Deg2Rad) * Time.deltaTime;

            yield return null;
        }
    }
}
