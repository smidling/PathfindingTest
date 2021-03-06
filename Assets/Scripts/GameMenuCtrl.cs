﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuCtrl : MonoBehaviour
{

    public static GameMenuCtrl Instance;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    [SerializeField]
    private Button buttonMenu;
    [SerializeField]
    private LongButtonPress buttonLeft;
    [SerializeField]
    private LongButtonPress buttonRight;
    [SerializeField]
    private Button buttonShoot1;
    [SerializeField]
    private Button buttonShoot2;


    [Header("Debug widgets")]
    [Space(5)]
    [SerializeField]
    private HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private GridLayoutGroup algorithmViewGrid;
    [SerializeField]
    private GameObject algorithmViewNodePrefab;

    [Header("Debug data")]
    [Space(5)]

    [SerializeField]
    private ViewNodeCtrl[,] algorithmViewNodes;
    private bool shoot = false;

    private bool listInitiated = false;


    void Start()
    {
        buttonMenu.onClick.AddListener(delegate { ButtonClickedMenu(); });
        buttonShoot1.onClick.AddListener(delegate { ButtonClickedShoot(); });
        buttonShoot2.onClick.AddListener(delegate { ButtonClickedShoot(); });
    }

    void FixedUpdate()
    {
        ControlsOverlord.Instance.SetInputs(
            buttonLeft.isPressed, buttonRight.isPressed, shoot);
        shoot = false;
    }

    void OnEnable()
    {
//        if (SettingsMenuCtrl.Instance.DebugEnabled && !listInitiated)
//        {
//            InitListNodes();
//        }

        // start the game
        Time.timeScale = 1; 
    }

    private void InitListNodes()
    {
            int size = LevelGenerator.Instance.LevelSize;
            algorithmViewNodes = new ViewNodeCtrl[size, size];
            horizontalLayoutGroup.spacing = 1080f / size - 2f; // hardcode for debug purposs
            verticalLayoutGroup.spacing = 1080f / size - 2f;
            algorithmViewGrid.cellSize = Vector2.one * 1080f / size ;

        GameObject.Instantiate(linePrefab, new Vector3(), new Quaternion(), verticalLayoutGroup.transform);
        GameObject.Instantiate(linePrefab, new Vector3(), new Quaternion(), horizontalLayoutGroup.transform);
        for (int iter = 0; iter < size; iter++)
            {
                GameObject.Instantiate(linePrefab, new Vector3(), new Quaternion(), verticalLayoutGroup.transform);
                GameObject.Instantiate(linePrefab, new Vector3(), new Quaternion(), horizontalLayoutGroup.transform);
                for (int iter2 = 0; iter2 < size; iter2++)
                {
                    GameObject go = Instantiate(algorithmViewNodePrefab, new Vector3(), new Quaternion(),
                        algorithmViewGrid.transform);
                    go.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * 1080f / size / 2f;
//                    if (!(iter == -1 || iter >= size || iter2 == -1 || iter2 >= size))
                        algorithmViewNodes[iter2, iter] = go.GetComponent<ViewNodeCtrl>();
                }
            }
        listInitiated = true;
    }

    public void DrawNodeWidgets(int enemyNum, int x, int y)
    {
        if (!listInitiated)
            InitListNodes();
        algorithmViewNodes[x, y].ActivateImage(enemyNum);
    }
    public void DrawNodeWidgets(int enemyNum, HashSet<PathfindingNode> nodes)
    {
        if(!listInitiated)
            InitListNodes();
        foreach (PathfindingNode node in nodes)
        {
            algorithmViewNodes[node.gridX, node.gridY].ActivateImage(enemyNum);
        }
    }
    public void DrawNodeWidgets(int enemyNum, List<PathfindingNode> nodes)
    {
        if (!listInitiated)
            InitListNodes();
        for (int index = 0; index < nodes.Count; index++)
        {
            algorithmViewNodes[nodes[index].gridX, nodes[index].gridY].ActivateImage(enemyNum);
        }
    }


    public void ButtonClickedMenu()
    {
        // reset game, its easier
        SceneManager.LoadScene(0);
    }
    
    public void ButtonClickedShoot()
    {
        shoot = true;
    }
}
