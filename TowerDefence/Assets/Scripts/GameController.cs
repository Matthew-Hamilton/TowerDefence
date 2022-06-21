using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    GameState gameState = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TileGeneration.instance.ConstructMap(TileGeneration.instance.size);
            BuildMap();
        }
    }

   public void TogglePause()
    {
        if (gameState == GameState.PreInit)
            return;

        TMP_Text tempText = GameObject.Find("Play/Pause Button").GetComponentInChildren<TMP_Text>();

        if (gameState == GameState.PreStart)
        {
            gameState = GameState.InWave;
            tempText.text = "Pause";
            RandomiseSpawnPoint();
            StartCoroutine(EnemyController.instance.SpawnWave());
            return;
        }
        if (gameState == GameState.InWave)
        {
            gameState = GameState.Paused;
            tempText.text = "Play";
            DOTween.PauseAll();
            Time.timeScale = 0;
            return;
        }
        if (gameState == GameState.Paused)
        {
            gameState = GameState.InWave;
            tempText.text = "Pause";
            DOTween.TogglePauseAll();
            Time.timeScale = 1;
            return;
        }
    }

    void BuildMap()
    {
        TileGeneration.instance.Randomise();
        TileGeneration.instance.Smooth();
        TileGeneration.instance.Render();
        TileGeneration.GetNumHexes();
        gameState = GameState.PreStart;
    }

    bool RandomiseSpawnPoint()
    {
        PathFinding.instance.SetRandomStart();

        bool pathFound = PathFinding.instance.FindPath();
        if (pathFound)
            return pathFound;


        List<Node> tempMountainHolder = TileGeneration.instance.MountainNodes;
        tempMountainHolder.Shuffle();
        foreach (Node node in tempMountainHolder)
        {
            PathFinding.instance.startPoint = node;
            if (PathFinding.instance.FindPath())
            {
                pathFound = true;
                return pathFound;
            }
        }

        BuildMap();
        return RandomiseSpawnPoint();


    }

    public enum GameState
    {
        PreInit = 0,
        PreStart = 1,
        InWave = 2,
        WaveEnd = 3,
        GameOver = 4,
        Win = 5,
        Paused = 6
    }
}
