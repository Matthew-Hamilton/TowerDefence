using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

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
        
    }

    public enum GameState
    {
        PreStart = 0,
        InWave = 1,
        WaveEnd = 2,
        GameOver = 3,
        Win = 4
    }
}
