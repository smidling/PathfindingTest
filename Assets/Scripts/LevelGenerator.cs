using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGenerator : MonoBehaviour
{
    // singleton
    public static LevelGenerator Instance;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    // exposed members
    [SerializeField] private Camera gameCamera;

    [SerializeField] private GameObject groundTilePrefab;
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject spawnPointPrefab;
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private Transform groundParentTr;

    [SerializeField]
    private LineRenderer[] lineRenderers;

    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject baseGO;
    [SerializeField] private List<GameObject> enemiesGos;
    
    // Private members
    private const int minLevelSize = 5;
    private const int maxLevelSize = 20;
    private Vector2 startPoint;

    private List<PathfindingNode> path;
    private PathfindingNode[,] levelNodes;
    
    private const int enemyCount = 4; 
    private int levelSize = 20;
    private int levelDensity = 50;

    public int MinLevelSize
    {
        get { return minLevelSize; }
    }

    public int MaxLevelSize
    {
        get { return maxLevelSize; }
    }

    public int LevelSize
    {
        get { return levelSize; }
        set { levelSize = value; }
    }

    public int LevelDensity
    {
        get { return levelDensity; }
        set { levelDensity = value; }
    }

    public PathfindingNode GetBaseNode()
    {
        return GetNode(baseGO.transform);
    }

    public void StartTheGame()
    {
//        Time.timeScale = 0;
        SettingsMenuCtrl.Instance.RefreshSettings();
        GenerateLevel(levelSize);
        Camera.main.cullingMask ^=
                SettingsMenuCtrl.Instance.DebugEnabled ? 1 << LayerMask.NameToLayer("Debug")
                : 0 << LayerMask.NameToLayer("Debug");
    }
    
    void GenerateLevel(int size)
    {
        size = Mathf.Clamp(size, minLevelSize, maxLevelSize);
        if (!gameCamera)
            gameCamera = Camera.main;
        if(gameCamera)
            gameCamera.orthographicSize = size/2f * 1920 / 1080 / Screen.width * Screen.height;
        startPoint = Vector2.one * levelSize / 2 * -1;
        
        levelNodes = new PathfindingNode[size, size];

        //create playing field
        for (int vert = 0; vert < size; vert++)
        {
            for (int hor = 0; hor < size; hor++)
            {
                GameObject.Instantiate(groundTilePrefab, new Vector3(hor, vert, 0), new Quaternion(), groundParentTr);
                levelNodes[hor, vert] = new PathfindingNode(true, hor, vert);
            }
        }
        
        // create maze
        for (int vert = -1; vert <= size; vert++)
        {
            // add left and right level border
            GameObject.Instantiate(wallTilePrefab, new Vector3(-1, vert, 0), new Quaternion(), groundParentTr);
            GameObject.Instantiate(wallTilePrefab, new Vector3(size, vert, 0), new Quaternion(), groundParentTr);
            

            int predefinedHole = Random.Range(0, size-1);
            for (int hor = 0; hor < size; hor++)
            {
                // full bottom and top border
                if (vert == -1 || vert == size)
                {
                    GameObject.Instantiate(wallTilePrefab, new Vector3(hor, vert, 0), new Quaternion(), groundParentTr);
                    continue;
                }

                // every second empty
                if (vert % 2 == 0)
                    continue;

                // ensure there is a hole in every wall
                if (predefinedHole == hor)
                    continue;
                // generate random walls
                //todo implement here random destructable walls
                if (Random.Range(0, 100) < levelDensity)
                {
                    GameObject.Instantiate(wallTilePrefab, new Vector3(hor, vert, 0), new Quaternion(), groundParentTr);
                    levelNodes[hor, vert].walkable = false;
                }
            }
        }

        // place base & player
        int baseLocation = Random.Range(0, size-1);
        baseGO.transform.localPosition = new Vector3(baseLocation, 0, 0);
        playerGO.transform.localPosition = new Vector3(baseLocation, 0, 0);

        // place enemies
        enemiesGos = new List<GameObject>();
        int ver = size; // upper half of level only
        for (int iter = 0; iter < enemyCount; iter++)
        {
            ver--;
            int hor = Random.Range(0, size - 1);
            int startHor = hor; 
            while (levelNodes[hor, ver].walkable == false)
            {
                // find empty space for enemy
                hor++;
                if (hor == size)
                    hor = 0;
                if(hor == startHor)
                    continue;
            }
            bool randAlgorithm = Random.Range(0, 100) < 50;
            // spawn enemy
            enemiesGos.Add(GameObject.Instantiate(randAlgorithm ? enemyPrefabs[1] : enemyPrefabs[0],
                new Vector3(hor, ver, 0), new Quaternion(), groundParentTr));
            
            // start enemy pathfinding
            EnemyAI tempAi = enemiesGos[iter].GetComponent<EnemyAI>();
            tempAi.StartPathfinding(randAlgorithm, iter, levelNodes[hor, ver], GetNode(baseGO.transform));
            }
        // center playing field 
        groundParentTr.position = new Vector3(startPoint.x, startPoint.y, 0);
    }

    private float pathfindingStep = 0.1f;


    public void DrawPath(int enemyNum, List<PathfindingNode> myPath, PathfindingNode startNode )
    {
        StartCoroutine(DrawPathSequental(enemyNum, myPath, startNode));
    }

    IEnumerator DrawPathSequental(int enemyNum, List<PathfindingNode> myPath, PathfindingNode startNode)
    {
        // draw lines
        int nodeIter = 0;
        // brute overide to for line to start from enemy
        lineRenderers[enemyNum].positionCount = 1;
        lineRenderers[enemyNum].SetPosition(nodeIter++, GetWorldPos(startNode) + Vector3.one / 2f);
        foreach (var node in myPath)
        {
            yield return new WaitForSeconds(0.1f);
            lineRenderers[enemyNum].positionCount = nodeIter + 1;
            lineRenderers[enemyNum].SetPosition(nodeIter++, GetWorldPos(node) + Vector3.one / 2f);
        }
    }
    
    public PathfindingNode GetNode(Transform goPos)
    {
        return levelNodes[(int)goPos.position.x, (int)goPos.position.y];
    }

    public Vector3 GetWorldPos(PathfindingNode node)
    {
        return new Vector3(node.gridX, node.gridY, 0f);
    }
    

    public PathfindingNode[,] CopyLevelNodes()
    {
        PathfindingNode[,] newArrayNodes = new PathfindingNode[LevelSize, LevelSize];
        //        newArrayNodes = levelNodes.Clone() as PathfindingNode[,];
        for (int y = 0; y < LevelSize; y++) {
            for (int x = 0; x < LevelSize; x++)
            {
                newArrayNodes[x, y] = levelNodes[x, y].CopyNode();
            }
        }
        return newArrayNodes;
    }
}
